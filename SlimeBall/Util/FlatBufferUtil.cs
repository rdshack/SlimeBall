using System;
using System.Collections.Generic;
using FlatBuffers;


public static class FlatBufferUtil
{
  public static VectorOffset AddVectorToBufferFromOffsetList<T>(
    FlatBufferBuilder              builder, 
    Action<FlatBufferBuilder, int> vectorStarter, 
    List<Offset<T>>                offsets) where T : struct
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (offsets.Count == 0)
    {
      return new VectorOffset(0);
    }
            
    vectorStarter(builder, offsets.Count);
    for (int i = offsets.Count - 1; i >= 0; i--)
    {
      builder.AddOffset(offsets[i].Value);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromUlongList(
    FlatBufferBuilder              builder, 
    Action<FlatBufferBuilder, int> vectorStarter, 
    List<ulong>                offsets)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (offsets.Count == 0)
    {
      return new VectorOffset(0);
    }
            
    vectorStarter(builder, offsets.Count);
    for (int i = offsets.Count - 1; i >= 0; i--)
    {
      builder.AddUlong(offsets[i]);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromLongList(
    FlatBufferBuilder              builder, 
    Action<FlatBufferBuilder, int> vectorStarter, 
    List<long>                    offsets)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (offsets.Count == 0)
    {
      return new VectorOffset(0);
    }
            
    vectorStarter(builder, offsets.Count);
    for (int i = offsets.Count - 1; i >= 0; i--)
    {
      builder.AddLong(offsets[i]);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromIntList(
    FlatBufferBuilder              builder, 
    Action<FlatBufferBuilder, int> vectorStarter, 
    List<int>                    offsets)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (offsets.Count == 0)
    {
      return new VectorOffset(0);
    }
            
    vectorStarter(builder, offsets.Count);
    for (int i = offsets.Count - 1; i >= 0; i--)
    {
      builder.AddInt(offsets[i]);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromStringList(
    FlatBufferBuilder              builder,
    List<StringOffset>                    offsets)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (offsets.Count == 0)
    {
      return new VectorOffset(0);
    }
            
    builder.StartVector(4, offsets.Count, 4);
    for (int i = offsets.Count - 1; i >= 0; i--)
    {
      builder.AddOffset(offsets[i].Value);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromByteList(
    FlatBufferBuilder builder,
    List<byte>        bytes)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (bytes.Count == 0)
    {
      return new VectorOffset(0);
    }

    builder.StartVector(1, bytes.Count, 1);
    for (int i = bytes.Count - 1; i >= 0; i--)
    {
      builder.AddByte(bytes[i]);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromByteArray(
    FlatBufferBuilder builder,
    byte[]        bytes,
    int size)
  {
    if (size > bytes.Length)
    {
      throw new Exception();
    }
    
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (bytes.Length == 0)
    {
      return new VectorOffset(0);
    }

    builder.StartVector(1, size, 1);
    for (int i = size - 1; i >= 0; i--)
    {
      builder.AddByte(bytes[i]);    
    }

    return builder.EndVector();
  }
  
  public static VectorOffset AddVectorToBufferFromByteArraySeg(
    FlatBufferBuilder  builder,
    ArraySegment<byte> bytes)
  {
    //no need to start a vector (wastes 4 bytes of flat cost)
    if (bytes.Count == 0)
    {
      return new VectorOffset(0);
    }
    
    builder.StartVector(1, bytes.Count, 1);
    for (int i = bytes.Count - 1; i >= 0; i--)
    {
      builder.AddByte(bytes[i]);    
    }

    return builder.EndVector();
  }
}