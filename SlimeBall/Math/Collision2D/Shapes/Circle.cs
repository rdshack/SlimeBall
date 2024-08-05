using System;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D;

public struct Circle : IEquatable<Circle>
{
  private readonly Fix64Vec2 _center;
  private readonly Fix64 _radius;

  public Fix64Vec2 Center
  {
    get { return _center; }
  }

  public Fix64 Radius
  {
    get { return _radius; }
  }
  
  public static Circle Create(Fix64Vec2 center, Fix64 radius)
  {
    return new Circle(center, radius);
  }

  public Circle(Fix64Vec2 center, Fix64 radius)
  {
    _center = center;
    _radius = radius;
  }
  
  public override bool Equals(object obj)
  {
    return obj is Circle && 
           ((Circle)obj)._center == _center &&
           ((Circle)obj)._radius == _radius;
  }
  
  public override int GetHashCode()
  {
    return HashCode.Combine(_center, _radius);
  }

  public bool Equals(Circle other)
  {
    return _center == other._center && _radius == other._radius;
  }
}