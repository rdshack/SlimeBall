
using System;
using System.Collections.Generic;
using System.Numerics;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D
{
  [Flags]
  public enum ColliderType
  {
    AABB = 1,
    Circle = 1 << 1
  }

  public static class Collision2D
  {
    private static Fix64 EPSILON = new Fix64(1) / new Fix64(1_000_000);

    private static List<Fix64Vec2> _tempVec2s = new List<Fix64Vec2>();
    
    //todo: convert from float2 to Fix64Vec2
    /*public static bool RayIntersectRay(float2     rayAStart, 
                                       float2     rayADir, 
                                       float2     rayBStart, 
                                       float2     rayBDir,
                                       out float2 intersect)
    {
      float2 paramSolution;
      if (GetIntersectionParameters(rayAStart, rayAStart + rayADir, rayBStart, rayBStart + rayBDir, out paramSolution))
      {
        float t = paramSolution.x;
        float u = paramSolution.y;

        if (t > -EPSILON && u > -EPSILON)
        {
          intersect = rayAStart + t * rayADir;
          return true;
        }
      }

      intersect = new float2(0, 0);
      return false;
    }

    public static float3 ClosestPointOnRayFromPosition(float3 rayOrigin, float3 rayDir, float3 position)
    {
      float3 rayVector = (rayOrigin + rayDir) - rayOrigin;
      float3 posVector = position - rayOrigin;
      float dotProduct = dot(posVector, rayVector);
      float param = -1;

      if (lengthsq(rayVector) != 0)
      {
        param = dotProduct / lengthsq(rayVector);
      }

      if (param < 0)
      {
        return rayOrigin;
      }
      else
      {
        return rayOrigin + param * rayVector;
      }
    }*/

    public static bool CircleOverlapsCircle(Circle c1, Circle c2)
    {
      return MathUtil.LengthSq(c2.Center - c1.Center) < (c1.Radius + c2.Radius) * (c1.Radius + c2.Radius);
    }

    public static bool Aabb2DOverlapsAabb2D(AABB bb1, AABB bb2)
    {
      return bb1.OverlapsAABB(bb2);
    }
    
    public static bool Aabb2DOverlapsCircle(AABB bb, Circle c)
    {
      return bb.OverlapsCircle(c);
    }

    public static bool CircleOverlapsPolygon(Circle c, Polygon p)
    {
      return CircleOverlapsPolygon(c, p, Fix64.Zero, out _);
    }

    public static bool CircleOverlapsPolygon(Circle c, Polygon p, Fix64 sepAmount, out Fix64Vec2 sepVec)
    {
      sepVec = Fix64Vec2.Zero;
      _tempVec2s.Clear();
      for (int i = 0; i < p.VertCount; i++)
      {
        _tempVec2s.Add(p.GetNormal(i));
      }

      Fix64Vec2 closestVert = Fix64Vec2.Zero;
      Fix64 closestDist = Fix64.MaxValue;
      for (int i = 0; i < p.VertCount; i++)
      {
        Fix64Vec2 v = p.GetVert(i);
        Fix64 distSq = MathUtil.LengthSq(c.Center - v);
        if (distSq < closestDist)
        {
          closestDist = distSq;
          closestVert = v;
        }
      }
      
      Fix64Vec2 circleAxis = MathUtil.Normalize(c.Center - closestVert);
      _tempVec2s.Add(circleAxis);

      Fix64 minSepVecLenSq = Fix64.MaxValue;
      foreach (var axis in _tempVec2s)
      {
        Fix64Vec2 p1Projection = ProjectPolygonOnAxis(p, axis);
        Fix64Vec2 cProjection = ProjectCircleOnAxis(c, axis);

        bool overlap = p1Projection.y >= cProjection.x && p1Projection.x <= cProjection.y;
        if (!overlap)
        {
          return false;
        }

        Fix64Vec2 curSepVec;
        if (p1Projection.y >= cProjection.y)
        {
          curSepVec = axis * (p1Projection.x - cProjection.y - sepAmount);
        }
        else
        {
          curSepVec = axis * (p1Projection.y - cProjection.x + sepAmount);
        }

        Fix64 curSepVecLenSq = MathUtil.LengthSq(curSepVec);
        if (curSepVecLenSq < minSepVecLenSq)
        {
          sepVec = curSepVec;
          minSepVecLenSq = curSepVecLenSq;
        }
      }

      return true;
    }

    public static bool CapsuleOverlapsPolygon(Capsule c, Polygon p)
    {
      return PolygonOverlapsPolygon(c.InteriorBox, p) ||
             CircleOverlapsPolygon(c.Tail, p) ||
             CircleOverlapsPolygon(c.Head, p);
    }

    public static bool PolygonOverlapsPolygon(Polygon p1, Polygon p2)
    {
      _tempVec2s.Clear();
      for (int i = 0; i < p1.VertCount; i++)
      {
        _tempVec2s.Add(p1.GetNormal(i));
      }
      
      for (int i = 0; i < p2.VertCount; i++)
      {
        _tempVec2s.Add(p2.GetNormal(i));
      }

      foreach (var axis in _tempVec2s)
      {
        Fix64Vec2 p1Projection = ProjectPolygonOnAxis(p1, axis);
        Fix64Vec2 p2Projection = ProjectPolygonOnAxis(p2, axis);

        bool overlap = p1Projection.y >= p2Projection.x && p1Projection.x <= p2Projection.y;
        if (!overlap)
        {
          return false;
        }
      }

      return true;
    }

    public static Fix64Vec2 Reflect(Fix64Vec2 ray, Fix64Vec2 surfaceNormal)
    {
      Fix64 dot = MathUtil.Dot(ray, surfaceNormal);
      Fix64Vec2 perpComponent = surfaceNormal * -dot;
      return ray + new Fix64(2) * perpComponent;
    }
    
    public static Fix64Vec2 ProjectCircleOnAxis(Circle p, Fix64Vec2 normalizedAxisVector)
    {
      Fix64 pointProjection = MathUtil.Dot(p.Center, normalizedAxisVector);
      return new Fix64Vec2(pointProjection - p.Radius, pointProjection + p.Radius);
    }
  
    public static Fix64Vec2 ProjectPolygonOnAxis(Polygon p, Fix64Vec2 normalizedAxisVector)
    {
      Fix64 min = Fix64.MaxValue;
      Fix64 max = Fix64.MinValue;

      for (int i = 0; i < p.VertCount; i++)
      {
        Fix64Vec2 curVert = p.GetVert(i);
        Fix64 pointProjection = MathUtil.Dot(curVert, normalizedAxisVector);
        min = MathUtil.Min(min, pointProjection);
        max = MathUtil.Max(max, pointProjection);
      }

      return new Fix64Vec2(min, max);
    }

    private static bool GetIntersectionParameters(Fix64Vec2 a1, Fix64Vec2 a2, Fix64Vec2 b1, Fix64Vec2 b2, out Fix64Vec2 output)
    {
      Fix64Vec2 b1Ma1 = b1 - a1;
      Fix64Vec2 a = a2 - a1;
      Fix64Vec2 b = b2 - b1;

      Fix64 b1Ma1CrossA = MathUtil.Cross(b1Ma1, a);
      Fix64 b1Ma1CrossB = MathUtil.Cross(b1Ma1, b);
      Fix64 aCrossB = MathUtil.Cross(a, b);

      //check for no solution / co-linear solution
      if (Fix64.Abs(aCrossB) < EPSILON)
      {
        output = new Fix64Vec2(0, 0);
        return false;
      }

      Fix64 t = b1Ma1CrossB / aCrossB;
      Fix64 u = b1Ma1CrossA / aCrossB;

      output = new Fix64Vec2(t, u);
      return true;
    }
    
    public static bool CircleAgainstLineSweepTest(Fix64Vec2 cStart, Fix64Vec2 cEnd, Fix64 radius, Fix64Vec2 linePoint, 
                                                  Fix64Vec2 lineDirUnit, out Fix64Vec2 circleEndPos, out Fix64 tVal)
    {
      Fix64Vec2 linePointToCircleCenter = cStart - linePoint;
      Fix64Vec2 perpComponent =
        linePointToCircleCenter - (MathUtil.Dot(linePointToCircleCenter, lineDirUnit) * lineDirUnit);

      //early out if circle is overlapping line to start - we're colliding!
      if (MathUtil.LengthSq(perpComponent) <= radius * radius)
      {
        tVal = Fix64.Zero;
        circleEndPos = cStart;
        return true;
      }
      
      //move line closer to point equal to circle radius. The problem is now a point to line sweep,
      //which can be done using a parameterized line-line intersection.
      Fix64Vec2 dirTowardsCircle = MathUtil.Normalize(perpComponent);
      Fix64Vec2 newLinePoint = linePoint + dirTowardsCircle * radius;
      if (!GetIntersectionParameters(cStart, cEnd, newLinePoint, newLinePoint - lineDirUnit, out Fix64Vec2 output))
      {
        tVal = Fix64.Zero;
        circleEndPos = Fix64Vec2.Zero;
        return false;
      }
      
      if (output.x < Fix64.Zero || output.x > Fix64.One)
      {
        tVal = Fix64.Zero;
        circleEndPos = Fix64Vec2.Zero;
        return false;
      }

      tVal = output.x;
      circleEndPos = MathUtil.Lerp(cStart, cEnd, output.x);
      return true;
    }

    public static bool SegmentIntersectCircle(Fix64Vec2 point, Fix64Vec2 v, Circle cir, out Fix64 tVal)
    {
      Fix64Vec2 m = point - cir.Center;
      Fix64 b = MathUtil.Dot(m, v);
      Fix64 c = MathUtil.Dot(m, m) - cir.Radius * cir.Radius;

      if (c > Fix64.Zero && b > Fix64.Zero)
      {
        tVal = Fix64.Zero;
        return false;
      }

      Fix64 discr = b * b - c;
      if (discr < Fix64.Zero)
      {
        tVal = Fix64.Zero;
        return false;
      }

      tVal = -b - Fix64.Sqrt(discr);

      if (tVal < Fix64.Zero)
      {
        tVal = Fix64.Zero;
      }

      return true;
    }

    public static bool CircleOverlapsCircle(Circle c0, Circle c1, Fix64 sep, out Fix64Vec2 c0SepVec, out Fix64Vec2 c1SepVec)
    {
      Fix64Vec2 c0toC1 = (c0.Center - c1.Center);
      Fix64 distanceSquared = MathUtil.LengthSq(c0toC1);
      Fix64 radiusSquared = (c0.Radius + c1.Radius) * (c0.Radius + c1.Radius);

      Fix64 overlapSquared = distanceSquared - radiusSquared;
      if (overlapSquared > Fix64.Zero)
      {
        c0SepVec = Fix64Vec2.Zero;
        c1SepVec = Fix64Vec2.Zero;
        return false;
      }

      Fix64 halfOverlap = Fix64.Sqrt(Fix64.Abs(overlapSquared)) / new Fix64(2);
      Fix64 sepLength = halfOverlap + sep / new Fix64(2);

      Fix64Vec2 sepVec;
      if (distanceSquared == Fix64.Zero)
      {
        //if their centers are the same, choose an arbitrary separation vector (in this case x-axis)
        sepVec = new Fix64Vec2(Fix64.One, Fix64.Zero);
      }
      else
      {
        //otherwise use their delta vector
        sepVec = MathUtil.Normalize(c0toC1);
      }
      
      c0SepVec = sepVec * sepLength;
      c1SepVec = sepVec * -sepLength;
      return true;
    }

    public static bool IntersectRayAabb(Fix64Vec2 p, Fix64Vec2 d, AABB aabb, out Fix64 tMin, out Fix64Vec2 colPoint)
    {
      colPoint = Fix64Vec2.Zero;
      tMin = Fix64.Zero;
      Fix64 tMax = Fix64.MaxValue;

      for (int i = 0; i < 2; i++)
      {
        if (Fix64.Abs(d[i]) < EPSILON)
        {
          //ray is parallel to slab, no hit if origin is not within slab
          if (p[i] < aabb.GetMin()[i] || p[i] > aabb.GetMax()[i])
          {
            return false;
          }
        }
        else
        {
          //compute intersection t value of ray with near and far plane of slab
          Fix64 ood = Fix64.One / d[i];
          Fix64 t1 = (aabb.GetMin()[i] - p[i]) * ood;
          Fix64 t2 = (aabb.GetMax()[i] - p[i]) * ood;
          
          //make t1 be intersection with near plane, t2 with far plane
          if (t1 > t2)
          {
            Fix64 temp = t1;
            t1 = t2;
            t2 = temp;
          }
          
          //Compute the intersection of slab intersection intervals
          tMin = MathUtil.Max(tMin, t1);
          tMax = MathUtil.Min(tMax, t2);

          if (tMin > tMax)
          {
            return false;
          }
        }
      }

      colPoint = p + d * tMin;
      return true;
    }

    public static bool MovingCircleAABBSweepTest(Circle c, Fix64Vec2 moveDelta, AABB aabb, out Fix64 tVal, out Fix64Vec2 colNorm)
    {
      colNorm = Fix64Vec2.Zero;
      tVal = Fix64.Zero;
      var vLen = MathUtil.Length(moveDelta);
      if (vLen == Fix64.Zero)
      {
        return false;
      }
      
      //compute AABB of aabb after expanding it by circle along all sides
      AABB expanded = aabb.Expand(new Fix64Vec2(c.Radius + c.Radius, c.Radius + c.Radius));
      
      //intersect ray against expanded aabb.
      Fix64Vec2 p;
      if (!IntersectRayAabb(c.Center, moveDelta, expanded, out tVal, out p) || tVal > Fix64.One)
      {
        return false;
      }

      int u = 0;
      int v = 0;
      if (p.x < aabb.GetMin().x)
      {
        u |= 1;
      }
      if (p.x > aabb.GetMax().x)
      {
        v |= 1;
      }
      if (p.y < aabb.GetMin().y)
      {
        u |= 2;
      }
      if (p.y > aabb.GetMax().y)
      {
        v |= 2;
      }

      int m = u + v;
      if (m == 3)
      {
        Fix64Vec2 cornerPoint;
        switch (u)
        {
          case 0: cornerPoint = aabb.GetMax();
            break;
          case 1: cornerPoint = new Fix64Vec2(aabb.GetMin().x, aabb.GetMax().y);
            break;
          case 2: cornerPoint = new Fix64Vec2(aabb.GetMax().x, aabb.GetMin().y);
            break;
          default: cornerPoint = aabb.GetMin();
            break;
        }

        if (SegmentIntersectCircle(c.Center, moveDelta / vLen, new Circle(cornerPoint, c.Radius), out tVal))
        {
          tVal /= vLen;
          bool collision = tVal <= Fix64.One;

          if (collision)
          {
            colNorm = MathUtil.Normalize(c.Center - cornerPoint);
            return true;
          }
        }

        return false;
      }

      if (u == 1)
      {
        colNorm = new Fix64Vec2(-1, 0);
      }
      else if (u == 2)
      {
        colNorm = new Fix64Vec2(0, -1);
      }
      else if (v == 1)
      {
        colNorm = new Fix64Vec2(1, 0);
      }
      else
      {
        colNorm = new Fix64Vec2(0, 1);
      }

      return true;
    }
    

    public static bool MovingCircleMovingCircleSweepTest(Circle c0, Circle c1, Fix64Vec2 v0, Fix64Vec2 v1,
                                                         out Fix64 tVal)
    {
      tVal = Fix64.Zero;
      c1 = new Circle(c1.Center, c1.Radius + c0.Radius);
      Fix64Vec2 v = v0 - v1;

      Fix64 vLen = MathUtil.Length(v);
      if (vLen == Fix64.Zero)
      {
        return false;
      }
      
      if (SegmentIntersectCircle(c0.Center, v / vLen, c1, out tVal))
      {
        tVal /= vLen;
        return tVal <= Fix64.One;
      }

      return false;
    }
  }
}