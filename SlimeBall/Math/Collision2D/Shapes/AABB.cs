using System;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D
{
  public struct AABB : IEquatable<AABB>
  {
    private Fix64Vec2 _min;
    private Fix64Vec2 _max;
    private Fix64Vec2 _center;

    public Fix64Vec2 Center
    {
      get { return _center; }
    }

    public Fix64Vec2 Size
    {
      get { return _max - _min; }
    }

    public Fix64Vec2 GetMin()
    {
      return _min;
    }

    public Fix64Vec2 GetMax()
    {
      return _max;
    }

    public static AABB Create(Fix64Vec2 center, Fix64Vec2 size)
    {
      return new AABB(center - size / 2, center + size / 2);
    }

    private AABB(Fix64Vec2 min, Fix64Vec2 max)
    {
      _min = min;
      _max = max;
      _center = MathUtil.Lerp(_min, _max, (Fix64) 0.5f);
    }

    public void Move(Fix64Vec2 center)
    {
      Fix64Vec2 size = Size;
      _center = center;
      _min = _center - size / 2;
      _min = _center + size / 2;
    }

    public bool OverlapsAABB(AABB other)
    {
      return ((_max.x >= other.GetMin().x) & (_min.x <= other.GetMin().x)) &&
             ((_max.x >= other.GetMin().y) & (_min.x <= other.GetMin().y));
    }

    /// <summary>
    /// From https://stackoverflow.com/questions/28343716/sphere-intersection-test-of-aabb/28344608#28344608
    /// </summary>
    public bool OverlapsCircle(Circle c)
    {
      Fix64 dmin = Fix64.Zero;
      for (int i = 0; i < 2; i++)
      {
        if (c.Center[i] < _min[i])
        {
          Fix64 dist = c.Center[i] - _min[i];
          dmin += dist * dist;
        }
        else if (c.Center[i] > _max[i])
        {
          Fix64 dist = c.Center[i] - _max[i];
          dmin += dist * dist;
        }
      }

      return dmin <= c.Radius * c.Radius;
    }

    public bool Contains(Fix64Vec2 p)
    {
      return p.x > _min.x && p.x < _max.x &&
             p.y > _min.y && p.y < _max.y;
    }
    
    public override bool Equals(object obj)
    {
      return obj is AABB && 
             ((AABB)obj)._max == _max &&
             ((AABB)obj)._min == _min;
    }
  
    public override int GetHashCode()
    {
      return HashCode.Combine(_min, _max);
    }

    public bool Equals(AABB other)
    {
      return _min == other._min && _max == other._max;
    }

    public AABB Expand(Fix64Vec2 expansion)
    {
      return Create(_center, Size + expansion);
    }
  }
}