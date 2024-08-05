using System;
using FixMath.NET;

namespace SimMath;

/// <summary>
/// Partial so that we can extend with constructors which accept types of 3rd party libraries like Unity.math
/// </summary>
public partial struct Fix64Vec3 : IEquatable<Fix64Vec3>
{
  public Fix64 x;
  public Fix64 y;
  public Fix64 z;

  public Fix64Vec3(int x, int y, int z)
  {
    this.x = (Fix64) x;
    this.y = (Fix64) y;
    this.z = (Fix64) z;
  }

  private static Fix64Vec3 _zero;
  public static Fix64Vec3 Zero
  {
    get { return _zero; }
  }

  public static Fix64Vec3 FromRaw(long x, long y, long z)
  {
    Fix64Vec3 vec3 = new Fix64Vec3();
    vec3.x = Fix64.FromRaw(x);
    vec3.y = Fix64.FromRaw(y);
    vec3.z = Fix64.FromRaw(z);
    return vec3;
  }

  public Fix64Vec3(Fix64 x, Fix64 y, Fix64 z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  public override bool Equals(object obj)
  {
    return obj is Fix64Vec3 && 
           ((Fix64Vec3)obj).x == x &&
           ((Fix64Vec3)obj).y == y &&
           ((Fix64Vec3)obj).z == z;
  }
  
  public override int GetHashCode()
  {
    return HashCode.Combine(x.RawValue, y.RawValue, z.RawValue);
  }

  public bool Equals(Fix64Vec3 other)
  {
    return x == other.x && y == other.y && z == other.z;
  }

  public static Fix64Vec3 operator /(Fix64Vec3 a, Fix64 b)
  {
    a.x /= b;
    a.y /= b;
    a.z /= b;
    return a;
  }
  
  public static Fix64Vec3 operator /(Fix64Vec3 a, int b)
  {
    return a / (Fix64)b;
  }
  
  public static Fix64Vec3 operator *(Fix64Vec3 v, Fix64 b)
  {
    v.x *= b;
    v.y *= b;
    v.z *= b;
    return v;
  }
  
  public static Fix64Vec3 operator *(Fix64 scalar, Fix64Vec3 v)
  {
    return v * scalar;
  }
  
  public static Fix64Vec3 operator *(Fix64Vec3 v, int scalar)
  {
    return v * (Fix64)scalar;
  }
  
  public static Fix64Vec3 operator *(int scalar, Fix64Vec3 v)
  {
    return v * (Fix64)scalar;
  }
  
  public static Fix64Vec3 operator +(Fix64Vec3 a, Fix64Vec3 b)
  {
    a.x += b.x;
    a.y += b.y;
    a.z += b.z;
    return a;
  }
  
  public static Fix64Vec3 operator -(Fix64Vec3 a, Fix64Vec3 b)
  {
    a.x -= b.x;
    a.y -= b.y;
    a.z -= b.z;
    return a;
  }
}