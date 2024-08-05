namespace Indigo.Collision2D;

public class ShapeAllocator
{
  private ObjPool<Polygon> _polygonPool;
  private ObjPool<Capsule> _capsulePool;

  public ShapeAllocator()
  {
    _polygonPool = new ObjPool<Polygon>(Polygon.Create, Polygon.Reset);
    _capsulePool = new ObjPool<Capsule>(Capsule.Create, Capsule.Reset);
  }

  public void ReturnPolygon(Polygon p)
  {
    _polygonPool.Return(p);
  }

  public Polygon GetPolygon()
  {
    return _polygonPool.Get();
  }
}