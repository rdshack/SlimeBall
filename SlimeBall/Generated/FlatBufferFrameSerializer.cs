//Generated code


using System;
using System.Collections.Generic;
using ecs;
using FlatBuffers;
using FlatComponents;
using SimMath;
using FixMath.NET;
using System.Text;

public class FlatBufferFrameSerializer : IFrameSerializer
{
  private FlatBufferBuilder         _fbb = new FlatBufferBuilder(100);
  private List<Offset<ComponentSet>>  _compSets    = new List<Offset<ComponentSet>>();
  private byte[]                      _tempBytes   = new byte[100];
  private List<ulong>                 _entityIds   = new List<ulong>();
  private List<Offset<NewEntityData>> _newEntities = new List<Offset<NewEntityData>>();

  private Offset<FlatComponents.PositionComponent> _tempPositionComponentOffset;
  private Offset<FlatComponents.RotationComponent> _tempRotationComponentOffset;
  private Offset<FlatComponents.VelocityComponent> _tempVelocityComponentOffset;
  private Offset<FlatComponents.RotationVelocityComponent> _tempRotationVelocityComponentOffset;
  private Offset<FlatComponents.TimeComponent> _tempTimeComponentOffset;
  private Offset<FlatComponents.BallMovementComponent> _tempBallMovementComponentOffset;
  private Offset<FlatComponents.GravityComponent> _tempGravityComponentOffset;
  private Offset<FlatComponents.CircleColliderComponent> _tempCircleColliderComponentOffset;
  private Offset<FlatComponents.PlayerMovementComponent> _tempPlayerMovementComponentOffset;
  private Offset<FlatComponents.PlayerOwnedComponent> _tempPlayerOwnedComponentOffset;
  private Offset<FlatComponents.RequestDestroyComponent> _tempRequestDestroyComponentOffset;
  private Offset<FlatComponents.GameComponent> _tempGameComponentOffset;
  private Offset<FlatComponents.CollisionEventComponent> _tempCollisionEventComponentOffset;
  private Offset<FlatComponents.PawnSpawnEventComponent> _tempPawnSpawnEventComponentOffset;
  private Offset<FlatComponents.PlayerInputComponent> _tempPlayerInputComponentOffset;
  private Offset<FlatComponents.CreateNewPlayerInputComponent> _tempCreateNewPlayerInputComponentOffset;

  private Action<FlatBufferBuilder, int> _startComponentStateVector = FlatComponents.InputData.StartComponentStateVector;
  private Action<FlatBufferBuilder, int> _startEntityIdsVector = FlatComponents.FrameData.StartEntityIdsVector;
  private Action<FlatBufferBuilder, int> _startNewEntitiesVector =   FlatComponents.FrameData.StartNewEntitiesVector;

  private IWorldLogger _logger;
  private StringBuilder _stringBuilder;

  public FlatBufferFrameSerializer(IWorldLogger logger = null)
  {
    _logger = logger;
    if (logger != null)
    {
      _stringBuilder = new StringBuilder();
    }
  }

  public int Serialize(ArchetypeGraph archetypeGraph, IByteArrayResizer resizer, IFrameSyncData data, ref byte[] resultBuffer)
  {
    _fbb.Clear();
    _compSets.Clear();

    foreach (var ed in data.GetClientInputData())
    {
      var componentTypeIndices = archetypeGraph.GetComponentIndicesForArchetype(ed.GetArchetype());
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        IComponent c = ed.GetComponent(componentTypeIndex);
        BuildComponent(componentTypeIndex, c);
      }

      FlatComponents.ComponentSet.StartComponentSet(_fbb);
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        AddComponentToSet(componentTypeIndex);
      }
      _compSets.Add(ComponentSet.EndComponentSet(_fbb));
    }

    VectorOffset compSetsOffset = FlatBufferUtil.AddVectorToBufferFromOffsetList(_fbb, FlatComponents.FrameSyncData.StartInputStateVector, _compSets);
    var frameSyncData = FlatComponents.FrameSyncData.CreateFrameSyncData(_fbb, data.GetFrameNum(), data.GetFullStateHash(), compSetsOffset);
    _fbb.Finish(frameSyncData.Value);

    int length = _fbb.DataBuffer.Length - _fbb.DataBuffer.Position;
    if (length > resultBuffer.Length)
    {
      resizer.Resize(ref resultBuffer, length);
    }

    var source = _fbb.DataBuffer.ToArraySegment(_fbb.DataBuffer.Position, length);
    Array.Copy(source.Array, source.Offset, resultBuffer, 0, length);

    return length;
}

  public int Serialize(ArchetypeGraph archetypeGraph, IByteArrayResizer resizer, IFrameInputData data, ref byte[] resultBuffer)
  {
    _fbb.Clear();
    _compSets.Clear();

    foreach (var ed in data.GetComponentGroups())
    {
      var componentTypeIndices = archetypeGraph.GetComponentIndicesForArchetype(ed.GetArchetype());
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        IComponent c = ed.GetComponent(componentTypeIndex);
        BuildComponent(componentTypeIndex, c);
      }

      FlatComponents.ComponentSet.StartComponentSet(_fbb);
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        AddComponentToSet(componentTypeIndex);
      }
      _compSets.Add(ComponentSet.EndComponentSet(_fbb));
    }

    VectorOffset compSetsOffset = FlatBufferUtil.AddVectorToBufferFromOffsetList(_fbb, _startComponentStateVector, _compSets);
    var frameInputData = FlatComponents.InputData.CreateInputData(_fbb, data.GetFrameNum(), compSetsOffset);

    _fbb.Finish(frameInputData.Value);

    int length = _fbb.DataBuffer.Length - _fbb.DataBuffer.Position;
    if (length > resultBuffer.Length)
    {
      resizer.Resize(ref resultBuffer, length);
    }

    var source = _fbb.DataBuffer.ToArraySegment(_fbb.DataBuffer.Position, length);
    Array.Copy(source.Array, source.Offset, resultBuffer, 0, length);

    return length;
}


  public void DeserializeFrame(byte[] data, IDeserializedFrameDataStore output, int dataStart)
  {
    FlatComponents.FrameData frameData = FlatComponents.FrameData.GetRootAsFrameData(new ByteBuffer(data, dataStart));
    output.FrameNum = frameData.FrameNum;
    output.NextEntityId = new EntityId(frameData.NextEntityId);

    for (int i = 0; i < frameData.NewEntitiesLength; i++)
    {
      var newEntData = frameData.NewEntities(i).Value;
      output.SetNewEntityHash(new EntityId(newEntData.EntityId), newEntData.StateHash);
    }

    for (int i = 0; i < frameData.EntityIdsLength; i++)
    {
      var id = frameData.EntityIds(i);
      output.AddEntity(new EntityId(id), output.IsNewEntity(new EntityId(id)));
    }

    for (int i = 0; i < frameData.ComponentStateLength; i++)
    {
      var compSet = frameData.ComponentState(i).Value;
      if (compSet.PositionComponent.HasValue)
      {
        var flatC = compSet.PositionComponent.Value;
        var c = (PositionComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PositionComponent>());
        c.posX = flatC.PosX;
        c.posY = flatC.PosY;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.RotationComponent.HasValue)
      {
        var flatC = compSet.RotationComponent.Value;
        var c = (RotationComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationComponent>());
        c.rot = Fix64.FromRaw(flatC.Rot);
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.VelocityComponent.HasValue)
      {
        var flatC = compSet.VelocityComponent.Value;
        var c = (VelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<VelocityComponent>());
        c.veloX = flatC.VeloX;
        c.veloY = flatC.VeloY;
        c.maxSpeed = flatC.MaxSpeed;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.RotationVelocityComponent.HasValue)
      {
        var flatC = compSet.RotationVelocityComponent.Value;
        var c = (RotationVelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationVelocityComponent>());
        c.rotVelo = Fix64.FromRaw(flatC.RotVelo);
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.TimeComponent.HasValue)
      {
        var flatC = compSet.TimeComponent.Value;
        var c = (TimeComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<TimeComponent>());
        c.deltaTimeMs = flatC.DeltaTimeMs;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.BallMovementComponent.HasValue)
      {
        var flatC = compSet.BallMovementComponent.Value;
        var c = (BallMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<BallMovementComponent>());
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.bounceDampening = flatC.BounceDampening;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.GravityComponent.HasValue)
      {
        var flatC = compSet.GravityComponent.Value;
        var c = (GravityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GravityComponent>());
        c.gravity = flatC.Gravity;
        c.veloToApply = flatC.VeloToApply;
        c.grounded = flatC.Grounded;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.CircleColliderComponent.HasValue)
      {
        var flatC = compSet.CircleColliderComponent.Value;
        var c = (CircleColliderComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CircleColliderComponent>());
        c.drag = flatC.Drag;
        c.angularDrag = flatC.AngularDrag;
        c.radius = flatC.Radius;
        c.mass = flatC.Mass;
        c.geoCollisionResponse = (StaticGeoCollisionResponse)flatC.GeoCollisionResponse;
        c.layer = (CollisionLayer)flatC.Layer;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.PlayerMovementComponent.HasValue)
      {
        var flatC = compSet.PlayerMovementComponent.Value;
        var c = (PlayerMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerMovementComponent>());
        c.maxSpeedX = flatC.MaxSpeedX;
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.movementVeloToApply = flatC.MovementVeloToApply;
        c.lastJumpFrame = flatC.LastJumpFrame;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.PlayerOwnedComponent.HasValue)
      {
        var flatC = compSet.PlayerOwnedComponent.Value;
        var c = (PlayerOwnedComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerOwnedComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.RequestDestroyComponent.HasValue)
      {
        var flatC = compSet.RequestDestroyComponent.Value;
        var c = (RequestDestroyComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RequestDestroyComponent>());
        c.destroy = flatC.Destroy;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.GameComponent.HasValue)
      {
        var flatC = compSet.GameComponent.Value;
        var c = (GameComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GameComponent>());
        c.gamePhase = (GamePhase)flatC.GamePhase;
        c.leftPlayerScore = flatC.LeftPlayerScore;
        c.rightPlayerScore = flatC.RightPlayerScore;
        c.leftPlayerPawn = new EntityId(flatC.LeftPlayerPawn);
        c.rightPlayerPawn = new EntityId(flatC.RightPlayerPawn);
        c.ball = new EntityId(flatC.Ball);
        c.mostRecentPointWasLeft = flatC.MostRecentPointWasLeft;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.CollisionEventComponent.HasValue)
      {
        var flatC = compSet.CollisionEventComponent.Value;
        var c = (CollisionEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CollisionEventComponent>());
        c.collisionObjAEntity = new EntityId(flatC.CollisionObjAEntity);
        c.collisionObjBEntity = new EntityId(flatC.CollisionObjBEntity);
        c.collisionObjAPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjAPos.Value.X, flatC.CollisionObjAPos.Value.Y);
        c.collisionObjBPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjBPos.Value.X, flatC.CollisionObjBPos.Value.Y);
        c.collisionType = (CollisionType)flatC.CollisionType;
        c.staticCollisionType = (StaticColliderType)flatC.StaticCollisionType;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.PawnSpawnEventComponent.HasValue)
      {
        var flatC = compSet.PawnSpawnEventComponent.Value;
        var c = (PawnSpawnEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PawnSpawnEventComponent>());
        c.newPawn = new EntityId(flatC.NewPawn);
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.PlayerInputComponent.HasValue)
      {
        var flatC = compSet.PlayerInputComponent.Value;
        var c = (PlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerInputComponent>());
        c.moveInputX = flatC.MoveInputX;
        c.moveInputY = flatC.MoveInputY;
        c.jumpPressed = flatC.JumpPressed;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
      if (compSet.CreateNewPlayerInputComponent.HasValue)
      {
        var flatC = compSet.CreateNewPlayerInputComponent.Value;
        var c = (CreateNewPlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CreateNewPlayerInputComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId(frameData.EntityIds(i)), c);
      }
    }
  }

  public void DeserializeSyncFrame(byte[] data, IDeserializedFrameSyncStore output, int dataStart)
  {
    FlatComponents.FrameSyncData frameData = FlatComponents.FrameSyncData.GetRootAsFrameSyncData(new ByteBuffer(data, dataStart));
    output.FrameNum = frameData.FrameNum;
    output.FullStateHash = frameData.FullStateHash;
    for (int i = 0; i < frameData.InputStateLength; i++)
    {
      var compSet = frameData.InputState(i).Value;
      if (compSet.PositionComponent.HasValue)
      {
        var flatC = compSet.PositionComponent.Value;
        var c = (PositionComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PositionComponent>());
        c.posX = flatC.PosX;
        c.posY = flatC.PosY;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RotationComponent.HasValue)
      {
        var flatC = compSet.RotationComponent.Value;
        var c = (RotationComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationComponent>());
        c.rot = Fix64.FromRaw(flatC.Rot);
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.VelocityComponent.HasValue)
      {
        var flatC = compSet.VelocityComponent.Value;
        var c = (VelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<VelocityComponent>());
        c.veloX = flatC.VeloX;
        c.veloY = flatC.VeloY;
        c.maxSpeed = flatC.MaxSpeed;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RotationVelocityComponent.HasValue)
      {
        var flatC = compSet.RotationVelocityComponent.Value;
        var c = (RotationVelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationVelocityComponent>());
        c.rotVelo = Fix64.FromRaw(flatC.RotVelo);
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.TimeComponent.HasValue)
      {
        var flatC = compSet.TimeComponent.Value;
        var c = (TimeComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<TimeComponent>());
        c.deltaTimeMs = flatC.DeltaTimeMs;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.BallMovementComponent.HasValue)
      {
        var flatC = compSet.BallMovementComponent.Value;
        var c = (BallMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<BallMovementComponent>());
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.bounceDampening = flatC.BounceDampening;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.GravityComponent.HasValue)
      {
        var flatC = compSet.GravityComponent.Value;
        var c = (GravityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GravityComponent>());
        c.gravity = flatC.Gravity;
        c.veloToApply = flatC.VeloToApply;
        c.grounded = flatC.Grounded;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CircleColliderComponent.HasValue)
      {
        var flatC = compSet.CircleColliderComponent.Value;
        var c = (CircleColliderComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CircleColliderComponent>());
        c.drag = flatC.Drag;
        c.angularDrag = flatC.AngularDrag;
        c.radius = flatC.Radius;
        c.mass = flatC.Mass;
        c.geoCollisionResponse = (StaticGeoCollisionResponse)flatC.GeoCollisionResponse;
        c.layer = (CollisionLayer)flatC.Layer;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerMovementComponent.HasValue)
      {
        var flatC = compSet.PlayerMovementComponent.Value;
        var c = (PlayerMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerMovementComponent>());
        c.maxSpeedX = flatC.MaxSpeedX;
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.movementVeloToApply = flatC.MovementVeloToApply;
        c.lastJumpFrame = flatC.LastJumpFrame;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerOwnedComponent.HasValue)
      {
        var flatC = compSet.PlayerOwnedComponent.Value;
        var c = (PlayerOwnedComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerOwnedComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RequestDestroyComponent.HasValue)
      {
        var flatC = compSet.RequestDestroyComponent.Value;
        var c = (RequestDestroyComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RequestDestroyComponent>());
        c.destroy = flatC.Destroy;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.GameComponent.HasValue)
      {
        var flatC = compSet.GameComponent.Value;
        var c = (GameComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GameComponent>());
        c.gamePhase = (GamePhase)flatC.GamePhase;
        c.leftPlayerScore = flatC.LeftPlayerScore;
        c.rightPlayerScore = flatC.RightPlayerScore;
        c.leftPlayerPawn = new EntityId(flatC.LeftPlayerPawn);
        c.rightPlayerPawn = new EntityId(flatC.RightPlayerPawn);
        c.ball = new EntityId(flatC.Ball);
        c.mostRecentPointWasLeft = flatC.MostRecentPointWasLeft;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CollisionEventComponent.HasValue)
      {
        var flatC = compSet.CollisionEventComponent.Value;
        var c = (CollisionEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CollisionEventComponent>());
        c.collisionObjAEntity = new EntityId(flatC.CollisionObjAEntity);
        c.collisionObjBEntity = new EntityId(flatC.CollisionObjBEntity);
        c.collisionObjAPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjAPos.Value.X, flatC.CollisionObjAPos.Value.Y);
        c.collisionObjBPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjBPos.Value.X, flatC.CollisionObjBPos.Value.Y);
        c.collisionType = (CollisionType)flatC.CollisionType;
        c.staticCollisionType = (StaticColliderType)flatC.StaticCollisionType;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PawnSpawnEventComponent.HasValue)
      {
        var flatC = compSet.PawnSpawnEventComponent.Value;
        var c = (PawnSpawnEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PawnSpawnEventComponent>());
        c.newPawn = new EntityId(flatC.NewPawn);
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerInputComponent.HasValue)
      {
        var flatC = compSet.PlayerInputComponent.Value;
        var c = (PlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerInputComponent>());
        c.moveInputX = flatC.MoveInputX;
        c.moveInputY = flatC.MoveInputY;
        c.jumpPressed = flatC.JumpPressed;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CreateNewPlayerInputComponent.HasValue)
      {
        var flatC = compSet.CreateNewPlayerInputComponent.Value;
        var c = (CreateNewPlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CreateNewPlayerInputComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
    }
  }


  public void DeserializeInputFrame(byte[] data, IDeserializedFrameSyncStore output, int dataStart)
  {
    FlatComponents.InputData frameData = FlatComponents.InputData.GetRootAsInputData(new ByteBuffer(data, dataStart));
    output.FrameNum = frameData.FrameNum;
    for (int i = 0; i < frameData.ComponentStateLength; i++)
    {
      var compSet = frameData.ComponentState(i).Value;
      if (compSet.PositionComponent.HasValue)
      {
        var flatC = compSet.PositionComponent.Value;
        var c = (PositionComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PositionComponent>());
        c.posX = flatC.PosX;
        c.posY = flatC.PosY;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RotationComponent.HasValue)
      {
        var flatC = compSet.RotationComponent.Value;
        var c = (RotationComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationComponent>());
        c.rot = Fix64.FromRaw(flatC.Rot);
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.VelocityComponent.HasValue)
      {
        var flatC = compSet.VelocityComponent.Value;
        var c = (VelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<VelocityComponent>());
        c.veloX = flatC.VeloX;
        c.veloY = flatC.VeloY;
        c.maxSpeed = flatC.MaxSpeed;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RotationVelocityComponent.HasValue)
      {
        var flatC = compSet.RotationVelocityComponent.Value;
        var c = (RotationVelocityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RotationVelocityComponent>());
        c.rotVelo = Fix64.FromRaw(flatC.RotVelo);
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.TimeComponent.HasValue)
      {
        var flatC = compSet.TimeComponent.Value;
        var c = (TimeComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<TimeComponent>());
        c.deltaTimeMs = flatC.DeltaTimeMs;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.BallMovementComponent.HasValue)
      {
        var flatC = compSet.BallMovementComponent.Value;
        var c = (BallMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<BallMovementComponent>());
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.bounceDampening = flatC.BounceDampening;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.GravityComponent.HasValue)
      {
        var flatC = compSet.GravityComponent.Value;
        var c = (GravityComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GravityComponent>());
        c.gravity = flatC.Gravity;
        c.veloToApply = flatC.VeloToApply;
        c.grounded = flatC.Grounded;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CircleColliderComponent.HasValue)
      {
        var flatC = compSet.CircleColliderComponent.Value;
        var c = (CircleColliderComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CircleColliderComponent>());
        c.drag = flatC.Drag;
        c.angularDrag = flatC.AngularDrag;
        c.radius = flatC.Radius;
        c.mass = flatC.Mass;
        c.geoCollisionResponse = (StaticGeoCollisionResponse)flatC.GeoCollisionResponse;
        c.layer = (CollisionLayer)flatC.Layer;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerMovementComponent.HasValue)
      {
        var flatC = compSet.PlayerMovementComponent.Value;
        var c = (PlayerMovementComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerMovementComponent>());
        c.maxSpeedX = flatC.MaxSpeedX;
        c.maxFallSpeed = flatC.MaxFallSpeed;
        c.movementVeloToApply = flatC.MovementVeloToApply;
        c.lastJumpFrame = flatC.LastJumpFrame;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerOwnedComponent.HasValue)
      {
        var flatC = compSet.PlayerOwnedComponent.Value;
        var c = (PlayerOwnedComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerOwnedComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.RequestDestroyComponent.HasValue)
      {
        var flatC = compSet.RequestDestroyComponent.Value;
        var c = (RequestDestroyComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<RequestDestroyComponent>());
        c.destroy = flatC.Destroy;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.GameComponent.HasValue)
      {
        var flatC = compSet.GameComponent.Value;
        var c = (GameComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<GameComponent>());
        c.gamePhase = (GamePhase)flatC.GamePhase;
        c.leftPlayerScore = flatC.LeftPlayerScore;
        c.rightPlayerScore = flatC.RightPlayerScore;
        c.leftPlayerPawn = new EntityId(flatC.LeftPlayerPawn);
        c.rightPlayerPawn = new EntityId(flatC.RightPlayerPawn);
        c.ball = new EntityId(flatC.Ball);
        c.mostRecentPointWasLeft = flatC.MostRecentPointWasLeft;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CollisionEventComponent.HasValue)
      {
        var flatC = compSet.CollisionEventComponent.Value;
        var c = (CollisionEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CollisionEventComponent>());
        c.collisionObjAEntity = new EntityId(flatC.CollisionObjAEntity);
        c.collisionObjBEntity = new EntityId(flatC.CollisionObjBEntity);
        c.collisionObjAPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjAPos.Value.X, flatC.CollisionObjAPos.Value.Y);
        c.collisionObjBPos = SimMath.Fix64Vec2.FromRaw(flatC.CollisionObjBPos.Value.X, flatC.CollisionObjBPos.Value.Y);
        c.collisionType = (CollisionType)flatC.CollisionType;
        c.staticCollisionType = (StaticColliderType)flatC.StaticCollisionType;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PawnSpawnEventComponent.HasValue)
      {
        var flatC = compSet.PawnSpawnEventComponent.Value;
        var c = (PawnSpawnEventComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PawnSpawnEventComponent>());
        c.newPawn = new EntityId(flatC.NewPawn);
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.PlayerInputComponent.HasValue)
      {
        var flatC = compSet.PlayerInputComponent.Value;
        var c = (PlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<PlayerInputComponent>());
        c.moveInputX = flatC.MoveInputX;
        c.moveInputY = flatC.MoveInputY;
        c.jumpPressed = flatC.JumpPressed;
        output.AddComponent(new EntityId((ulong)i), c);
      }
      if (compSet.CreateNewPlayerInputComponent.HasValue)
      {
        var flatC = compSet.CreateNewPlayerInputComponent.Value;
        var c = (CreateNewPlayerInputComponent) output.ComponentPool.Get(output.ComponentDefinitions.GetIndex<CreateNewPlayerInputComponent>());
        c.playerId = flatC.PlayerId;
        output.AddComponent(new EntityId((ulong)i), c);
      }
    }
  }

  public int Serialize(ArchetypeGraph archetypeGraph, IByteArrayResizer resizer, IFrameData data, ref byte[] resultBuffer)
  {
    _stringBuilder?.Clear();
    _stringBuilder?.AppendLine($"Serializing frame '{data.GetFrameNum()}' with next id '{data.GetEntityRepo().GetNextEntityId().Id}'");
    _fbb.Clear();
    _compSets.Clear();
    _newEntities.Clear();
    _entityIds.Clear();


    foreach (var ed in data.GetEntityRepo().GetEntitiesData())
    {
      _stringBuilder?.AppendLine($"Entity '{ed.GetEntityId().Id}'");
      _entityIds.Add(ed.GetEntityId().Id);
      var componentTypeIndices = archetypeGraph.GetComponentIndicesForArchetype(ed.GetArchetype());
      bool createdThisFrame = data.IsNewEntity(ed.GetEntityId());
      int startPos = _fbb.DataBuffer.Position;
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        IComponent c = data.GetEntityRepo().GetEntityComponent(ed.GetEntityId(), componentTypeIndex);
        BuildComponent(componentTypeIndex, c);
      }

      FlatComponents.ComponentSet.StartComponentSet(_fbb);
      foreach (var componentTypeIndex in componentTypeIndices)
      {
        AddComponentToSet(componentTypeIndex);
      }
      _compSets.Add(ComponentSet.EndComponentSet(_fbb));

      if (createdThisFrame)
      {
         var seg = _fbb.DataBuffer.ToArraySegment(startPos, _fbb.DataBuffer.Position - startPos);
        int compHash = CreateStateHash(seg.Array, startPos, _fbb.DataBuffer.Position - startPos);
      }
    }

    VectorOffset ids = FlatBufferUtil.AddVectorToBufferFromUlongList(_fbb, _startEntityIdsVector, _entityIds);
    VectorOffset newEntityData = FlatBufferUtil.AddVectorToBufferFromOffsetList(_fbb, _startNewEntitiesVector, _newEntities);
    VectorOffset compSetsOffset = FlatBufferUtil.AddVectorToBufferFromOffsetList(_fbb, _startComponentStateVector, _compSets);
    var frameSyncData = FlatComponents.FrameData.CreateFrameData(_fbb, data.GetEntityRepo().GetNextEntityId().Id, data.GetFrameNum(), newEntityData, ids, compSetsOffset );
    _fbb.Finish(frameSyncData.Value);

    int length = _fbb.DataBuffer.Length - _fbb.DataBuffer.Position;
    if (length > resultBuffer.Length)
    {
      resizer.Resize(ref resultBuffer, length);
    }

    var source = _fbb.DataBuffer.ToArraySegment(_fbb.DataBuffer.Position, length);
    Array.Copy(source.Array, source.Offset, resultBuffer, 0, length);

    _logger?.Log(_stringBuilder.ToString());
    return length;
}

  private void BuildComponent(ComponentTypeIndex componentTypeIndex, IComponent component)
  {
    switch (componentTypeIndex.Index)
    {
      case 0: BuildPositionComponent((PositionComponent)component); break;
      case 1: BuildRotationComponent((RotationComponent)component); break;
      case 2: BuildVelocityComponent((VelocityComponent)component); break;
      case 3: BuildRotationVelocityComponent((RotationVelocityComponent)component); break;
      case 4: BuildTimeComponent((TimeComponent)component); break;
      case 5: BuildBallMovementComponent((BallMovementComponent)component); break;
      case 6: BuildGravityComponent((GravityComponent)component); break;
      case 7: BuildCircleColliderComponent((CircleColliderComponent)component); break;
      case 8: BuildPlayerMovementComponent((PlayerMovementComponent)component); break;
      case 9: BuildPlayerOwnedComponent((PlayerOwnedComponent)component); break;
      case 10: BuildRequestDestroyComponent((RequestDestroyComponent)component); break;
      case 11: BuildGameComponent((GameComponent)component); break;
      case 12: BuildCollisionEventComponent((CollisionEventComponent)component); break;
      case 13: BuildPawnSpawnEventComponent((PawnSpawnEventComponent)component); break;
      case 14: BuildPlayerInputComponent((PlayerInputComponent)component); break;
      case 15: BuildCreateNewPlayerInputComponent((CreateNewPlayerInputComponent)component); break;
    }
  }
  private void BuildPositionComponent(PositionComponent c)
  {
    _stringBuilder?.AppendLine($"PositionComponent:");
    FlatComponents.PositionComponent.StartPositionComponent(_fbb);
    _stringBuilder?.AppendLine($"posX: {c.posX}");
    FlatComponents.PositionComponent.AddPosX(_fbb, c.posX);
    _stringBuilder?.AppendLine($"posY: {c.posY}");
    FlatComponents.PositionComponent.AddPosY(_fbb, c.posY);
_tempPositionComponentOffset = FlatComponents.PositionComponent.EndPositionComponent(_fbb);
  }
  private void BuildRotationComponent(RotationComponent c)
  {
    _stringBuilder?.AppendLine($"RotationComponent:");
    FlatComponents.RotationComponent.StartRotationComponent(_fbb);
    _stringBuilder?.AppendLine($"rot: {c.rot.RawValue}");
    FlatComponents.RotationComponent.AddRot(_fbb, c.rot.RawValue);
_tempRotationComponentOffset = FlatComponents.RotationComponent.EndRotationComponent(_fbb);
  }
  private void BuildVelocityComponent(VelocityComponent c)
  {
    _stringBuilder?.AppendLine($"VelocityComponent:");
    FlatComponents.VelocityComponent.StartVelocityComponent(_fbb);
    _stringBuilder?.AppendLine($"veloX: {c.veloX}");
    FlatComponents.VelocityComponent.AddVeloX(_fbb, c.veloX);
    _stringBuilder?.AppendLine($"veloY: {c.veloY}");
    FlatComponents.VelocityComponent.AddVeloY(_fbb, c.veloY);
    _stringBuilder?.AppendLine($"maxSpeed: {c.maxSpeed}");
    FlatComponents.VelocityComponent.AddMaxSpeed(_fbb, c.maxSpeed);
_tempVelocityComponentOffset = FlatComponents.VelocityComponent.EndVelocityComponent(_fbb);
  }
  private void BuildRotationVelocityComponent(RotationVelocityComponent c)
  {
    _stringBuilder?.AppendLine($"RotationVelocityComponent:");
    FlatComponents.RotationVelocityComponent.StartRotationVelocityComponent(_fbb);
    _stringBuilder?.AppendLine($"rotVelo: {c.rotVelo.RawValue}");
    FlatComponents.RotationVelocityComponent.AddRotVelo(_fbb, c.rotVelo.RawValue);
_tempRotationVelocityComponentOffset = FlatComponents.RotationVelocityComponent.EndRotationVelocityComponent(_fbb);
  }
  private void BuildTimeComponent(TimeComponent c)
  {
    _stringBuilder?.AppendLine($"TimeComponent:");
    FlatComponents.TimeComponent.StartTimeComponent(_fbb);
    _stringBuilder?.AppendLine($"deltaTimeMs: {c.deltaTimeMs}");
    FlatComponents.TimeComponent.AddDeltaTimeMs(_fbb, c.deltaTimeMs);
_tempTimeComponentOffset = FlatComponents.TimeComponent.EndTimeComponent(_fbb);
  }
  private void BuildBallMovementComponent(BallMovementComponent c)
  {
    _stringBuilder?.AppendLine($"BallMovementComponent:");
    FlatComponents.BallMovementComponent.StartBallMovementComponent(_fbb);
    _stringBuilder?.AppendLine($"maxFallSpeed: {c.maxFallSpeed}");
    FlatComponents.BallMovementComponent.AddMaxFallSpeed(_fbb, c.maxFallSpeed);
    _stringBuilder?.AppendLine($"bounceDampening: {c.bounceDampening}");
    FlatComponents.BallMovementComponent.AddBounceDampening(_fbb, c.bounceDampening);
_tempBallMovementComponentOffset = FlatComponents.BallMovementComponent.EndBallMovementComponent(_fbb);
  }
  private void BuildGravityComponent(GravityComponent c)
  {
    _stringBuilder?.AppendLine($"GravityComponent:");
    FlatComponents.GravityComponent.StartGravityComponent(_fbb);
    _stringBuilder?.AppendLine($"gravity: {c.gravity}");
    FlatComponents.GravityComponent.AddGravity(_fbb, c.gravity);
    _stringBuilder?.AppendLine($"veloToApply: {c.veloToApply}");
    FlatComponents.GravityComponent.AddVeloToApply(_fbb, c.veloToApply);
    _stringBuilder?.AppendLine($"grounded: {c.grounded}");
    FlatComponents.GravityComponent.AddGrounded(_fbb, c.grounded);
_tempGravityComponentOffset = FlatComponents.GravityComponent.EndGravityComponent(_fbb);
  }
  private void BuildCircleColliderComponent(CircleColliderComponent c)
  {
    _stringBuilder?.AppendLine($"CircleColliderComponent:");
    FlatComponents.CircleColliderComponent.StartCircleColliderComponent(_fbb);
    _stringBuilder?.AppendLine($"drag: {c.drag}");
    FlatComponents.CircleColliderComponent.AddDrag(_fbb, c.drag);
    _stringBuilder?.AppendLine($"angularDrag: {c.angularDrag}");
    FlatComponents.CircleColliderComponent.AddAngularDrag(_fbb, c.angularDrag);
    _stringBuilder?.AppendLine($"radius: {c.radius}");
    FlatComponents.CircleColliderComponent.AddRadius(_fbb, c.radius);
    _stringBuilder?.AppendLine($"mass: {c.mass}");
    FlatComponents.CircleColliderComponent.AddMass(_fbb, c.mass);
    _stringBuilder?.AppendLine($"geoCollisionResponse: (sbyte)c.geoCollisionResponse");
    FlatComponents.CircleColliderComponent.AddGeoCollisionResponse(_fbb, (sbyte)c.geoCollisionResponse);
    _stringBuilder?.AppendLine($"layer: (sbyte)c.layer");
    FlatComponents.CircleColliderComponent.AddLayer(_fbb, (sbyte)c.layer);
_tempCircleColliderComponentOffset = FlatComponents.CircleColliderComponent.EndCircleColliderComponent(_fbb);
  }
  private void BuildPlayerMovementComponent(PlayerMovementComponent c)
  {
    _stringBuilder?.AppendLine($"PlayerMovementComponent:");
    FlatComponents.PlayerMovementComponent.StartPlayerMovementComponent(_fbb);
    _stringBuilder?.AppendLine($"maxSpeedX: {c.maxSpeedX}");
    FlatComponents.PlayerMovementComponent.AddMaxSpeedX(_fbb, c.maxSpeedX);
    _stringBuilder?.AppendLine($"maxFallSpeed: {c.maxFallSpeed}");
    FlatComponents.PlayerMovementComponent.AddMaxFallSpeed(_fbb, c.maxFallSpeed);
    _stringBuilder?.AppendLine($"movementVeloToApply: {c.movementVeloToApply}");
    FlatComponents.PlayerMovementComponent.AddMovementVeloToApply(_fbb, c.movementVeloToApply);
    _stringBuilder?.AppendLine($"lastJumpFrame: {c.lastJumpFrame}");
    FlatComponents.PlayerMovementComponent.AddLastJumpFrame(_fbb, c.lastJumpFrame);
_tempPlayerMovementComponentOffset = FlatComponents.PlayerMovementComponent.EndPlayerMovementComponent(_fbb);
  }
  private void BuildPlayerOwnedComponent(PlayerOwnedComponent c)
  {
    _stringBuilder?.AppendLine($"PlayerOwnedComponent:");
    FlatComponents.PlayerOwnedComponent.StartPlayerOwnedComponent(_fbb);
    _stringBuilder?.AppendLine($"playerId: {c.playerId}");
    FlatComponents.PlayerOwnedComponent.AddPlayerId(_fbb, c.playerId);
_tempPlayerOwnedComponentOffset = FlatComponents.PlayerOwnedComponent.EndPlayerOwnedComponent(_fbb);
  }
  private void BuildRequestDestroyComponent(RequestDestroyComponent c)
  {
    _stringBuilder?.AppendLine($"RequestDestroyComponent:");
    FlatComponents.RequestDestroyComponent.StartRequestDestroyComponent(_fbb);
    _stringBuilder?.AppendLine($"destroy: {c.destroy}");
    FlatComponents.RequestDestroyComponent.AddDestroy(_fbb, c.destroy);
_tempRequestDestroyComponentOffset = FlatComponents.RequestDestroyComponent.EndRequestDestroyComponent(_fbb);
  }
  private void BuildGameComponent(GameComponent c)
  {
    _stringBuilder?.AppendLine($"GameComponent:");
    FlatComponents.GameComponent.StartGameComponent(_fbb);
    _stringBuilder?.AppendLine($"gamePhase: (sbyte)c.gamePhase");
    FlatComponents.GameComponent.AddGamePhase(_fbb, (sbyte)c.gamePhase);
    _stringBuilder?.AppendLine($"leftPlayerScore: {c.leftPlayerScore}");
    FlatComponents.GameComponent.AddLeftPlayerScore(_fbb, c.leftPlayerScore);
    _stringBuilder?.AppendLine($"rightPlayerScore: {c.rightPlayerScore}");
    FlatComponents.GameComponent.AddRightPlayerScore(_fbb, c.rightPlayerScore);
    _stringBuilder?.AppendLine($"leftPlayerPawn: c.leftPlayerPawn.Id");
    FlatComponents.GameComponent.AddLeftPlayerPawn(_fbb, c.leftPlayerPawn.Id);
    _stringBuilder?.AppendLine($"rightPlayerPawn: c.rightPlayerPawn.Id");
    FlatComponents.GameComponent.AddRightPlayerPawn(_fbb, c.rightPlayerPawn.Id);
    _stringBuilder?.AppendLine($"ball: c.ball.Id");
    FlatComponents.GameComponent.AddBall(_fbb, c.ball.Id);
    _stringBuilder?.AppendLine($"mostRecentPointWasLeft: {c.mostRecentPointWasLeft}");
    FlatComponents.GameComponent.AddMostRecentPointWasLeft(_fbb, c.mostRecentPointWasLeft);
_tempGameComponentOffset = FlatComponents.GameComponent.EndGameComponent(_fbb);
  }
  private void BuildCollisionEventComponent(CollisionEventComponent c)
  {
    _stringBuilder?.AppendLine($"CollisionEventComponent:");
    FlatComponents.CollisionEventComponent.StartCollisionEventComponent(_fbb);
    _stringBuilder?.AppendLine($"collisionObjAEntity: c.collisionObjAEntity.Id");
    FlatComponents.CollisionEventComponent.AddCollisionObjAEntity(_fbb, c.collisionObjAEntity.Id);
    _stringBuilder?.AppendLine($"collisionObjBEntity: c.collisionObjBEntity.Id");
    FlatComponents.CollisionEventComponent.AddCollisionObjBEntity(_fbb, c.collisionObjBEntity.Id);
    _stringBuilder?.AppendLine($"collisionObjAPos: x:{c.collisionObjAPos.x.RawValue}, y:{c.collisionObjAPos.y.RawValue}");
    FlatComponents.CollisionEventComponent.AddCollisionObjAPos(_fbb, FlatComponents.Fix64Vec2.CreateFix64Vec2(_fbb, c.collisionObjAPos.x.RawValue, c.collisionObjAPos.y.RawValue));
    _stringBuilder?.AppendLine($"collisionObjBPos: x:{c.collisionObjBPos.x.RawValue}, y:{c.collisionObjBPos.y.RawValue}");
    FlatComponents.CollisionEventComponent.AddCollisionObjBPos(_fbb, FlatComponents.Fix64Vec2.CreateFix64Vec2(_fbb, c.collisionObjBPos.x.RawValue, c.collisionObjBPos.y.RawValue));
    _stringBuilder?.AppendLine($"collisionType: (sbyte)c.collisionType");
    FlatComponents.CollisionEventComponent.AddCollisionType(_fbb, (sbyte)c.collisionType);
    _stringBuilder?.AppendLine($"staticCollisionType: (sbyte)c.staticCollisionType");
    FlatComponents.CollisionEventComponent.AddStaticCollisionType(_fbb, (sbyte)c.staticCollisionType);
_tempCollisionEventComponentOffset = FlatComponents.CollisionEventComponent.EndCollisionEventComponent(_fbb);
  }
  private void BuildPawnSpawnEventComponent(PawnSpawnEventComponent c)
  {
    _stringBuilder?.AppendLine($"PawnSpawnEventComponent:");
    FlatComponents.PawnSpawnEventComponent.StartPawnSpawnEventComponent(_fbb);
    _stringBuilder?.AppendLine($"newPawn: c.newPawn.Id");
    FlatComponents.PawnSpawnEventComponent.AddNewPawn(_fbb, c.newPawn.Id);
    _stringBuilder?.AppendLine($"playerId: {c.playerId}");
    FlatComponents.PawnSpawnEventComponent.AddPlayerId(_fbb, c.playerId);
_tempPawnSpawnEventComponentOffset = FlatComponents.PawnSpawnEventComponent.EndPawnSpawnEventComponent(_fbb);
  }
  private void BuildPlayerInputComponent(PlayerInputComponent c)
  {
    _stringBuilder?.AppendLine($"PlayerInputComponent:");
    FlatComponents.PlayerInputComponent.StartPlayerInputComponent(_fbb);
    _stringBuilder?.AppendLine($"moveInputX: {c.moveInputX}");
    FlatComponents.PlayerInputComponent.AddMoveInputX(_fbb, c.moveInputX);
    _stringBuilder?.AppendLine($"moveInputY: {c.moveInputY}");
    FlatComponents.PlayerInputComponent.AddMoveInputY(_fbb, c.moveInputY);
    _stringBuilder?.AppendLine($"jumpPressed: {c.jumpPressed}");
    FlatComponents.PlayerInputComponent.AddJumpPressed(_fbb, c.jumpPressed);
_tempPlayerInputComponentOffset = FlatComponents.PlayerInputComponent.EndPlayerInputComponent(_fbb);
  }
  private void BuildCreateNewPlayerInputComponent(CreateNewPlayerInputComponent c)
  {
    _stringBuilder?.AppendLine($"CreateNewPlayerInputComponent:");
    FlatComponents.CreateNewPlayerInputComponent.StartCreateNewPlayerInputComponent(_fbb);
    _stringBuilder?.AppendLine($"playerId: {c.playerId}");
    FlatComponents.CreateNewPlayerInputComponent.AddPlayerId(_fbb, c.playerId);
_tempCreateNewPlayerInputComponentOffset = FlatComponents.CreateNewPlayerInputComponent.EndCreateNewPlayerInputComponent(_fbb);
  }

  public int CreateStateHash(byte[] seg, int pos, int len)
  {
    unchecked
    {
      const int p = 16777619;
      int hash = (int)2166136261;
      for (int i = pos; i < len; i++)
        hash = (hash ^ seg[i]) * p;;
      return hash;
    }
  }

  private void AddComponentToSet(ComponentTypeIndex componentTypeIndex)
  {
    switch (componentTypeIndex.Index)
    {
      case 0: FlatComponents.ComponentSet.AddPositionComponent(_fbb, _tempPositionComponentOffset); break;
      case 1: FlatComponents.ComponentSet.AddRotationComponent(_fbb, _tempRotationComponentOffset); break;
      case 2: FlatComponents.ComponentSet.AddVelocityComponent(_fbb, _tempVelocityComponentOffset); break;
      case 3: FlatComponents.ComponentSet.AddRotationVelocityComponent(_fbb, _tempRotationVelocityComponentOffset); break;
      case 4: FlatComponents.ComponentSet.AddTimeComponent(_fbb, _tempTimeComponentOffset); break;
      case 5: FlatComponents.ComponentSet.AddBallMovementComponent(_fbb, _tempBallMovementComponentOffset); break;
      case 6: FlatComponents.ComponentSet.AddGravityComponent(_fbb, _tempGravityComponentOffset); break;
      case 7: FlatComponents.ComponentSet.AddCircleColliderComponent(_fbb, _tempCircleColliderComponentOffset); break;
      case 8: FlatComponents.ComponentSet.AddPlayerMovementComponent(_fbb, _tempPlayerMovementComponentOffset); break;
      case 9: FlatComponents.ComponentSet.AddPlayerOwnedComponent(_fbb, _tempPlayerOwnedComponentOffset); break;
      case 10: FlatComponents.ComponentSet.AddRequestDestroyComponent(_fbb, _tempRequestDestroyComponentOffset); break;
      case 11: FlatComponents.ComponentSet.AddGameComponent(_fbb, _tempGameComponentOffset); break;
      case 12: FlatComponents.ComponentSet.AddCollisionEventComponent(_fbb, _tempCollisionEventComponentOffset); break;
      case 13: FlatComponents.ComponentSet.AddPawnSpawnEventComponent(_fbb, _tempPawnSpawnEventComponentOffset); break;
      case 14: FlatComponents.ComponentSet.AddPlayerInputComponent(_fbb, _tempPlayerInputComponentOffset); break;
      case 15: FlatComponents.ComponentSet.AddCreateNewPlayerInputComponent(_fbb, _tempCreateNewPlayerInputComponentOffset); break;
    }
  }
}
