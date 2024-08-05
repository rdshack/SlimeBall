using System;
using ecs;

namespace Indigo.Collision2D;

public struct ColliderId : IEquatable<ColliderId>, IComparable<ColliderId>
{
  public readonly ulong Id;

  private static ulong _nextId = 1;

  public ColliderId(ulong id)
  {
    Id = id;
  }

  public static ColliderId NextId()
  {
    return new ColliderId(_nextId++);
  }

  public bool IsValid()
  {
    return Id != 0;
  }

  public bool Equals(ColliderId other)
  {
    return Id == other.Id;
  }

  public override bool Equals(object? obj)
  {
    return obj is ColliderId other && Equals(other);
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode();
  }
  
  public static bool operator == (ColliderId a, ColliderId b) => a.Equals(b);
  public static bool operator != (ColliderId a, ColliderId b) => !a.Equals(b);

  public int CompareTo(ColliderId other)
  {
    return Id.CompareTo(other.Id);
  }
}