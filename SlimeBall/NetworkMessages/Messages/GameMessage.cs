// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Messages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct GameMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static GameMessage GetRootAsGameMessage(ByteBuffer _bb) { return GetRootAsGameMessage(_bb, new GameMessage()); }
  public static GameMessage GetRootAsGameMessage(ByteBuffer _bb, GameMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public GameMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string MsgId { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetMsgIdBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetMsgIdBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetMsgIdArray() { return __p.__vector_as_array<byte>(4); }
  public long MsgCreationTime { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0; } }
  public Messages.GameMessageType Type { get { int o = __p.__offset(8); return o != 0 ? (Messages.GameMessageType)__p.bb.GetSbyte(o + __p.bb_pos) : Messages.GameMessageType.JoinGame; } }
  public byte Content(int j) { int o = __p.__offset(10); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int ContentLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetContentBytes() { return __p.__vector_as_span<byte>(10, 1); }
#else
  public ArraySegment<byte>? GetContentBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] GetContentArray() { return __p.__vector_as_array<byte>(10); }

  public static Offset<Messages.GameMessage> CreateGameMessage(FlatBufferBuilder builder,
      StringOffset msgIdOffset = default(StringOffset),
      long msgCreationTime = 0,
      Messages.GameMessageType type = Messages.GameMessageType.JoinGame,
      VectorOffset contentOffset = default(VectorOffset)) {
    builder.StartTable(4);
    GameMessage.AddMsgCreationTime(builder, msgCreationTime);
    GameMessage.AddContent(builder, contentOffset);
    GameMessage.AddMsgId(builder, msgIdOffset);
    GameMessage.AddType(builder, type);
    return GameMessage.EndGameMessage(builder);
  }

  public static void StartGameMessage(FlatBufferBuilder builder) { builder.StartTable(4); }
  public static void AddMsgId(FlatBufferBuilder builder, StringOffset msgIdOffset) { builder.AddOffset(0, msgIdOffset.Value, 0); }
  public static void AddMsgCreationTime(FlatBufferBuilder builder, long msgCreationTime) { builder.AddLong(1, msgCreationTime, 0); }
  public static void AddType(FlatBufferBuilder builder, Messages.GameMessageType type) { builder.AddSbyte(2, (sbyte)type, 0); }
  public static void AddContent(FlatBufferBuilder builder, VectorOffset contentOffset) { builder.AddOffset(3, contentOffset.Value, 0); }
  public static VectorOffset CreateContentVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateContentVectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static void StartContentVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static Offset<Messages.GameMessage> EndGameMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<Messages.GameMessage>(o);
  }
};


}
