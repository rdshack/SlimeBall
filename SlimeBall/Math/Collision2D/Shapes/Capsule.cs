using System;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D;

public class Capsule
{
  private Circle         _tail;
  private Circle         _head;
  private Polygon        _interiorBox;
  private ShapeAllocator _allocator;
  private bool           _setup;

  public Circle Tail
  {
    get { return _tail; }
  }
  
  public Circle Head
  {
    get { return _head; }
  }
  
  public Polygon InteriorBox
  {
    get { return _interiorBox; }
  }

  public void Setup(Fix64Vec2 tail, Fix64Vec2 head, Fix64 radius, ShapeAllocator allocator)
  {
    if (_setup)
    {
      throw new Exception();
    }

    _setup = true;
    _allocator = allocator;
    _tail = new Circle(tail, radius);
    _head = new Circle(head, radius);
    _interiorBox = _allocator.GetPolygon();
    
    //build interior box
    Fix64Vec2 spine = _head.Center - _tail.Center;
    Fix64Vec2 spinePerp = MathUtil.Normalize(new Fix64Vec2(spine.y, -spine.x));
    
    _interiorBox.SetupStart(4);
    _interiorBox.AddVert(_head.Center + spinePerp * _head.Radius);
    _interiorBox.AddVert(_head.Center - spinePerp * _head.Radius);
    _interiorBox.AddVert(_tail.Center + spinePerp * _tail.Radius);
    _interiorBox.AddVert(_tail.Center - spinePerp * _tail.Radius);
    _interiorBox.SetupEnd();
  }

  public void Reset()
  {
    if (!_setup)
    {
      throw new Exception();
    }

    _setup = false;
    _tail = _head = default;
    _allocator.ReturnPolygon(_interiorBox);
  }

  public static Capsule Create()
  {
    return new Capsule();
  }
  
  public static void Reset(Capsule c)
  {
    c.Reset();
  }
}