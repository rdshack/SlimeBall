using System.Collections.Generic;
using System.Numerics;
using SimMath;
using ecs;
using FixMath.NET;
using Indigo.Collision2D;

namespace Indigo.Slimeball;

public class ApplyMotionSystem : ISystem
{
  private CollisionContext _staticColliderContext;
  private Query            _query;
  private Query            _rotationQuery;
  private World            _world;

  private CollisionContext.PhysicsWorldStateInput  _physInput  = new CollisionContext.PhysicsWorldStateInput();
  private CollisionContext.PhysicsWorldStateOutput _physOutput = new CollisionContext.PhysicsWorldStateOutput();
  private ObjPool<CollisionContext.BallSweepInput> _sweepInputPool = 
    new ObjPool<CollisionContext.BallSweepInput>(CollisionContext.BallSweepInput.Create, 
                                                 CollisionContext.BallSweepInput.Reset);
  
  private EntityRepo _dataSource;

  public ApplyMotionSystem(World w, CollisionContext staticColliderContext)
  {
    _world = w;
    _dataSource = w.GetEntityRepo();
    _staticColliderContext = staticColliderContext;
    _query = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().
                          With<TimeComponent>().
                          With<PositionComponent>().
                          With<GravityComponent>().
                          With<VelocityComponent>().
                          With<CircleColliderComponent>();
    _query.SetContainsArchetypeFilter(a);

    _rotationQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a1 = w.GetArchetypes().
                    With<TimeComponent>().
                    With<RotationComponent>().
                    With<RotationVelocityComponent>().
                    With<CircleColliderComponent>();
    _rotationQuery.SetContainsArchetypeFilter(a1);
  }
  
  public void Execute()
  {
    _sweepInputPool.ReturnAll();
    _physInput.Reset();
    _physOutput.Reset();
    
    GameComponent gameComponent = _world.GetEntityRepo().GetSingletonComponent<GameComponent>();
    if (gameComponent.gamePhase == GamePhase.WaitingForAllPlayers)
    {
      return;
    }

    var rotationQueryResults = _rotationQuery.Resolve(_dataSource);
    for (int i = 0; i < rotationQueryResults.Count; i++)
    {
      IEntityData e = rotationQueryResults[i];
      TimeComponent timeComponent = e.Get<TimeComponent>();
      RotationComponent rotationComponent = e.Get<RotationComponent>();
      RotationVelocityComponent rotationVelocityComponent = e.Get<RotationVelocityComponent>();
      CircleColliderComponent ballColliderComp = e.Get<CircleColliderComponent>();
      
      Fix64 dtSec = (Fix64)timeComponent.deltaTimeMs / (Fix64)1000;

      Fix64 newVelo = rotationVelocityComponent.rotVelo;
      Fix64 dragCoef = new Fix64(ballColliderComp.angularDrag) / new Fix64(1_000);
      Fix64 veloSquared = newVelo * newVelo;
      Fix64 dragReduc =  veloSquared * dragCoef * dtSec;
      newVelo = MathUtil.MoveTowards(newVelo, Fix64.Zero, dragReduc);
      rotationVelocityComponent.rotVelo = newVelo;

      Fix64 rotDelta = rotationVelocityComponent.rotVelo * dtSec;
      rotationComponent.rot += rotDelta;
    }

    var queryResults = _query.Resolve(_dataSource);
    for(int i = 0; i < queryResults.Count; i++)
    {
      IEntityData e = queryResults[i];
      
      TimeComponent timeComponent = e.Get<TimeComponent>();
      PositionComponent positionComponent = e.Get<PositionComponent>();
      GravityComponent gravityComponent = e.Get<GravityComponent>();
      VelocityComponent ballVeloComp = e.Get<VelocityComponent>();
      CircleColliderComponent ballColliderComp = e.Get<CircleColliderComponent>();

      Fix64 dtSec = (Fix64)timeComponent.deltaTimeMs / (Fix64)1000;
      
      Fix64Vec2 newVelo = new Fix64Vec2(ballVeloComp.veloX, ballVeloComp.veloY);
      
      newVelo += new Fix64Vec2(0, gravityComponent.veloToApply);

      Fix64 dragCoef = new Fix64(ballColliderComp.drag) / new Fix64(1_000_000);
      Fix64Vec2 veloSquared = new Fix64Vec2(newVelo.x * newVelo.x, newVelo.y * newVelo.y);
      Fix64Vec2 dragReduc =  veloSquared * dragCoef;
      newVelo = newVelo.MoveTowards(Fix64Vec2.Zero, MathUtil.Length(dragReduc));

      if (ballVeloComp.maxSpeed > 0)
      {
        newVelo = MathUtil.ClampLength(newVelo, new Fix64(ballVeloComp.maxSpeed));
      }

      ballVeloComp.veloX = (int) newVelo.x;
      ballVeloComp.veloY = (int) newVelo.y;

      CollisionContext.BallSweepInput ballSweepInput = _sweepInputPool.Get();

      Fix64 rotationalVelo = Fix64.Zero;
      if (e.GetArchetype().Contains<RotationVelocityComponent>())
      {
        rotationalVelo = e.Get<RotationVelocityComponent>().rotVelo;
      }

      ballSweepInput.Init(e.GetEntityId(), 
                          dtSec, 
                          new Fix64Vec2(positionComponent.posX,  positionComponent.posY), 
                          new Fix64Vec2(ballVeloComp.veloX,  ballVeloComp.veloY), 
                          (Fix64) ballColliderComp.radius, 
                          (Fix64) ballColliderComp.mass, 
                          ballColliderComp.geoCollisionResponse, 
                          ballColliderComp.layer, 
                          rotationalVelo);


      _physInput.BallSweepData.Add(ballSweepInput);
    }
    
    _staticColliderContext.PhysUpdate(_physInput, _physOutput);
    
    for(int i = 0; i < queryResults.Count; i++)
    {
      IEntityData e = queryResults[i];
      PositionComponent positionComponent = e.Get<PositionComponent>();
      VelocityComponent ballVeloComp = e.Get<VelocityComponent>();

      CollisionContext.BallSweepState eResult = _physOutput.BallSweepData[i];
      positionComponent.posX = (int)eResult.curPos.x;
      positionComponent.posY = (int) eResult.curPos.y;
      
      ballVeloComp.veloX = (int)eResult.outputVelo.x;
      ballVeloComp.veloY = (int) eResult.outputVelo.y;

      if (e.GetArchetype().Contains<RotationVelocityComponent>())
      {
        e.Get<RotationVelocityComponent>().rotVelo = eResult.outputRotationalVelo;
      }
    }

    foreach (var collisionEventComponent in _physOutput.Collisions)
    {
      EntityId collisionEventId = _dataSource.CreateEntity(AliasLookup.CollisionEvent);
      CollisionEventComponent component = _dataSource.GetEntityComponent<CollisionEventComponent>(collisionEventId);
      _world.GetEntityRepo().CopyTo(collisionEventComponent, component);
    }
  }
}