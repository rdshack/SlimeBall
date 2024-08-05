//Generated code


using System;
using System.Collections.Generic;
using ecs;

public class ComponentDefinitions : IComponentDefinitions
{
  private List<ComponentTypeIndex>             _indices   = new List<ComponentTypeIndex>();
  private Dictionary<Type, ComponentTypeIndex> _typeToIdx = new Dictionary<Type, ComponentTypeIndex>();
  private Dictionary<ComponentTypeIndex, Type> _idxToType = new Dictionary<ComponentTypeIndex, Type>();

  private HashSet<Type>               _singletonComponentTypes         = new HashSet<Type>();
  private HashSet<ComponentTypeIndex> _singletonComponentTypeIndices   = new HashSet<ComponentTypeIndex>();
  private List<ComponentTypeIndex>    _singletonComponentList          = new List<ComponentTypeIndex>();
  private List<ComponentTypeIndex> _singleFrameComponentTypeIndices = new List<ComponentTypeIndex>();

  public ComponentDefinitions()
  {
    var idx0 = new ComponentTypeIndex(0);
    _indices.Add(idx0);
    _typeToIdx[typeof(PositionComponent)] = idx0;
    _idxToType[idx0] = typeof(PositionComponent);

    var idx1 = new ComponentTypeIndex(1);
    _indices.Add(idx1);
    _typeToIdx[typeof(RotationComponent)] = idx1;
    _idxToType[idx1] = typeof(RotationComponent);

    var idx2 = new ComponentTypeIndex(2);
    _indices.Add(idx2);
    _typeToIdx[typeof(VelocityComponent)] = idx2;
    _idxToType[idx2] = typeof(VelocityComponent);

    var idx3 = new ComponentTypeIndex(3);
    _indices.Add(idx3);
    _typeToIdx[typeof(RotationVelocityComponent)] = idx3;
    _idxToType[idx3] = typeof(RotationVelocityComponent);

    var idx4 = new ComponentTypeIndex(4);
    _indices.Add(idx4);
    _typeToIdx[typeof(TimeComponent)] = idx4;
    _idxToType[idx4] = typeof(TimeComponent);

    var idx5 = new ComponentTypeIndex(5);
    _indices.Add(idx5);
    _typeToIdx[typeof(BallMovementComponent)] = idx5;
    _idxToType[idx5] = typeof(BallMovementComponent);

    var idx6 = new ComponentTypeIndex(6);
    _indices.Add(idx6);
    _typeToIdx[typeof(GravityComponent)] = idx6;
    _idxToType[idx6] = typeof(GravityComponent);

    var idx7 = new ComponentTypeIndex(7);
    _indices.Add(idx7);
    _typeToIdx[typeof(CircleColliderComponent)] = idx7;
    _idxToType[idx7] = typeof(CircleColliderComponent);

    var idx8 = new ComponentTypeIndex(8);
    _indices.Add(idx8);
    _typeToIdx[typeof(PlayerMovementComponent)] = idx8;
    _idxToType[idx8] = typeof(PlayerMovementComponent);

    var idx9 = new ComponentTypeIndex(9);
    _indices.Add(idx9);
    _typeToIdx[typeof(PlayerOwnedComponent)] = idx9;
    _idxToType[idx9] = typeof(PlayerOwnedComponent);

    var idx10 = new ComponentTypeIndex(10);
    _indices.Add(idx10);
    _typeToIdx[typeof(RequestDestroyComponent)] = idx10;
    _idxToType[idx10] = typeof(RequestDestroyComponent);

    var idx11 = new ComponentTypeIndex(11);
    _indices.Add(idx11);
    _typeToIdx[typeof(GameComponent)] = idx11;
    _idxToType[idx11] = typeof(GameComponent);

    var idx12 = new ComponentTypeIndex(12);
    _indices.Add(idx12);
    _typeToIdx[typeof(CollisionEventComponent)] = idx12;
    _idxToType[idx12] = typeof(CollisionEventComponent);

    var idx13 = new ComponentTypeIndex(13);
    _indices.Add(idx13);
    _typeToIdx[typeof(PawnSpawnEventComponent)] = idx13;
    _idxToType[idx13] = typeof(PawnSpawnEventComponent);

    var idx14 = new ComponentTypeIndex(14);
    _indices.Add(idx14);
    _typeToIdx[typeof(PlayerInputComponent)] = idx14;
    _idxToType[idx14] = typeof(PlayerInputComponent);

    var idx15 = new ComponentTypeIndex(15);
    _indices.Add(idx15);
    _typeToIdx[typeof(CreateNewPlayerInputComponent)] = idx15;
    _idxToType[idx15] = typeof(CreateNewPlayerInputComponent);

    foreach(var idx in _indices)
    {
      var componentType = _idxToType[idx];
      bool isSingleton = Attribute.GetCustomAttribute(componentType, typeof(SingletonComponent)) != null;
      if(isSingleton)
      {
        _singletonComponentTypes.Add(componentType);
        _singletonComponentTypeIndices.Add(idx);
        _singletonComponentList.Add(idx);
      }
      bool isSingleFrame = Attribute.GetCustomAttribute(componentType, typeof(SingleFrameComponent)) != null;
      if(isSingleFrame)
      {
        _singleFrameComponentTypeIndices.Add(idx);
      }
    }
  }

  public Type GetComponentType(ComponentTypeIndex idx)
  {
    return _idxToType[idx];
  }

  public ComponentTypeIndex GetIndex(IComponent c)
  {
    return _typeToIdx[c.GetType()];
  }

  public ComponentTypeIndex GetIndex(Type cType)
  {
    return _typeToIdx[cType];
  }

  public IEnumerable<ComponentTypeIndex> GetTypeIndices()
  {
    return _indices;
  }

  public ComponentTypeIndex GetIndex<T>() where T : IComponent, new()
  {
    return _typeToIdx[typeof(T)];
  }

  public bool IsSingletonComponent(ComponentTypeIndex idx)
  {
    return _singletonComponentTypeIndices.Contains(idx);
  }

  public List<ComponentTypeIndex> GetAllSingletonComponents()
  {
    return _singletonComponentList;
  }

  public List<ComponentTypeIndex> GetAllSingleFrameComponents()
  {
    return _singleFrameComponentTypeIndices;
  }

  public bool MatchesComponentFieldKey(IComponent c, object val)
  {
    switch (GetIndex(c).Index)
    {
      case 9:
        if (val is not int playerOwnedComponentKeyVal)
        {
          return false;
        }

        PlayerOwnedComponent playerOwnedComponent = (PlayerOwnedComponent) c;
        return playerOwnedComponent.playerId == playerOwnedComponentKeyVal;
      default:
        return false;
    }
  }

  public int CompareComponentFieldKeys(IComponent a, IComponent b)
    {
    if (a.GetType() != b.GetType())
    {
      throw new Exception();
    }

    switch (GetIndex(a).Index)
    {
      case 9:
        PlayerOwnedComponent playerOwnedComponentA = (PlayerOwnedComponent) a;
        PlayerOwnedComponent playerOwnedComponentB = (PlayerOwnedComponent) b;
        return playerOwnedComponentA.playerId.CompareTo(playerOwnedComponentB.playerId);
      default:
        throw new Exception();
    }
  }
}
