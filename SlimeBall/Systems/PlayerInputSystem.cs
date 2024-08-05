using System;
using System.Linq;
using ecs;
using FixMath.NET;

namespace Indigo.Slimeball;

public class PlayerInputSystem : ISystem
{
  private World _world;
  private Query _playerInputQuery;
  private Query _matchingPlayerPawnQuery;

  private EntityRepo _dataSource;

  public PlayerInputSystem(World w)
  {
    _world = w;
    _dataSource = w.GetEntityRepo();
    _playerInputQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype inputArch = w.GetArchetypes().GetAliasArchetype(AliasLookup.PlayerInput);
    _playerInputQuery.SetContainsArchetypeFilter(inputArch);

    _matchingPlayerPawnQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype pawnArch = w.GetArchetypes().GetAliasArchetype(AliasLookup.Slime);
    _matchingPlayerPawnQuery.SetContainsArchetypeFilter(pawnArch);
  }
  
  public void Execute()
  {
    var inputs = _playerInputQuery.Resolve(_dataSource);
    foreach (IEntityData input in inputs)
    {
      var pid = input.Get<PlayerOwnedComponent>().playerId;
      var playerOwnedIdx = _world.GetComponentLookup().GetIndex<PlayerOwnedComponent>();
      _matchingPlayerPawnQuery.SetMatchesComponentFieldKeyFilter(playerOwnedIdx, pid);

      IEntityData? matchingPlayerPawn = _matchingPlayerPawnQuery.Resolve(_dataSource).FirstOrDefault();
      if (matchingPlayerPawn == null)
      {
        continue;
      }
      
      PlayerMovementComponent playerMovementComponent = matchingPlayerPawn.Get<PlayerMovementComponent>();
      
      var playerInputCom = input.Get<PlayerInputComponent>();
      playerMovementComponent.movementVeloToApply = 1100 * MathF.Sign(playerInputCom.moveInputX);

      if (playerInputCom.jumpPressed)
      {
        var gravityComponent = matchingPlayerPawn.Get<GravityComponent>();
        if (gravityComponent.grounded)
        {
          playerMovementComponent.lastJumpFrame = _world.GetNextFrameNum() - 1;
        }
      }
    }
  }
}