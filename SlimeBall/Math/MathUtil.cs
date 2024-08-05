using System;
using FixMath.NET;
using Indigo.Collision2D;

namespace SimMath;

public static class MathUtil
{
  public static Fix64 LengthSq(Fix64Vec2 v)
  {
    return v.x * v.x + v.y * v.y;
  }
  
  public static Fix64 LengthSq(Fix64Vec3 v)
  {
    return v.x * v.x + v.y * v.y + v.z * v.z;
  }
  
  public static Fix64 Length(Fix64Vec2 v)
  {
    return Fix64.Sqrt(LengthSq(v));
  }
  
  public static Fix64 Length(Fix64Vec3 v)
  {
    return Fix64.Sqrt(LengthSq(v));
  }

  public static Fix64Vec2 Normalize(Fix64Vec2 v)
  {
    Fix64 length = Length(v);

    if (length == Fix64.Zero)
    {
      return new Fix64Vec2(Fix64.Zero, Fix64.Zero);
    }
    
    v /= length;
    return v;
  }
  
  public static Fix64Vec2 ClampLength(Fix64Vec2 v, Fix64 maxLength)
  {
    Fix64 lengthSq = LengthSq(v);

    if (lengthSq <= maxLength * maxLength)
    {
      return v;
    }
    
    return Normalize(v) * maxLength;
  }
  
  public static Fix64Vec3 Normalize(Fix64Vec3 v)
  {
    Fix64 length = Length(v);

    if (length == Fix64.Zero)
    {
      return new Fix64Vec3(Fix64.Zero, Fix64.Zero, Fix64.Zero);
    }
    
    v /= length;
    return v;
  }

  public static Fix64Vec3 Lerp(Fix64Vec3 source, Fix64Vec3 target, Fix64 lerp)
  {
    return new Fix64Vec3(Lerp(source.x, target.x, lerp),
                         Lerp(source.y, target.y, lerp),
                         Lerp(source.z, target.z, lerp));
  }
  
  public static Fix64Vec2 Lerp(Fix64Vec2 source, Fix64Vec2 target, Fix64 lerp)
  {
    return new Fix64Vec2(Lerp(source.x, target.x, lerp),
                         Lerp(source.y, target.y, lerp));
  }
  
  public static Fix64Vec2 MoveTowards(Fix64Vec2 source, Fix64Vec2 target, Fix64 maxDist)
  {
    Fix64Vec2 delta = target - source;
    Fix64 deltaLenSq = LengthSq(delta);
    
    if (deltaLenSq > maxDist * maxDist)
    {
      Fix64Vec2 dir = delta / (Fix64.Sqrt(deltaLenSq));
      return source + dir * maxDist;
    }
    
    return target;
  }

  public static Fix64 Dot(Fix64Vec3 a, Fix64Vec3 b)
  {
    return a.x * b.x + a.y * b.y + a.z * b.z;
  }

  public static Fix64 Dot(Fix64Vec2 a, Fix64Vec2 b)
  {
    return a.x * b.x + a.y * b.y;
  }
  
  public static Fix64 Cross(Fix64Vec2 a, Fix64Vec2 b)
  {
    return a.x * b.y - b.x * a.y;
  }

  public static Fix64 Clamp(Fix64 v, Fix64 min, Fix64 max)
  {
    if (v <= min)
    {
      return min;
    }
    
    if (v >= max)
    {
      return max;
    }

    return v;
  }

  public static Fix64Vec3 Slerp(Fix64Vec3 start, Fix64Vec3 end, Fix64 percent)
  {
    start = Normalize(start);
    end = Normalize(end);
    percent = Clamp01(percent);
    
    Fix64 dot = Dot(start, end);
    dot = Clamp(dot, -Fix64.One, Fix64.One);
    Fix64 theta = Fix64.Acos(dot) * percent;
    Fix64Vec3 relativeVec = end - start * dot;

    if (LengthSq(relativeVec) > Fix64.Zero)
    {
      relativeVec = Normalize(relativeVec); 
    }

    return ((start * Fix64.Cos(theta)) + (relativeVec * Fix64.Sin(theta)));
  }

  public static Fix64 Lerp(Fix64 source, Fix64 target, Fix64 lerp)
  {
    lerp = Clamp01(lerp);
    Fix64 delta = target - source;
    return source + delta * lerp;
  }
  
  public static Fix64 MoveTowards(Fix64 source, Fix64 target, Fix64 maxDist)
  {
    Fix64 delta = target - source;
    Fix64 clampedDelta = Clamp(delta, -maxDist, maxDist);
    return source + clampedDelta;
  }

  public static Fix64 Clamp01(Fix64 v)
  {
    if (v <= Fix64.Zero)
    {
      return Fix64.Zero;
    }
    
    if (v >= Fix64.One)
    {
      return Fix64.One;
    }

    return v;
  }

  public static Fix64 Sign(Fix64 v)
  {
    if (v > Fix64.Zero)
    {
      return Fix64.One;
    }
    
    if (v < Fix64.Zero)
    {
      return -Fix64.One;
    }

    return Fix64.Zero;
  }

  public static Fix64 Max(Fix64 v1, Fix64 v2)
  {
    if (v1 > v2)
    {
      return v1;
    }

    return v2;
  }
  
  public static Fix64 Min(Fix64 v1, Fix64 v2)
  {
    if (v1 < v2)
    {
      return v1;
    }

    return v2;
  }
}