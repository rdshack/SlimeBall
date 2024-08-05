using ecs;
using FixMath.NET;
using SimMath;

namespace Indigo.Slimeball;

public class GameManagerSystem : ISystem
{
  private Query _addPlayerInputQuery;
  private Query _collisionEventQuery;
  
  private EntityRepo _repo;
  private World      _world;

  public GameManagerSystem(World w)
  {
    _world = w;
    _repo = w.GetEntityRepo();
    _addPlayerInputQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().With<CreateNewPlayerInputComponent>();
    _addPlayerInputQuery.SetContainsArchetypeFilter(a);
    
    _collisionEventQuery = new Query(w.GetArchetypes(), w.GetComponentLookup());
    _collisionEventQuery.SetContainsAliasFilter(AliasLookup.CollisionEvent);
  }
  
  public void Execute()
  {
    GameComponent gameComponent = _repo.GetSingletonComponent<GameComponent>();
    
    switch (gameComponent.gamePhase)
    {
      case GamePhase.WaitingForAllPlayers:
        ProcessWaitingForPlayersState();
        break;
      case GamePhase.WaitingForServe:
        ProcessWaitForServeState();
        break;
      case GamePhase.PointInPlay:
        ProcessPointInPlay();
        break;
    }
  }

  private void ProcessPointInPlay()
  {
    GameComponent gameComponent = _repo.GetSingletonComponent<GameComponent>();
    foreach (IEntityData collisionEvent in _collisionEventQuery.Resolve(_repo))
    {
      CollisionEventComponent c = collisionEvent.Get<CollisionEventComponent>();
      
      //1 is ball-floor collision
      if (c.collisionType == CollisionType.BallStatic && 
          c.staticCollisionType == StaticColliderType.Floor)
      {
        gameComponent.gamePhase = GamePhase.WaitingForServe;

        PositionComponent positionComponent = _repo.GetEntityComponent<PositionComponent>(gameComponent.ball);
        if (positionComponent.posX > 0)
        {
          gameComponent.leftPlayerScore++;
          gameComponent.mostRecentPointWasLeft = true;
        }
        else
        {
          gameComponent.rightPlayerScore++;
          gameComponent.mostRecentPointWasLeft = false;
        }
        
        _repo.DestroyEntity(gameComponent.ball);
        _repo.DestroyEntity(gameComponent.leftPlayerPawn);
        _repo.DestroyEntity(gameComponent.rightPlayerPawn);
        gameComponent.ball = AddBall(gameComponent.mostRecentPointWasLeft);
        gameComponent.leftPlayerPawn = AddPlayerPawn(0);
        gameComponent.rightPlayerPawn = AddPlayerPawn(1);

        break;
      }
    }
  }

  private void ProcessWaitingForPlayersState()
  {
    GameComponent gameComponent = _repo.GetSingletonComponent<GameComponent>();

    foreach (IEntityData item in _addPlayerInputQuery.Resolve(_repo))
    {
      if (!gameComponent.leftPlayerPawn.IsValid())
      {
        CreateNewPlayerInputComponent addPlayerInputComponent = item.Get<CreateNewPlayerInputComponent>();
        gameComponent.leftPlayerPawn = AddPlayerPawn(addPlayerInputComponent.playerId);
      }
      else if(!gameComponent.rightPlayerPawn.IsValid())
      {
        CreateNewPlayerInputComponent addPlayerInputComponent = item.Get<CreateNewPlayerInputComponent>();
        gameComponent.rightPlayerPawn = AddPlayerPawn(addPlayerInputComponent.playerId);
        gameComponent.gamePhase = GamePhase.WaitingForServe;

        gameComponent.ball = AddBall(true);
      }
    }
  }

  private void ProcessWaitForServeState()
  {
    GameComponent gameComponent = _repo.GetSingletonComponent<GameComponent>();
    foreach (IEntityData collisionEvent in _collisionEventQuery.Resolve(_repo))
    {
      CollisionEventComponent c = collisionEvent.Get<CollisionEventComponent>();
      
      //1 is ball-ball collision
      if (c.collisionType == CollisionType.BallBall)
      {
        gameComponent.gamePhase = GamePhase.PointInPlay;
        _repo.GetEntityComponent<GravityComponent>(gameComponent.ball).gravity = -11_000;
      }
    }
  }

  private EntityId AddPlayerPawn(int playerId)
  {
    EntityId playerPawnEntity = _repo.CreateEntity(AliasLookup.Slime);
    _repo.GetEntityComponent<PlayerOwnedComponent>(playerPawnEntity).playerId = playerId;

    int xPos = playerId == 0 ? -3000 : 3000;
    _repo.GetEntityComponent<PositionComponent>(playerPawnEntity).posX = xPos;
    _repo.GetEntityComponent<PositionComponent>(playerPawnEntity).posY = 3000;
    _repo.GetEntityComponent<GravityComponent>(playerPawnEntity).gravity = -18_000;
    _repo.GetEntityComponent<CircleColliderComponent>(playerPawnEntity).radius = 600;
    _repo.GetEntityComponent<CircleColliderComponent>(playerPawnEntity).mass = 3;
    _repo.GetEntityComponent<PlayerMovementComponent>(playerPawnEntity).maxSpeedX = 5000;
    _repo.GetEntityComponent<CircleColliderComponent>(playerPawnEntity).geoCollisionResponse = StaticGeoCollisionResponse.Slide;
    _repo.GetEntityComponent<CircleColliderComponent>(playerPawnEntity).layer = CollisionLayer.Pawn;

    EntityId playerSpawnEventEntity = _repo.CreateEntity(_world.GetArchetypes().With<PawnSpawnEventComponent>());
    PawnSpawnEventComponent spawnEventComponent = _repo.GetEntityComponent<PawnSpawnEventComponent>(playerSpawnEventEntity);
    spawnEventComponent.newPawn = playerPawnEntity;
    spawnEventComponent.playerId = playerId;
    
    return playerPawnEntity;
  }
  
  private EntityId AddBall(bool spawnLeft)
  {
    EntityId ball = _repo.CreateEntity(AliasLookup.Ball);
    _repo.GetEntityComponent<PositionComponent>(ball).posX = spawnLeft ? -2500 : 2500;
    _repo.GetEntityComponent<PositionComponent>(ball).posY = 3200;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).radius = 400;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).mass = 1;
    _repo.GetEntityComponent<VelocityComponent>(ball).veloX = 0;
    _repo.GetEntityComponent<VelocityComponent>(ball).veloY = 0;
    _repo.GetEntityComponent<VelocityComponent>(ball).maxSpeed = 11500;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).geoCollisionResponse = StaticGeoCollisionResponse.Bounce;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).layer = CollisionLayer.Ball;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).drag = 1;
    _repo.GetEntityComponent<CircleColliderComponent>(ball).angularDrag = 1;
    _repo.GetEntityComponent<RotationVelocityComponent>(ball).rotVelo = new Fix64(0);
    return ball;
  }
}