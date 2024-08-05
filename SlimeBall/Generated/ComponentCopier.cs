//Generated code


using System;
using System.Collections.Generic;
using ecs;
using System.Text;

public class ComponentCopier : IComponentFactory
{
  private const int DEFAULT_POOL_SIZE = 5;

  private ComponentDefinitions _componentDefinitions;
  private Dictionary<Type, Action<IComponent, IComponent>> copyMethods;

  private Dictionary<Type, Action<IComponent>> resetMethods;

  private List<ObjPool<IComponent>>                           _poolList = new List<ObjPool<IComponent>>();
  private Dictionary<ComponentTypeIndex, ObjPool<IComponent>> _pools = new Dictionary<ComponentTypeIndex, ObjPool<IComponent>>();
  private ObjPool<EntityData>     _entityDataPool = new ObjPool<EntityData>(EntityData.Create, EntityData.Reset);
  private ObjPool<ComponentGroup> _compGroupPool  = new ObjPool<ComponentGroup>(ComponentGroup.Create, ComponentGroup.Reset);

  public ComponentCopier(ComponentDefinitions componentDefinitions)
  {
    _componentDefinitions = componentDefinitions;

    copyMethods = new Dictionary<Type, Action<IComponent, IComponent>>();

    resetMethods = new Dictionary<Type, Action<IComponent>>();

    copyMethods[typeof(PositionComponent)] = CopyPositionComponent;
    copyMethods[typeof(RotationComponent)] = CopyRotationComponent;
    copyMethods[typeof(VelocityComponent)] = CopyVelocityComponent;
    copyMethods[typeof(RotationVelocityComponent)] = CopyRotationVelocityComponent;
    copyMethods[typeof(TimeComponent)] = CopyTimeComponent;
    copyMethods[typeof(BallMovementComponent)] = CopyBallMovementComponent;
    copyMethods[typeof(GravityComponent)] = CopyGravityComponent;
    copyMethods[typeof(CircleColliderComponent)] = CopyCircleColliderComponent;
    copyMethods[typeof(PlayerMovementComponent)] = CopyPlayerMovementComponent;
    copyMethods[typeof(PlayerOwnedComponent)] = CopyPlayerOwnedComponent;
    copyMethods[typeof(RequestDestroyComponent)] = CopyRequestDestroyComponent;
    copyMethods[typeof(GameComponent)] = CopyGameComponent;
    copyMethods[typeof(CollisionEventComponent)] = CopyCollisionEventComponent;
    copyMethods[typeof(PawnSpawnEventComponent)] = CopyPawnSpawnEventComponent;
    copyMethods[typeof(PlayerInputComponent)] = CopyPlayerInputComponent;
    copyMethods[typeof(CreateNewPlayerInputComponent)] = CopyCreateNewPlayerInputComponent;

    resetMethods[typeof(PositionComponent)] = ResetPositionComponent;
    resetMethods[typeof(RotationComponent)] = ResetRotationComponent;
    resetMethods[typeof(VelocityComponent)] = ResetVelocityComponent;
    resetMethods[typeof(RotationVelocityComponent)] = ResetRotationVelocityComponent;
    resetMethods[typeof(TimeComponent)] = ResetTimeComponent;
    resetMethods[typeof(BallMovementComponent)] = ResetBallMovementComponent;
    resetMethods[typeof(GravityComponent)] = ResetGravityComponent;
    resetMethods[typeof(CircleColliderComponent)] = ResetCircleColliderComponent;
    resetMethods[typeof(PlayerMovementComponent)] = ResetPlayerMovementComponent;
    resetMethods[typeof(PlayerOwnedComponent)] = ResetPlayerOwnedComponent;
    resetMethods[typeof(RequestDestroyComponent)] = ResetRequestDestroyComponent;
    resetMethods[typeof(GameComponent)] = ResetGameComponent;
    resetMethods[typeof(CollisionEventComponent)] = ResetCollisionEventComponent;
    resetMethods[typeof(PawnSpawnEventComponent)] = ResetPawnSpawnEventComponent;
    resetMethods[typeof(PlayerInputComponent)] = ResetPlayerInputComponent;
    resetMethods[typeof(CreateNewPlayerInputComponent)] = ResetCreateNewPlayerInputComponent;

    _pools[_componentDefinitions.GetIndex<PositionComponent>()] = new ObjPool<IComponent>(BuildPositionComponent, ResetPositionComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<PositionComponent>()]);
    _pools[_componentDefinitions.GetIndex<RotationComponent>()] = new ObjPool<IComponent>(BuildRotationComponent, ResetRotationComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<RotationComponent>()]);
    _pools[_componentDefinitions.GetIndex<VelocityComponent>()] = new ObjPool<IComponent>(BuildVelocityComponent, ResetVelocityComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<VelocityComponent>()]);
    _pools[_componentDefinitions.GetIndex<RotationVelocityComponent>()] = new ObjPool<IComponent>(BuildRotationVelocityComponent, ResetRotationVelocityComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<RotationVelocityComponent>()]);
    _pools[_componentDefinitions.GetIndex<TimeComponent>()] = new ObjPool<IComponent>(BuildTimeComponent, ResetTimeComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<TimeComponent>()]);
    _pools[_componentDefinitions.GetIndex<BallMovementComponent>()] = new ObjPool<IComponent>(BuildBallMovementComponent, ResetBallMovementComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<BallMovementComponent>()]);
    _pools[_componentDefinitions.GetIndex<GravityComponent>()] = new ObjPool<IComponent>(BuildGravityComponent, ResetGravityComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<GravityComponent>()]);
    _pools[_componentDefinitions.GetIndex<CircleColliderComponent>()] = new ObjPool<IComponent>(BuildCircleColliderComponent, ResetCircleColliderComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<CircleColliderComponent>()]);
    _pools[_componentDefinitions.GetIndex<PlayerMovementComponent>()] = new ObjPool<IComponent>(BuildPlayerMovementComponent, ResetPlayerMovementComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<PlayerMovementComponent>()]);
    _pools[_componentDefinitions.GetIndex<PlayerOwnedComponent>()] = new ObjPool<IComponent>(BuildPlayerOwnedComponent, ResetPlayerOwnedComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<PlayerOwnedComponent>()]);
    _pools[_componentDefinitions.GetIndex<RequestDestroyComponent>()] = new ObjPool<IComponent>(BuildRequestDestroyComponent, ResetRequestDestroyComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<RequestDestroyComponent>()]);
    _pools[_componentDefinitions.GetIndex<GameComponent>()] = new ObjPool<IComponent>(BuildGameComponent, ResetGameComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<GameComponent>()]);
    _pools[_componentDefinitions.GetIndex<CollisionEventComponent>()] = new ObjPool<IComponent>(BuildCollisionEventComponent, ResetCollisionEventComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<CollisionEventComponent>()]);
    _pools[_componentDefinitions.GetIndex<PawnSpawnEventComponent>()] = new ObjPool<IComponent>(BuildPawnSpawnEventComponent, ResetPawnSpawnEventComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<PawnSpawnEventComponent>()]);
    _pools[_componentDefinitions.GetIndex<PlayerInputComponent>()] = new ObjPool<IComponent>(BuildPlayerInputComponent, ResetPlayerInputComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<PlayerInputComponent>()]);
    _pools[_componentDefinitions.GetIndex<CreateNewPlayerInputComponent>()] = new ObjPool<IComponent>(BuildCreateNewPlayerInputComponent, ResetCreateNewPlayerInputComponent);
    _poolList.Add(_pools[_componentDefinitions.GetIndex<CreateNewPlayerInputComponent>()]);
  }


  public void ReturnAll()
  {
    _compGroupPool.ReturnAll();
    _entityDataPool.ReturnAll();

    foreach (var pool in _poolList)
    {
      pool.ReturnAll();
    }
  }

  public EntityData GetEntityData()
  {
    return _entityDataPool.Get();
  }

  public void ReturnEntityData(IComponentGroup e)
  {
    if (e is EntityData entityData)
    {
      _entityDataPool.Return(entityData);
    }
    else if (e is ComponentGroup componentGroup)
    {
      _compGroupPool.Return(componentGroup);
    }
    else
    {
      throw new Exception();
    }
  }

  public ComponentGroup GetComponentGroup(ArchetypeGraph graph, IComponentDefinitions definitions)
  {
    ComponentGroup group = _compGroupPool.Get();
    group.Setup(this, graph, definitions);
    return group;
  }

  public IComponent Get(ComponentTypeIndex idx)
  {
    return _pools[idx].Get();
  }

  public void Return(IComponent component)
  {
    _pools[_componentDefinitions.GetIndex(component)].Return(component);
  }

  public void Copy(IComponent source, IComponent target)
  {
    Type sourceType = source.GetType();
    copyMethods[sourceType](source, target);
  }

  public void Reset(IComponent c)
  {
    Type sourceType = c.GetType();
    resetMethods[sourceType](c);
  }

  private void CopyPositionComponent(IComponent s, IComponent t)
  {
    PositionComponent source = (PositionComponent) s;
    PositionComponent target = (PositionComponent) t;
    target.posX = source.posX;
    target.posY = source.posY;
  }
  private void ResetPositionComponent(IComponent c)
  {
    PositionComponent source = (PositionComponent) c;
    source.posX = default;
    source.posY = default;
  }
  private Component BuildPositionComponent()
  {
    return new PositionComponent();
  }
  private void CopyRotationComponent(IComponent s, IComponent t)
  {
    RotationComponent source = (RotationComponent) s;
    RotationComponent target = (RotationComponent) t;
    target.rot = source.rot;
  }
  private void ResetRotationComponent(IComponent c)
  {
    RotationComponent source = (RotationComponent) c;
    source.rot = default;
  }
  private Component BuildRotationComponent()
  {
    return new RotationComponent();
  }
  private void CopyVelocityComponent(IComponent s, IComponent t)
  {
    VelocityComponent source = (VelocityComponent) s;
    VelocityComponent target = (VelocityComponent) t;
    target.veloX = source.veloX;
    target.veloY = source.veloY;
    target.maxSpeed = source.maxSpeed;
  }
  private void ResetVelocityComponent(IComponent c)
  {
    VelocityComponent source = (VelocityComponent) c;
    source.veloX = default;
    source.veloY = default;
    source.maxSpeed = default;
  }
  private Component BuildVelocityComponent()
  {
    return new VelocityComponent();
  }
  private void CopyRotationVelocityComponent(IComponent s, IComponent t)
  {
    RotationVelocityComponent source = (RotationVelocityComponent) s;
    RotationVelocityComponent target = (RotationVelocityComponent) t;
    target.rotVelo = source.rotVelo;
  }
  private void ResetRotationVelocityComponent(IComponent c)
  {
    RotationVelocityComponent source = (RotationVelocityComponent) c;
    source.rotVelo = default;
  }
  private Component BuildRotationVelocityComponent()
  {
    return new RotationVelocityComponent();
  }
  private void CopyTimeComponent(IComponent s, IComponent t)
  {
    TimeComponent source = (TimeComponent) s;
    TimeComponent target = (TimeComponent) t;
    target.deltaTimeMs = source.deltaTimeMs;
  }
  private void ResetTimeComponent(IComponent c)
  {
    TimeComponent source = (TimeComponent) c;
    source.deltaTimeMs = default;
  }
  private Component BuildTimeComponent()
  {
    return new TimeComponent();
  }
  private void CopyBallMovementComponent(IComponent s, IComponent t)
  {
    BallMovementComponent source = (BallMovementComponent) s;
    BallMovementComponent target = (BallMovementComponent) t;
    target.maxFallSpeed = source.maxFallSpeed;
    target.bounceDampening = source.bounceDampening;
  }
  private void ResetBallMovementComponent(IComponent c)
  {
    BallMovementComponent source = (BallMovementComponent) c;
    source.maxFallSpeed = default;
    source.bounceDampening = default;
  }
  private Component BuildBallMovementComponent()
  {
    return new BallMovementComponent();
  }
  private void CopyGravityComponent(IComponent s, IComponent t)
  {
    GravityComponent source = (GravityComponent) s;
    GravityComponent target = (GravityComponent) t;
    target.gravity = source.gravity;
    target.veloToApply = source.veloToApply;
    target.grounded = source.grounded;
  }
  private void ResetGravityComponent(IComponent c)
  {
    GravityComponent source = (GravityComponent) c;
    source.gravity = default;
    source.veloToApply = default;
    source.grounded = default;
  }
  private Component BuildGravityComponent()
  {
    return new GravityComponent();
  }
  private void CopyCircleColliderComponent(IComponent s, IComponent t)
  {
    CircleColliderComponent source = (CircleColliderComponent) s;
    CircleColliderComponent target = (CircleColliderComponent) t;
    target.drag = source.drag;
    target.angularDrag = source.angularDrag;
    target.radius = source.radius;
    target.mass = source.mass;
    target.geoCollisionResponse = source.geoCollisionResponse;
    target.layer = source.layer;
  }
  private void ResetCircleColliderComponent(IComponent c)
  {
    CircleColliderComponent source = (CircleColliderComponent) c;
    source.drag = default;
    source.angularDrag = default;
    source.radius = default;
    source.mass = default;
    source.geoCollisionResponse = default;
    source.layer = default;
  }
  private Component BuildCircleColliderComponent()
  {
    return new CircleColliderComponent();
  }
  private void CopyPlayerMovementComponent(IComponent s, IComponent t)
  {
    PlayerMovementComponent source = (PlayerMovementComponent) s;
    PlayerMovementComponent target = (PlayerMovementComponent) t;
    target.maxSpeedX = source.maxSpeedX;
    target.maxFallSpeed = source.maxFallSpeed;
    target.movementVeloToApply = source.movementVeloToApply;
    target.lastJumpFrame = source.lastJumpFrame;
  }
  private void ResetPlayerMovementComponent(IComponent c)
  {
    PlayerMovementComponent source = (PlayerMovementComponent) c;
    source.maxSpeedX = default;
    source.maxFallSpeed = default;
    source.movementVeloToApply = default;
    source.lastJumpFrame = default;
  }
  private Component BuildPlayerMovementComponent()
  {
    return new PlayerMovementComponent();
  }
  private void CopyPlayerOwnedComponent(IComponent s, IComponent t)
  {
    PlayerOwnedComponent source = (PlayerOwnedComponent) s;
    PlayerOwnedComponent target = (PlayerOwnedComponent) t;
    target.playerId = source.playerId;
  }
  private void ResetPlayerOwnedComponent(IComponent c)
  {
    PlayerOwnedComponent source = (PlayerOwnedComponent) c;
    source.playerId = default;
  }
  private Component BuildPlayerOwnedComponent()
  {
    return new PlayerOwnedComponent();
  }
  private void CopyRequestDestroyComponent(IComponent s, IComponent t)
  {
    RequestDestroyComponent source = (RequestDestroyComponent) s;
    RequestDestroyComponent target = (RequestDestroyComponent) t;
    target.destroy = source.destroy;
  }
  private void ResetRequestDestroyComponent(IComponent c)
  {
    RequestDestroyComponent source = (RequestDestroyComponent) c;
    source.destroy = default;
  }
  private Component BuildRequestDestroyComponent()
  {
    return new RequestDestroyComponent();
  }
  private void CopyGameComponent(IComponent s, IComponent t)
  {
    GameComponent source = (GameComponent) s;
    GameComponent target = (GameComponent) t;
    target.gamePhase = source.gamePhase;
    target.leftPlayerScore = source.leftPlayerScore;
    target.rightPlayerScore = source.rightPlayerScore;
    target.leftPlayerPawn = source.leftPlayerPawn;
    target.rightPlayerPawn = source.rightPlayerPawn;
    target.ball = source.ball;
    target.mostRecentPointWasLeft = source.mostRecentPointWasLeft;
  }
  private void ResetGameComponent(IComponent c)
  {
    GameComponent source = (GameComponent) c;
    source.gamePhase = default;
    source.leftPlayerScore = default;
    source.rightPlayerScore = default;
    source.leftPlayerPawn = default;
    source.rightPlayerPawn = default;
    source.ball = default;
    source.mostRecentPointWasLeft = default;
  }
  private Component BuildGameComponent()
  {
    return new GameComponent();
  }
  private void CopyCollisionEventComponent(IComponent s, IComponent t)
  {
    CollisionEventComponent source = (CollisionEventComponent) s;
    CollisionEventComponent target = (CollisionEventComponent) t;
    target.collisionObjAEntity = source.collisionObjAEntity;
    target.collisionObjBEntity = source.collisionObjBEntity;
    target.collisionObjAPos = source.collisionObjAPos;
    target.collisionObjBPos = source.collisionObjBPos;
    target.collisionType = source.collisionType;
    target.staticCollisionType = source.staticCollisionType;
  }
  private void ResetCollisionEventComponent(IComponent c)
  {
    CollisionEventComponent source = (CollisionEventComponent) c;
    source.collisionObjAEntity = default;
    source.collisionObjBEntity = default;
    source.collisionObjAPos = default;
    source.collisionObjBPos = default;
    source.collisionType = default;
    source.staticCollisionType = default;
  }
  private Component BuildCollisionEventComponent()
  {
    return new CollisionEventComponent();
  }
  private void CopyPawnSpawnEventComponent(IComponent s, IComponent t)
  {
    PawnSpawnEventComponent source = (PawnSpawnEventComponent) s;
    PawnSpawnEventComponent target = (PawnSpawnEventComponent) t;
    target.newPawn = source.newPawn;
    target.playerId = source.playerId;
  }
  private void ResetPawnSpawnEventComponent(IComponent c)
  {
    PawnSpawnEventComponent source = (PawnSpawnEventComponent) c;
    source.newPawn = default;
    source.playerId = default;
  }
  private Component BuildPawnSpawnEventComponent()
  {
    return new PawnSpawnEventComponent();
  }
  private void CopyPlayerInputComponent(IComponent s, IComponent t)
  {
    PlayerInputComponent source = (PlayerInputComponent) s;
    PlayerInputComponent target = (PlayerInputComponent) t;
    target.moveInputX = source.moveInputX;
    target.moveInputY = source.moveInputY;
    target.jumpPressed = source.jumpPressed;
  }
  private void ResetPlayerInputComponent(IComponent c)
  {
    PlayerInputComponent source = (PlayerInputComponent) c;
    source.moveInputX = default;
    source.moveInputY = default;
    source.jumpPressed = default;
  }
  private Component BuildPlayerInputComponent()
  {
    return new PlayerInputComponent();
  }
  private void CopyCreateNewPlayerInputComponent(IComponent s, IComponent t)
  {
    CreateNewPlayerInputComponent source = (CreateNewPlayerInputComponent) s;
    CreateNewPlayerInputComponent target = (CreateNewPlayerInputComponent) t;
    target.playerId = source.playerId;
  }
  private void ResetCreateNewPlayerInputComponent(IComponent c)
  {
    CreateNewPlayerInputComponent source = (CreateNewPlayerInputComponent) c;
    source.playerId = default;
  }
  private Component BuildCreateNewPlayerInputComponent()
  {
    return new CreateNewPlayerInputComponent();
  }

  public string ToString(IComponent c)
  {
       StringBuilder sb = new StringBuilder();
    sb.AppendLine(c.GetType().ToString());
    if (c is PositionComponent positionComponent)
    {
      sb.AppendLine($"posX: {positionComponent.posX}");
      sb.AppendLine($"posY: {positionComponent.posY}");
     return sb.ToString();
    }
    if (c is RotationComponent rotationComponent)
    {
      sb.AppendLine($"rot: {rotationComponent.rot}");
     return sb.ToString();
    }
    if (c is VelocityComponent velocityComponent)
    {
      sb.AppendLine($"veloX: {velocityComponent.veloX}");
      sb.AppendLine($"veloY: {velocityComponent.veloY}");
      sb.AppendLine($"maxSpeed: {velocityComponent.maxSpeed}");
     return sb.ToString();
    }
    if (c is RotationVelocityComponent rotationVelocityComponent)
    {
      sb.AppendLine($"rotVelo: {rotationVelocityComponent.rotVelo}");
     return sb.ToString();
    }
    if (c is TimeComponent timeComponent)
    {
      sb.AppendLine($"deltaTimeMs: {timeComponent.deltaTimeMs}");
     return sb.ToString();
    }
    if (c is BallMovementComponent ballMovementComponent)
    {
      sb.AppendLine($"maxFallSpeed: {ballMovementComponent.maxFallSpeed}");
      sb.AppendLine($"bounceDampening: {ballMovementComponent.bounceDampening}");
     return sb.ToString();
    }
    if (c is GravityComponent gravityComponent)
    {
      sb.AppendLine($"gravity: {gravityComponent.gravity}");
      sb.AppendLine($"veloToApply: {gravityComponent.veloToApply}");
      sb.AppendLine($"grounded: {gravityComponent.grounded}");
     return sb.ToString();
    }
    if (c is CircleColliderComponent circleColliderComponent)
    {
      sb.AppendLine($"drag: {circleColliderComponent.drag}");
      sb.AppendLine($"angularDrag: {circleColliderComponent.angularDrag}");
      sb.AppendLine($"radius: {circleColliderComponent.radius}");
      sb.AppendLine($"mass: {circleColliderComponent.mass}");
      sb.AppendLine($"geoCollisionResponse: {circleColliderComponent.geoCollisionResponse}");
      sb.AppendLine($"layer: {circleColliderComponent.layer}");
     return sb.ToString();
    }
    if (c is PlayerMovementComponent playerMovementComponent)
    {
      sb.AppendLine($"maxSpeedX: {playerMovementComponent.maxSpeedX}");
      sb.AppendLine($"maxFallSpeed: {playerMovementComponent.maxFallSpeed}");
      sb.AppendLine($"movementVeloToApply: {playerMovementComponent.movementVeloToApply}");
      sb.AppendLine($"lastJumpFrame: {playerMovementComponent.lastJumpFrame}");
     return sb.ToString();
    }
    if (c is PlayerOwnedComponent playerOwnedComponent)
    {
      sb.AppendLine($"playerId: {playerOwnedComponent.playerId}");
     return sb.ToString();
    }
    if (c is RequestDestroyComponent requestDestroyComponent)
    {
      sb.AppendLine($"destroy: {requestDestroyComponent.destroy}");
     return sb.ToString();
    }
    if (c is GameComponent gameComponent)
    {
      sb.AppendLine($"gamePhase: {gameComponent.gamePhase}");
      sb.AppendLine($"leftPlayerScore: {gameComponent.leftPlayerScore}");
      sb.AppendLine($"rightPlayerScore: {gameComponent.rightPlayerScore}");
      sb.AppendLine($"leftPlayerPawn: {gameComponent.leftPlayerPawn.Id}");
      sb.AppendLine($"rightPlayerPawn: {gameComponent.rightPlayerPawn.Id}");
      sb.AppendLine($"ball: {gameComponent.ball.Id}");
      sb.AppendLine($"mostRecentPointWasLeft: {gameComponent.mostRecentPointWasLeft}");
     return sb.ToString();
    }
    if (c is CollisionEventComponent collisionEventComponent)
    {
      sb.AppendLine($"collisionObjAEntity: {collisionEventComponent.collisionObjAEntity.Id}");
      sb.AppendLine($"collisionObjBEntity: {collisionEventComponent.collisionObjBEntity.Id}");
      sb.AppendLine($"collisionObjAPos: ({collisionEventComponent.collisionObjAPos.x}, {collisionEventComponent.collisionObjAPos.y})");
      sb.AppendLine($"collisionObjBPos: ({collisionEventComponent.collisionObjBPos.x}, {collisionEventComponent.collisionObjBPos.y})");
      sb.AppendLine($"collisionType: {collisionEventComponent.collisionType}");
      sb.AppendLine($"staticCollisionType: {collisionEventComponent.staticCollisionType}");
     return sb.ToString();
    }
    if (c is PawnSpawnEventComponent pawnSpawnEventComponent)
    {
      sb.AppendLine($"newPawn: {pawnSpawnEventComponent.newPawn.Id}");
      sb.AppendLine($"playerId: {pawnSpawnEventComponent.playerId}");
     return sb.ToString();
    }
    if (c is PlayerInputComponent playerInputComponent)
    {
      sb.AppendLine($"moveInputX: {playerInputComponent.moveInputX}");
      sb.AppendLine($"moveInputY: {playerInputComponent.moveInputY}");
      sb.AppendLine($"jumpPressed: {playerInputComponent.jumpPressed}");
     return sb.ToString();
    }
    if (c is CreateNewPlayerInputComponent createNewPlayerInputComponent)
    {
      sb.AppendLine($"playerId: {createNewPlayerInputComponent.playerId}");
     return sb.ToString();
    }
    throw new Exception();
  }
}
