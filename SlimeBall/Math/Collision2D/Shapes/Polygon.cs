using System;
using System.Buffers;
using System.Collections.Generic;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D;

//Convex only
public class Polygon
{
  private Fix64Vec2[] _vertices;
  private Fix64Vec2[] _normals;
  private bool        _active;
  private bool        _finalized;
  private int         _vertCount;
  private int         _nextVertIndex;
  
  public int VertCount
  {
    get { return _vertCount; }
  }

  public void SetupStart(int vertCount)
  {
    if (_active)
    {
      throw new Exception();
    }

    _vertCount = vertCount;
    _active = true;
    _vertices = ArrayPool<Fix64Vec2>.Shared.Rent(vertCount);
    _normals = ArrayPool<Fix64Vec2>.Shared.Rent(vertCount);
  }

  public void Reset()
  {
    if (!_active)
    {
      throw new Exception();
    }
    
    _active = false;
    _finalized = false;
    _nextVertIndex = 0;
    ArrayPool<Fix64Vec2>.Shared.Return(_vertices);
    ArrayPool<Fix64Vec2>.Shared.Return(_normals);
    _vertices = null;
    _normals = null;
  }

  public void AddVert(Fix64Vec2 point)
  {
    if (_finalized || !_active)
    {
      throw new Exception();
    }
    
    _vertices[_nextVertIndex++] = point;
  }

  public void SetupEnd()
  {
    if (!Validate())
    {
      throw new Exception();
    }
    
    //build all normals
    for (int i = 0; i < _vertCount; i++)
    {
      Fix64Vec2 v1 = _vertices[i];
      Fix64Vec2 v2 = _vertices[(i + 1)%_vertCount];
      Fix64Vec2 v3 = _vertices[(i + 2)%_vertCount];

      Fix64 curWinding = MathUtil.Sign(MathUtil.Cross(v2 - v1, v3 - v2));
      Fix64Vec2 edge = v2 - v1;
      Fix64Vec2 edgeOutwardPerp = curWinding == Fix64.One ? 
                           new Fix64Vec2(edge.y, -edge.x) : 
                           new Fix64Vec2(-edge.y, edge.x);

      _normals[i] = MathUtil.Normalize(edgeOutwardPerp);
    }
    
    _finalized = true;
  }
  
  public Fix64Vec2 GetVert(int index)
  {
    if (!_finalized || !_active)
    {
      throw new Exception();
    }
    
    if (index >= _vertCount || index < 0)
    {
      throw new IndexOutOfRangeException();
    }

    return _vertices[index];
  }
  
  public Fix64Vec2 GetNormal(int index)
  {
    if (!_finalized || !_active)
    {
      throw new Exception();
    }
    
    if (index >= _vertCount || index < 0)
    {
      throw new IndexOutOfRangeException();
    }

    return _normals[index];
  }
  
  private bool Validate()
  {
    if (_nextVertIndex != _vertCount)
    {
      return false;
    }
    
    //confirm vertices wind in the same direction
    //todo: also test self intersection
    bool windingSet = false;
    Fix64 windingSign = Fix64.Zero;
    for (int i = 0; i < _vertCount; i++)
    {
      Fix64Vec2 v1 = _vertices[i];
      Fix64Vec2 v2 = _vertices[(i + 1)%_vertCount];
      Fix64Vec2 v3 = _vertices[(i + 2)%_vertCount];

      Fix64 curWinding = MathUtil.Sign(MathUtil.Cross(v2 - v1, v3 - v2));
      if (!windingSet)
      {
        windingSet = true;
        windingSign = curWinding;
      }
      else
      {
        if (windingSign != curWinding)
        {
          return false;
        }
      }
    }

    return true;
  }

  public static Polygon Create()
  {
    return new Polygon();
  }

  public static void Reset(Polygon p)
  {
    p.Reset();
  }
}