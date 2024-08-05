using ecs;

namespace Indigo.Slimeball;

//TODO: generate
public class WorldInitContext : IWorldUtilities
{
  private ComponentDefinitions _componentDefinitions;
  private IFrameSerializer      _frameSerializer;
  private IAliasLookup          _aliasLookup;
  private IWorldLogger          _logger;

  public WorldInitContext(IWorldLogger logger)
  {
    _componentDefinitions = new ComponentDefinitions();

    IWorldLogger loggerForSerialization = null;
    if ((logger.GetLogFlags() & LogFlags.SerializationDetails) == LogFlags.SerializationDetails)
    {
      loggerForSerialization = logger;
    }
    
    _frameSerializer = new FlatBufferFrameSerializer(loggerForSerialization);
    _aliasLookup = new AliasLookup();
    _logger = logger;
  }
  
  public IComponentDefinitions GetComponentIndex()
  {
    return _componentDefinitions;
  }

  public IFrameSerializer GetSerializer()
  {
    return _frameSerializer;
  }

  public IComponentFactory BuildComponentFactory()
  {
    return new ComponentCopier(_componentDefinitions);
  }

  public IWorldLogger GetLogger()
  {
    return _logger;
  }

  public IAliasLookup GetAliasDefinition()
  {
    return _aliasLookup;
  }
}