//Generated code


using System;
using System.Collections.Generic;
using ecs;

public enum CollisionLayer
{
  Pawn,
  Ball,
  TerrainAll,
  TerrainBall,
  TerrainPawn
}
public enum StaticGeoCollisionResponse
{
  Slide,
  Bounce
}
public enum GamePhase
{
  WaitingForAllPlayers,
  WaitingForServe,
  PointInPlay,
  ScoreResolution,
  GameOver
}
public enum CollisionType
{
  BallStatic,
  BallBall
}
public enum StaticColliderType
{
  Floor,
  Wall,
  InvisibleSeperator,
  Net
}
