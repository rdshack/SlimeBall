// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct GravityComponent : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static GravityComponent GetRootAsGravityComponent(ByteBuffer _bb) { return GetRootAsGravityComponent(_bb, new GravityComponent()); }
  public static GravityComponent GetRootAsGravityComponent(ByteBuffer _bb, GravityComponent obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public GravityComponent __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Gravity { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int VeloToApply { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public bool Grounded { get { int o = __p.__offset(8); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }

  public static Offset<FlatComponents.GravityComponent> CreateGravityComponent(FlatBufferBuilder builder,
      int gravity = 0,
      int veloToApply = 0,
      bool grounded = false) {
    builder.StartTable(3);
    GravityComponent.AddVeloToApply(builder, veloToApply);
    GravityComponent.AddGravity(builder, gravity);
    GravityComponent.AddGrounded(builder, grounded);
    return GravityComponent.EndGravityComponent(builder);
  }

  public static void StartGravityComponent(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddGravity(FlatBufferBuilder builder, int gravity) { builder.AddInt(0, gravity, 0); }
  public static void AddVeloToApply(FlatBufferBuilder builder, int veloToApply) { builder.AddInt(1, veloToApply, 0); }
  public static void AddGrounded(FlatBufferBuilder builder, bool grounded) { builder.AddBool(2, grounded, false); }
  public static Offset<FlatComponents.GravityComponent> EndGravityComponent(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatComponents.GravityComponent>(o);
  }
};


}