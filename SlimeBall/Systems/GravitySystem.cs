
using ecs;
using FixMath.NET;
using Indigo.Collision2D;

namespace Indigo.Slimeball;

public class GravitySystem : ISystem
{
  private Query      _query;
  private EntityRepo _dataSource;

  public GravitySystem(World w)
  {
    _dataSource = w.GetEntityRepo();
    _query = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().
                    With<GravityComponent>().
                    With<PositionComponent>().
                    With<TimeComponent>().
                    With<CircleColliderComponent>();
    
    _query.SetContainsArchetypeFilter(a);
  }
  
  public void Execute()
  {
    GameComponent gameComponent = _dataSource.GetSingletonComponent<GameComponent>();
    if (gameComponent.gamePhase == GamePhase.WaitingForAllPlayers)
    {
      return;
    }

    foreach (IEntityData e in _query.Resolve(_dataSource))
    {
      GravityComponent gravityComponent = e.Get<GravityComponent>();
      CircleColliderComponent colliderComponent = e.Get<CircleColliderComponent>();
      
      gravityComponent.grounded = e.Get<PositionComponent>().posY < (int)CollisionContext.FLOOR_HEIGHT +
                                  (int)CollisionContext.C_GAP + (int)CollisionContext.C_GAP +
                                  colliderComponent.radius + 1;
      gravityComponent.veloToApply = 0;

      if (!gravityComponent.grounded)
      {
        TimeComponent timeComponent = e.Get<TimeComponent>();

        Fix64 dtSec = (Fix64) timeComponent.deltaTimeMs / (Fix64) 1000;
        Fix64 yVeloDelta = (Fix64) gravityComponent.gravity * dtSec;
        gravityComponent.veloToApply = (int)yVeloDelta; 
      }
    }
  }
}