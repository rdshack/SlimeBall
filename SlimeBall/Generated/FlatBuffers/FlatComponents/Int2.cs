// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Int2 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Int2 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int X { get { return __p.bb.GetInt(__p.bb_pos + 0); } }
  public int Y { get { return __p.bb.GetInt(__p.bb_pos + 4); } }

  public static Offset<FlatComponents.Int2> CreateInt2(FlatBufferBuilder builder, int X, int Y) {
    builder.Prep(4, 8);
    builder.PutInt(Y);
    builder.PutInt(X);
    return new Offset<FlatComponents.Int2>(builder.Offset);
  }
};


}