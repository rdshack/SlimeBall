// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Fix64Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Fix64Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public long X { get { return __p.bb.GetLong(__p.bb_pos + 0); } }
  public long Y { get { return __p.bb.GetLong(__p.bb_pos + 8); } }
  public long Z { get { return __p.bb.GetLong(__p.bb_pos + 16); } }

  public static Offset<FlatComponents.Fix64Vec3> CreateFix64Vec3(FlatBufferBuilder builder, long X, long Y, long Z) {
    builder.Prep(8, 24);
    builder.PutLong(Z);
    builder.PutLong(Y);
    builder.PutLong(X);
    return new Offset<FlatComponents.Fix64Vec3>(builder.Offset);
  }
};


}