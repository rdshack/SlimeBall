//Generated code


using System;
using System.Collections.Generic;
using ecs;
using FixMath.NET;
using SimMath;

[SingletonComponent]
public class GameComponent : Component
{
    public GamePhase gamePhase;
    public int leftPlayerScore;
    public int rightPlayerScore;
    public EntityId leftPlayerPawn;
    public EntityId rightPlayerPawn;
    public EntityId ball;
    public bool mostRecentPointWasLeft;
}
