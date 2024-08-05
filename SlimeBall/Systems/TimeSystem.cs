
using ecs;

namespace Indigo.Slimeball;

public class TimeSystem : ISystem
{
  public const int MsPerFrame = 25;
  
  private Query             _query;
  private EntityRepo _dataSource;

  public TimeSystem(World w)
  {
    _dataSource = w.GetEntityRepo();
    _query = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().With<TimeComponent>();
    _query.SetContainsArchetypeFilter(a);
  }
  
  public void Execute()
  {
    foreach (IEntityData item in _query.Resolve(_dataSource))
    {
      TimeComponent timeComponent = item.Get<TimeComponent>();
      timeComponent.deltaTimeMs = MsPerFrame;
    }
  }
}