using System.Numerics;
using SimMath;
using ecs;
using FixMath.NET;

namespace Indigo.Slimeball;

public class PlayerMoveSystem : ISystem
{
  private Query                 _pawnQuery;
  private Query                 _inputQuery;
  private World                 _world;
  
  private EntityRepo _dataSource;

  public PlayerMoveSystem(World w)
  {
    _world = w;
    _dataSource = w.GetEntityRepo();
    _pawnQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().GetAliasArchetype(AliasLookup.Slime);
    _pawnQuery.SetContainsArchetypeFilter(a);

    _inputQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype inputArch = w.GetArchetypes()
                           .With<PlayerInputComponent>()
                           .With<PlayerOwnedComponent>();
    _inputQuery.SetContainsArchetypeFilter(inputArch);
  }
  
  public void Execute()
  {
    GameComponent gameComponent = _dataSource.GetSingletonComponent<GameComponent>();
    if (gameComponent.gamePhase == GamePhase.WaitingForAllPlayers)
    {
      return;
    }
    
    var inputQueryResults = _inputQuery.Resolve(_dataSource);
    var pawnQueryResults = _pawnQuery.Resolve(_dataSource);
    
    foreach (IEntityData input in inputQueryResults)
    {
      foreach (IEntityData pawn in pawnQueryResults)
      {
        if (pawn.Get<PlayerOwnedComponent>().playerId != input.Get<PlayerOwnedComponent>().playerId)
        {
          continue;
        }
        GravityComponent gravityComponent = pawn.Get<GravityComponent>();
        PlayerMovementComponent playerMovementComponent = pawn.Get<PlayerMovementComponent>();

        VelocityComponent velocityComponent = pawn.Get<VelocityComponent>();
        Fix64Vec2 newVelo = new Fix64Vec2(velocityComponent.veloX, velocityComponent.veloY);
        
        if (gravityComponent.grounded)
        {
          int curFrame = _world.GetNextFrameNum() - 1;
          int framesSinceJumpInput = curFrame - playerMovementComponent.lastJumpFrame;

          if (framesSinceJumpInput == 0)
          {
            newVelo.y = new Fix64(9_000);
          }
          else if(framesSinceJumpInput > 5)
          {
            newVelo.y = Fix64.Zero; 
          }
        }
        
        newVelo += new Fix64Vec2(0, gravityComponent.veloToApply);
        newVelo += new Fix64Vec2(playerMovementComponent.movementVeloToApply, 0);
        
        newVelo.x = MathUtil.Clamp(newVelo.x, 
                                   -(Fix64)playerMovementComponent.maxSpeedX,
                                   (Fix64)playerMovementComponent.maxSpeedX);

        //If grounded, and we are not trying to move, apply x-velocity decay
        if(gravityComponent.grounded && playerMovementComponent.movementVeloToApply == 0)
        {
          Fix64 cur = new Fix64(velocityComponent.veloX);
          Fix64 decayedVelo = MathUtil.Lerp(cur, Fix64.Zero, (Fix64)0.8f);
          newVelo.x = decayedVelo;

          if (newVelo.x < (Fix64)5)
          {
            newVelo.x = Fix64.Zero;
          }                 
        }

        velocityComponent.veloX = (int)newVelo.x;
        velocityComponent.veloY = (int) newVelo.y;
      }
    }
  }
}