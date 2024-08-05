using ecs;
using Indigo.Collision2D;
using SimMath;

namespace Indigo.Slimeball;


public class SlimeBallGame: IGame, IGameSettings
{
  private World            _world;
  private WorldInitContext _worldInitContext;
  
  public void BuildWorld(IWorldLogger logger)
  {
    _world = new World(new WorldInitContext(logger));

    //1 unity meter = 1000 game units
    CollisionContext staticGeo = new CollisionContext();
    
    _world.AddSystem(new DestroySystem(_world));
    _world.AddSystem(new TimeSystem(_world));
    _world.AddSystem(new PlayerInputSystem(_world));
    _world.AddSystem(new GravitySystem(_world));
    _world.AddSystem(new PlayerMoveSystem(_world));
    _world.AddSystem(new ApplyMotionSystem(_world, staticGeo));
    _world.AddSystem(new GameManagerSystem(_world));
  }

  public World GetWorld()
  {
    return _world;
  }

  public void Tick(IFrameInputData input)
  {
    _world.Tick(input);
  }

  IGameSettings IGame.GetSettings()
  {
    return this;
  }
  
  public double GetMsPerFrame()
  {
    return TimeSystem.MsPerFrame;
  }
}

