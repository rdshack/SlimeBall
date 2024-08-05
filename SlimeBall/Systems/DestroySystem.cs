using ecs;

namespace Indigo.Slimeball;

public class DestroySystem : ISystem
{
  private Query      _query;
  private EntityRepo _dataSource;

  public DestroySystem(World w)
  {
    _dataSource = w.GetEntityRepo();
    _query = new Query(w.GetArchetypes(), w.GetComponentLookup());
    Archetype a = w.GetArchetypes().With<RequestDestroyComponent>();
    _query.SetContainsArchetypeFilter(a);
  }
  
  public void Execute()
  {
    foreach (IEntityData item in _query.Resolve(_dataSource))
    {
      _dataSource.DestroyEntity(item.GetEntityId());
    }
  }
}