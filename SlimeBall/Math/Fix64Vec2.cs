using System;
using FixMath.NET;

namespace SimMath;

public partial struct Fix64Vec2 : IEquatable<Fix64Vec2>
{
  public Fix64 x;
  public Fix64 y;

  public Fix64Vec2(int x, int y)
  {
    this.x = (Fix64) x;
    this.y = (Fix64) y;
  }
  
  public Fix64Vec2(int x, Fix64 y)
  {
    this.x = (Fix64) x;
    this.y = y;
  }
  
  public Fix64Vec2(long x, long y)
  {
    this.x = (Fix64) x;
    this.y = (Fix64) y;
  }
  
  public Fix64Vec2(Fix64 x, Fix64 y)
  {
    this.x = x;
    this.y = y;
  }

  private static Fix64Vec2 _zero;
  public static Fix64Vec2 Zero
  {
    get { return _zero; }
  }
  
  public Fix64 this[int index]
  {
    get
    {
      switch (index)
      {
        case 0: return x;
        case 1 : return y;
        default: throw new Exception();
      }
    }
    set
    {
      switch (index)
      {
        case 0: x = value;
          break;
        case 1 : y = value;
          break;
        default: throw new Exception();
      }
    }
  }

  public static Fix64Vec2 FromRaw(long x, long y)
  {
    Fix64Vec2 newVec2 = new Fix64Vec2();
    newVec2.x = Fix64.FromRaw(x);
    newVec2.y = Fix64.FromRaw(y);
    return newVec2;
  }

  public override bool Equals(object obj)
  {
    return obj is Fix64Vec2 && 
           ((Fix64Vec2)obj).x == x &&
           ((Fix64Vec2)obj).y == y;
  }
  
  public override int GetHashCode()
  {
    return HashCode.Combine(x.RawValue, y.RawValue);
  }

  public bool Equals(Fix64Vec2 other)
  {
    return x == other.x && y == other.y;
  }

  public Fix64Vec2 MoveTowards(Fix64Vec2 target, Fix64 maxMovement)
  {
    Fix64Vec2 moveDelta = target - this;
    Fix64 distSq = MathUtil.LengthSq(moveDelta);
    Fix64 maxMoveSq = maxMovement * maxMovement;

    if (maxMoveSq >= distSq)
    {
      return target;
    }

    Fix64Vec2 dirNorm = MathUtil.Normalize(moveDelta);
    return this + maxMovement * dirNorm;
  }

  public static Fix64Vec2 operator /(Fix64Vec2 a, Fix64 b)
  {
    a.x /= b;
    a.y /= b;
    return a;
  }
  
  public static Fix64Vec2 operator /(Fix64Vec2 a, int b)
  {
    return a / (Fix64)b;
  }
  
  public static Fix64Vec2 operator *(Fix64Vec2 a, Fix64 b)
  {
    a.x *= b;
    a.y *= b;
    return a;
  }
  
  public static Fix64Vec2 operator *(Fix64 b, Fix64Vec2 a)
  {
    a.x *= b;
    a.y *= b;
    return a;
  }
  
  public static Fix64Vec2 operator *(Fix64Vec2 a, int b)
  {
    return a * (Fix64)b;
  }
  
  
  public static Fix64Vec2 operator +(Fix64Vec2 a, Fix64Vec2 b)
  {
    a.x += b.x;
    a.y += b.y;
    return a;
  }
  
  public static Fix64Vec2 operator -(Fix64Vec2 a, Fix64Vec2 b)
  {
    a.x -= b.x;
    a.y -= b.y;
    return a;
  }
  
  public static bool operator ==(Fix64Vec2 a, Fix64Vec2 b)
  {
    return a.Equals(b);
  }
  
  public static bool operator !=(Fix64Vec2 a, Fix64Vec2 b)
  {
    return !a.Equals(b);
  }
}