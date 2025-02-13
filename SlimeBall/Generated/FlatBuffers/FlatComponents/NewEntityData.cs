// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct NewEntityData : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static NewEntityData GetRootAsNewEntityData(ByteBuffer _bb) { return GetRootAsNewEntityData(_bb, new NewEntityData()); }
  public static NewEntityData GetRootAsNewEntityData(ByteBuffer _bb, NewEntityData obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public NewEntityData __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public ulong EntityId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public int StateHash { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<FlatComponents.NewEntityData> CreateNewEntityData(FlatBufferBuilder builder,
      ulong entityId = 0,
      int stateHash = 0) {
    builder.StartTable(2);
    NewEntityData.AddEntityId(builder, entityId);
    NewEntityData.AddStateHash(builder, stateHash);
    return NewEntityData.EndNewEntityData(builder);
  }

  public static void StartNewEntityData(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddEntityId(FlatBufferBuilder builder, ulong entityId) { builder.AddUlong(0, entityId, 0); }
  public static void AddStateHash(FlatBufferBuilder builder, int stateHash) { builder.AddInt(1, stateHash, 0); }
  public static Offset<FlatComponents.NewEntityData> EndNewEntityData(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatComponents.NewEntityData>(o);
  }
};


}
