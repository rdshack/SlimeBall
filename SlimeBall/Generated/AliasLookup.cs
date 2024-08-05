//Generated code


using System;
using System.Collections.Generic;
using ecs;

public class AliasLookup : IAliasLookup
{

  public static AliasId Slime = new AliasId(1);
  public static AliasId Ball = new AliasId(2);
  public static AliasId PlayerInput = new AliasId(3);
  public static AliasId AddPlayerInput = new AliasId(4);
  public static AliasId CollisionEvent = new AliasId(5);
  private       List<AliasId> _inputAlias = new List<AliasId>();
  private Dictionary<AliasId, HashSet<ComponentTypeIndex>> _associatedComponents =
    new Dictionary<AliasId, HashSet<ComponentTypeIndex>>();

  private Dictionary<Archetype, AliasId> _archetypeToAliasLookup = new Dictionary<Archetype, AliasId>();

  private Dictionary<AliasId, ComponentTypeIndex> _inputAliasKeyComponent =
    new Dictionary<AliasId, ComponentTypeIndex>();

  public AliasLookup()
  {

    var index = new ComponentDefinitions();
    var graph = new ArchetypeGraph(index, this);
    var SlimeIndices = new HashSet<ComponentTypeIndex>()
                                          {
                                            index.GetIndex<PositionComponent>(),  
                                            index.GetIndex<VelocityComponent>(),  
                                            index.GetIndex<TimeComponent>(),  
                                            index.GetIndex<PlayerOwnedComponent>(),  
                                            index.GetIndex<PlayerMovementComponent>(),  
                                            index.GetIndex<CircleColliderComponent>(),  
                                            index.GetIndex<GravityComponent>(),  
                                          };
    _associatedComponents.Add(Slime, SlimeIndices);
    _archetypeToAliasLookup.Add(graph.GetArchetypeFor(SlimeIndices), Slime);
    var BallIndices = new HashSet<ComponentTypeIndex>()
                                          {
                                            index.GetIndex<PositionComponent>(),  
                                            index.GetIndex<RotationComponent>(),  
                                            index.GetIndex<VelocityComponent>(),  
                                            index.GetIndex<RotationVelocityComponent>(),  
                                            index.GetIndex<BallMovementComponent>(),  
                                            index.GetIndex<GravityComponent>(),  
                                            index.GetIndex<CircleColliderComponent>(),  
                                            index.GetIndex<TimeComponent>(),  
                                          };
    _associatedComponents.Add(Ball, BallIndices);
    _archetypeToAliasLookup.Add(graph.GetArchetypeFor(BallIndices), Ball);
    _inputAlias.Add(PlayerInput);
    _inputAliasKeyComponent.Add(PlayerInput, index.GetIndex<PlayerOwnedComponent>());
    var PlayerInputIndices = new HashSet<ComponentTypeIndex>()
                                          {
                                            index.GetIndex<PlayerOwnedComponent>(),  
                                            index.GetIndex<PlayerInputComponent>(),  
                                          };
    _associatedComponents.Add(PlayerInput, PlayerInputIndices);
    _archetypeToAliasLookup.Add(graph.GetArchetypeFor(PlayerInputIndices), PlayerInput);
    _inputAlias.Add(AddPlayerInput);
    _inputAliasKeyComponent.Add(AddPlayerInput, index.GetIndex<PlayerOwnedComponent>());
    var AddPlayerInputIndices = new HashSet<ComponentTypeIndex>()
                                          {
                                            index.GetIndex<PlayerOwnedComponent>(),  
                                            index.GetIndex<CreateNewPlayerInputComponent>(),  
                                          };
    _associatedComponents.Add(AddPlayerInput, AddPlayerInputIndices);
    _archetypeToAliasLookup.Add(graph.GetArchetypeFor(AddPlayerInputIndices), AddPlayerInput);
    var CollisionEventIndices = new HashSet<ComponentTypeIndex>()
                                          {
                                            index.GetIndex<CollisionEventComponent>(),  
                                          };
    _associatedComponents.Add(CollisionEvent, CollisionEventIndices);
    _archetypeToAliasLookup.Add(graph.GetArchetypeFor(CollisionEventIndices), CollisionEvent);
  }
  public IEnumerable<ComponentTypeIndex> GetAssociatedComponents(AliasId id)
  {
    return _associatedComponents[id];
  }

  public IEnumerable<AliasId> GetInputAlias()
  {
    return _inputAlias;
  }

  public ComponentTypeIndex GetInputAliasKeyComponent(AliasId aliasId)
  {
    return _inputAliasKeyComponent[aliasId];
  }

  public AliasId GetAliasForArchetype(Archetype a)
  {
    return _archetypeToAliasLookup[a];
  }
}
