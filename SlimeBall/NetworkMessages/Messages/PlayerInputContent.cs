// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Messages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct PlayerInputContent : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static PlayerInputContent GetRootAsPlayerInputContent(ByteBuffer _bb) { return GetRootAsPlayerInputContent(_bb, new PlayerInputContent()); }
  public static PlayerInputContent GetRootAsPlayerInputContent(ByteBuffer _bb, PlayerInputContent obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public PlayerInputContent __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int PlayerId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int LastAuthInputReceivedFromHost { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int FramesStart { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public Messages.ByteArray? Inputs(int j) { int o = __p.__offset(10); return o != 0 ? (Messages.ByteArray?)(new Messages.ByteArray()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int InputsLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
  public long FrameCreationTimestamps(int j) { int o = __p.__offset(12); return o != 0 ? __p.bb.GetLong(__p.__vector(o) + j * 8) : (long)0; }
  public int FrameCreationTimestampsLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<long> GetFrameCreationTimestampsBytes() { return __p.__vector_as_span<long>(12, 8); }
#else
  public ArraySegment<byte>? GetFrameCreationTimestampsBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public long[] GetFrameCreationTimestampsArray() { return __p.__vector_as_array<long>(12); }

  public static Offset<Messages.PlayerInputContent> CreatePlayerInputContent(FlatBufferBuilder builder,
      int playerId = 0,
      int lastAuthInputReceivedFromHost = 0,
      int framesStart = 0,
      VectorOffset inputsOffset = default(VectorOffset),
      VectorOffset frameCreationTimestampsOffset = default(VectorOffset)) {
    builder.StartTable(5);
    PlayerInputContent.AddFrameCreationTimestamps(builder, frameCreationTimestampsOffset);
    PlayerInputContent.AddInputs(builder, inputsOffset);
    PlayerInputContent.AddFramesStart(builder, framesStart);
    PlayerInputContent.AddLastAuthInputReceivedFromHost(builder, lastAuthInputReceivedFromHost);
    PlayerInputContent.AddPlayerId(builder, playerId);
    return PlayerInputContent.EndPlayerInputContent(builder);
  }

  public static void StartPlayerInputContent(FlatBufferBuilder builder) { builder.StartTable(5); }
  public static void AddPlayerId(FlatBufferBuilder builder, int playerId) { builder.AddInt(0, playerId, 0); }
  public static void AddLastAuthInputReceivedFromHost(FlatBufferBuilder builder, int lastAuthInputReceivedFromHost) { builder.AddInt(1, lastAuthInputReceivedFromHost, 0); }
  public static void AddFramesStart(FlatBufferBuilder builder, int framesStart) { builder.AddInt(2, framesStart, 0); }
  public static void AddInputs(FlatBufferBuilder builder, VectorOffset inputsOffset) { builder.AddOffset(3, inputsOffset.Value, 0); }
  public static VectorOffset CreateInputsVector(FlatBufferBuilder builder, Offset<Messages.ByteArray>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateInputsVectorBlock(FlatBufferBuilder builder, Offset<Messages.ByteArray>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartInputsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddFrameCreationTimestamps(FlatBufferBuilder builder, VectorOffset frameCreationTimestampsOffset) { builder.AddOffset(4, frameCreationTimestampsOffset.Value, 0); }
  public static VectorOffset CreateFrameCreationTimestampsVector(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); for (int i = data.Length - 1; i >= 0; i--) builder.AddLong(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateFrameCreationTimestampsVectorBlock(FlatBufferBuilder builder, long[] data) { builder.StartVector(8, data.Length, 8); builder.Add(data); return builder.EndVector(); }
  public static void StartFrameCreationTimestampsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 8); }
  public static Offset<Messages.PlayerInputContent> EndPlayerInputContent(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Messages.PlayerInputContent>(o);
  }
};


}
