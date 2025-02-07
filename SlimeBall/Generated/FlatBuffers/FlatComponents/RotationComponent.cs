// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct RotationComponent : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static RotationComponent GetRootAsRotationComponent(ByteBuffer _bb) { return GetRootAsRotationComponent(_bb, new RotationComponent()); }
  public static RotationComponent GetRootAsRotationComponent(ByteBuffer _bb, RotationComponent obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public RotationComponent __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long Rot { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }

  public static Offset<FlatComponents.RotationComponent> CreateRotationComponent(FlatBufferBuilder builder,
      long rot = 0) {
    builder.StartTable(1);
    RotationComponent.AddRot(builder, rot);
    return RotationComponent.EndRotationComponent(builder);
  }

  public static void StartRotationComponent(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddRot(FlatBufferBuilder builder, long rot) { builder.AddLong(0, rot, 0); }
  public static Offset<FlatComponents.RotationComponent> EndRotationComponent(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatComponents.RotationComponent>(o);
  }
};


}
