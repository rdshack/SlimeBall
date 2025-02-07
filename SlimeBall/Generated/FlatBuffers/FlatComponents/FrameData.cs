// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatComponents
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct FrameData : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static FrameData GetRootAsFrameData(ByteBuffer _bb) { return GetRootAsFrameData(_bb, new FrameData()); }
  public static FrameData GetRootAsFrameData(ByteBuffer _bb, FrameData obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public FrameData __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public ulong NextEntityId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUlong(o + __p.bb_pos) : (ulong)0; } }
  public int FrameNum { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public FlatComponents.NewEntityData? NewEntities(int j) { int o = __p.__offset(8); return o != 0 ? (FlatComponents.NewEntityData?)(new FlatComponents.NewEntityData()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int NewEntitiesLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }
  public ulong EntityIds(int j) { int o = __p.__offset(10); return o != 0 ? __p.bb.GetUlong(__p.__vector(o) + j * 8) : (ulong)0; }
  public int EntityIdsLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<ulong> GetEntityIdsBytes() { return __p.__vector_as_span<ulong>(10, 8); }
#else
  public ArraySegment<byte>? GetEntityIdsBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public ulong[] GetEntityIdsArray() { return __p.__vector_as_array<ulong>(10); }
  public FlatComponents.ComponentSet? ComponentState(int j) { int o = __p.__offset(12); return o != 0 ? (FlatComponents.ComponentSet?)(new FlatComponents.ComponentSet()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int ComponentStateLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FlatComponents.FrameData> CreateFrameData(FlatBufferBuilder builder,
      ulong nextEntityId = 0,
      int frameNum = 0,
      VectorOffset newEntitiesOffset = default(VectorOffset),
      VectorOffset entityIdsOffset = default(VectorOffset),
      VectorOffset componentStateOffset = default(VectorOffset)) {
    builder.StartTable(5);
    FrameData.AddNextEntityId(builder, nextEntityId);
    FrameData.AddComponentState(builder, componentStateOffset);
    FrameData.AddEntityIds(builder, entityIdsOffset);
    FrameData.AddNewEntities(builder, newEntitiesOffset);
    FrameData.AddFrameNum(builder, frameNum);
    return FrameData.EndFrameData(builder);
  }

  public static void StartFrameData(FlatBufferBuilder builder) { builder.StartTable(5); }
  public static void AddNextEntityId(FlatBufferBuilder builder, ulong nextEntityId) { builder.AddUlong(0, nextEntityId, 0); }
  public static void AddFrameNum(FlatBufferBuilder builder, int frameNum) { builder.AddInt(1, frameNum, 0); }
  public static void AddNewEntities(FlatBufferBuilder builder, VectorOffset newEntitiesOffset) { builder.AddOffset(2, newEntitiesOffset.Value, 0); }
  public static VectorOffset CreateNewEntitiesVector(FlatBufferBuilder builder, Offset<FlatComponents.NewEntityData>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateNewEntitiesVectorBlock(FlatBufferBuilder builder, Offset<FlatComponents.NewEntityData>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartNewEntitiesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddEntityIds(FlatBufferBuilder builder, VectorOffset entityIdsOffset) { builder.AddOffset(3, entityIdsOffset.Value, 0); }
  public static VectorOffset CreateEntityIdsVector(FlatBufferBuilder builder, ulong[] data) { builder.StartVector(8, data.Length, 8); for (int i = data.Length - 1; i >= 0; i--) builder.AddUlong(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateEntityIdsVectorBlock(FlatBufferBuilder builder, ulong[] data) { builder.StartVector(8, data.Length, 8); builder.Add(data); return builder.EndVector(); }
  public static void StartEntityIdsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 8); }
  public static void AddComponentState(FlatBufferBuilder builder, VectorOffset componentStateOffset) { builder.AddOffset(4, componentStateOffset.Value, 0); }
  public static VectorOffset CreateComponentStateVector(FlatBufferBuilder builder, Offset<FlatComponents.ComponentSet>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateComponentStateVectorBlock(FlatBufferBuilder builder, Offset<FlatComponents.ComponentSet>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartComponentStateVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FlatComponents.FrameData> EndFrameData(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatComponents.FrameData>(o);
  }
};


}
