// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct BallMovementComponent : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static BallMovementComponent GetRootAsBallMovementComponent(ByteBuffer _bb) { return GetRootAsBallMovementComponent(_bb, new BallMovementComponent()); }
  public static BallMovementComponent GetRootAsBallMovementComponent(ByteBuffer _bb, BallMovementComponent obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public BallMovementComponent __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int MaxFallSpeed { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int BounceDampening { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<FlatComponents.BallMovementComponent> CreateBallMovementComponent(FlatBufferBuilder builder,
      int maxFallSpeed = 0,
      int bounceDampening = 0) {
    builder.StartTable(2);
    BallMovementComponent.AddBounceDampening(builder, bounceDampening);
    BallMovementComponent.AddMaxFallSpeed(builder, maxFallSpeed);
    return BallMovementComponent.EndBallMovementComponent(builder);
  }

  public static void StartBallMovementComponent(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddMaxFallSpeed(FlatBufferBuilder builder, int maxFallSpeed) { builder.AddInt(0, maxFallSpeed, 0); }
  public static void AddBounceDampening(FlatBufferBuilder builder, int bounceDampening) { builder.AddInt(1, bounceDampening, 0); }
  public static Offset<FlatComponents.BallMovementComponent> EndBallMovementComponent(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatComponents.BallMovementComponent>(o);
  }
};


}
