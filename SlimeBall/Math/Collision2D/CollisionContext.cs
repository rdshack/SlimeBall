using System;
using System.Collections.Generic;
using ecs;
using FixMath.NET;
using SimMath;

namespace Indigo.Collision2D;


//Stateless physics engine. All entity state must be passed in each tick. We could have made this stateful,
//but then we'd be duplicating entity data and need to sync back and forth. We can keep this code simpler by
//just having the ecs part of the game pass in the state that needs to be advanced
public class CollisionContext
{
  public class PhysicsWorldStateInput
  {
    public List<BallSweepInput> BallSweepData = new List<BallSweepInput>();

    public void Reset()
    {
      BallSweepData.Clear();
    }
  }

  private struct CollisionLayerPair : IEquatable<CollisionLayerPair>
  {
    public readonly int a;
    public readonly int b;

    public CollisionLayerPair(CollisionLayer a, CollisionLayer b)
    {
      this.a = (int)a;
      this.b = (int)b;
    }

    public bool Equals(CollisionLayerPair other)
    {
      return (a == other.a || a == other.b) &&
             (b == other.b || b == other.a);
    }

    public override bool Equals(object? obj)
    {
      return obj is CollisionLayerPair other && Equals(other);
    }

    //we don't want order to matter.
    public override int GetHashCode()
    {
      return HashHelper(a) + HashHelper(b);
    }

    private static int HashHelper(int a)
    {
      int x = a;
      x ^= x >> 17;
      x *= 830770091;
      return x;
    }
  }

  private struct Boundary
  {
    public AABB               aabb;
    public CollisionLayer     layer;
    public StaticColliderType staticColliderType;
  }
  
  private class PhysicsWorldState
  {
    public Boundary[]           Boundaries;
    public List<BallSweepState> BallSweepData = new List<BallSweepState>();

    public void Clear()
    {
      BallSweepData.Clear();
    }
  }
  
  public class PhysicsWorldStateOutput
  {
    public List<BallSweepState>          BallSweepData = new List<BallSweepState>();
    public List<CollisionEventComponent> Collisions    = new List<CollisionEventComponent>();

    public void Reset()
    {
      BallSweepData.Clear();
      Collisions.Clear();
    }
  }
  
  public class BallSweepInput
  {
    public EntityId                   id;
    public Fix64                      dtSec;
    public Fix64Vec2                  startPos;
    public Fix64Vec2                  velo;
    public Fix64                      radius;
    public Fix64                      mass;
    public Fix64                      rotVelo;
    public StaticGeoCollisionResponse geoCollisionResponse;
    public CollisionLayer             layer;

    public BallSweepInput()
    {
      
    }

    public void Init(EntityId id,
                          Fix64 dtSec, Fix64Vec2 startPos, Fix64Vec2 velo, 
                          Fix64 radius, Fix64 mass,
                          StaticGeoCollisionResponse response,
                          CollisionLayer layer,
                          Fix64 rotVelo)
    {
      this.id = id;
      this.dtSec = dtSec;
      this.startPos = startPos;
      this.velo = velo;
      this.radius = radius;
      this.mass = mass;
      this.geoCollisionResponse = response;
      this.layer = layer;
      this.rotVelo = rotVelo;
    }

    public static BallSweepInput Create()
    {
      return new BallSweepInput();
    }

    public static void Reset(BallSweepInput obj)
    {
      obj.id = default;
      obj.dtSec = default;
      obj.startPos = default;
      obj.velo = default;
      obj.radius = default;
      obj.mass = default;
      obj.geoCollisionResponse = default;
    }
  }
  
  public class BallSweepState
  {
    public EntityId                   id;
    public int                        index;
    public StaticGeoCollisionResponse geoCollisionResponse;
    public Fix64                      radius;
    public Fix64                      mass;
    public Fix64Vec2                  curPos;
    public Fix64                      remainingMovement;
    public Fix64Vec2                  normalizedDir; 
    public Fix64                      speed;
    public CollisionLayer             layer;
    public Fix64Vec2                  outputVelo;
    public Fix64                      outputRotationalVelo;

    public BallSweepState()
    {
    }

    public void Init(EntityId id, int index, StaticGeoCollisionResponse response,
                     Fix64Vec2 startPos, Fix64 remainingMovement, Fix64 speed,
                     Fix64 radius, Fix64 mass, Fix64Vec2 normalizedDir, CollisionLayer layer,
                     Fix64Vec2 outputVelo, Fix64 outputRotationalVelo)
    {
      this.id = id;
      this.index = index;
      this.geoCollisionResponse = response;
      this.radius = radius;
      this.mass = mass;
      this.curPos = startPos;
      this.remainingMovement = remainingMovement;
      this.normalizedDir = normalizedDir;
      this.speed = speed;
      this.layer = layer;
      this.outputVelo = outputVelo;
      this.outputRotationalVelo = outputRotationalVelo;
    }

    public static BallSweepState Create()
    {
      return new BallSweepState();
    }

    public static void Reset(BallSweepState obj)
    {
      obj.id = default;
      obj.index = default;
      obj.geoCollisionResponse = default;
      obj.radius = default;
      obj.mass = default;
      obj.curPos = default;
      obj.remainingMovement = default;
      obj.normalizedDir = default;
      obj.speed = default;
    }
  }

  private class BallSweepResult
  {
    public BallSweepState  newSweepStateA;
    public BallSweepState? newSweepStateB;
    public Boundary?       boundary      = null;
    public Fix64           collisionTVal = Fix64.One;
    public bool            collided      = false;
    public Fix64Vec2       collisionPosA;
    public Fix64Vec2       collisionPosB;
    public Fix64Vec2       collisionNormA;
    public Fix64Vec2       collisionNormB;
    
    public void Init(BallSweepState curStateA, BallSweepState curStateB)
    {
      newSweepStateA = curStateA;
      newSweepStateB = curStateB;
    }
    
    public void Init(BallSweepState curStateA)
    {
      newSweepStateA = curStateA;
      newSweepStateB = null;
    }

    public static BallSweepResult Create()
    {
      return new BallSweepResult();
    }

    public static void Reset(BallSweepResult obj)
    {
      obj.newSweepStateA = default;
      obj.newSweepStateB = default;
      obj.boundary = default;
      obj.collisionTVal = default;
      obj.collided = default;
      obj.collisionPosA = default;
      obj.collisionPosB = default;
      obj.collisionNormA = default;
      obj.collisionNormB = default;
    }
  }
  

  public static Fix64 FLOOR_HEIGHT = new Fix64(1000);
  public static Fix64 FLOOR_HEIGHT_BALL = new Fix64(1150);
  public static Fix64 C_GAP        = new Fix64(5);
  
  private HashSet<CollisionLayerPair> _collidingLayers = new HashSet<CollisionLayerPair>();
  private Boundary[]                  _boundaries;
  private Polygon[]                   _boundariesAsPolygons;

  private ObjPool<BallSweepResult> _ballSweepResultPool = new ObjPool<BallSweepResult>(BallSweepResult.Create, 
                                                                                       BallSweepResult.Reset);
  
  private ObjPool<BallSweepState> _ballSweepStatePool = new ObjPool<BallSweepState>(BallSweepState.Create, 
                                                                                     BallSweepState.Reset);

  private ObjPool<CollisionEventComponent> _collisionEventPool = new ObjPool<CollisionEventComponent>(CreateColEvent,
                                                                                                      ResetColEvent);


  private PhysicsWorldState _tempWorldState = new PhysicsWorldState();
  private List<BallSweepState> _tempBallSeepStateList = new List<BallSweepState>();

  public CollisionContext()
  {
    var leftAabb = AABB.Create(new Fix64Vec2(-7050, 0), new Fix64Vec2(100, 20_000));
    var rightAabb = AABB.Create(new Fix64Vec2(7050, 0), new Fix64Vec2(100, 20_000));
    var lowerPlayerAabb = AABB.Create(new Fix64Vec2(0, FLOOR_HEIGHT-new Fix64(50)), new Fix64Vec2(20_000, 100));
    var lowerBallAabb = AABB.Create(new Fix64Vec2(0, FLOOR_HEIGHT_BALL-new Fix64(50)), new Fix64Vec2(20_000, 100));
    var upperAabb = AABB.Create(new Fix64Vec2(0, 9_000), new Fix64Vec2(10_000, 100));
    var centerWallAabb = AABB.Create(new Fix64Vec2(0, 0), new Fix64Vec2(100, 4500));
    var centerInvisBarrierAabb = AABB.Create(new Fix64Vec2(0, 0), new Fix64Vec2(100, 20_000));

    var left = new Boundary() {aabb = leftAabb, layer = CollisionLayer.TerrainAll, staticColliderType = StaticColliderType.Wall};
    var right = new Boundary() {aabb = rightAabb, layer = CollisionLayer.TerrainAll, staticColliderType = StaticColliderType.Wall};
    var lowerPlayer = new Boundary() {aabb = lowerPlayerAabb, layer = CollisionLayer.TerrainPawn, staticColliderType = StaticColliderType.InvisibleSeperator};
    var lowerBall = new Boundary() {aabb = lowerBallAabb, layer = CollisionLayer.TerrainBall, staticColliderType = StaticColliderType.Floor};
    var upper = new Boundary() {aabb = upperAabb, layer = CollisionLayer.TerrainAll, staticColliderType = StaticColliderType.Wall};
    var centerWall = new Boundary() {aabb = centerWallAabb, layer = CollisionLayer.TerrainAll, staticColliderType = StaticColliderType.Net};
    var centerInvisBarrier = new Boundary() {aabb = centerInvisBarrierAabb, layer = CollisionLayer.TerrainPawn, staticColliderType = StaticColliderType.InvisibleSeperator};

    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Ball, CollisionLayer.Ball));
    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Ball, CollisionLayer.TerrainBall));
    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Ball, CollisionLayer.TerrainAll));
    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Ball, CollisionLayer.Pawn));
    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Pawn, CollisionLayer.TerrainPawn));
    _collidingLayers.Add(new CollisionLayerPair(CollisionLayer.Pawn, CollisionLayer.TerrainAll));

    _boundaries = new[] {left, right, upper, lowerPlayer, lowerBall, centerWall, centerInvisBarrier };
    _boundariesAsPolygons = new Polygon[_boundaries.Length];

    for(int i = 0; i < _boundaries.Length; i++)
    {
      AABB aabb = _boundaries[i].aabb;
      Polygon p = new Polygon();
      var p0 = new Fix64Vec2(aabb.GetMin().x, aabb.GetMax().y);
      var p1 = aabb.GetMax();
      var p2 = new Fix64Vec2(aabb.GetMax().x, aabb.GetMin().y);
      var p3 = aabb.GetMin();
      p.SetupStart(4);
      p.AddVert(p0);
      p.AddVert(p1);
      p.AddVert(p2);
      p.AddVert(p3);
      p.SetupEnd();

      _boundariesAsPolygons[i] = p;
    }
  }

  public void PhysUpdate(PhysicsWorldStateInput worldInput, PhysicsWorldStateOutput worldOutput)
  {
    _tempWorldState.Clear();
    _ballSweepResultPool.ReturnAll();
    _ballSweepStatePool.ReturnAll();
    _collisionEventPool.ReturnAll();

    _tempWorldState.Boundaries = _boundaries;

    for (int i = 0; i < worldInput.BallSweepData.Count; i++)
    {
      _tempWorldState.BallSweepData.Add(ConvertToBallSweepState(i, worldInput.BallSweepData[i]));
    }
    
    int iterations = 0;
    while (iterations++ < 100)
    {
      SolveOverlaps();
      Fix64 firstHitT = FindEarliestCollisionInSystem(out BallSweepResult? firstHit);

      //if anything collided, we must reset all objects to their position at the
      //time of the first collision, then resolve that collision before proceeding
      //to next iteration
      if (firstHit != null)
      {
        foreach (var sweepState in _tempWorldState.BallSweepData)
        {
          if (sweepState.index == firstHit.newSweepStateA.index ||
              (firstHit.newSweepStateB != null && sweepState.index == firstHit.newSweepStateB.index))
          {
            continue;
          }
          
          Fix64Vec2 deltaMove = sweepState.remainingMovement * sweepState.normalizedDir * firstHitT;
          sweepState.curPos += deltaMove;
          sweepState.remainingMovement -= MathUtil.Length(deltaMove);
        }

        CollisionEventComponent collisionEventComponent = ProcessCollisionResponse(firstHit);
        worldOutput.Collisions.Add(collisionEventComponent);
      }
      else
      {
        foreach (BallSweepState sweepState in _tempWorldState.BallSweepData)
        {
          Fix64Vec2 deltaMove = sweepState.normalizedDir * sweepState.remainingMovement;
          sweepState.curPos += deltaMove;
          sweepState.remainingMovement -= MathUtil.Length(deltaMove);
        }
        
        //otherwise, we can escape
        break;
      }
    }

    foreach (BallSweepState r in _tempWorldState.BallSweepData)
    {
      worldOutput.BallSweepData.Add(r);
    }
  }

  private Fix64 FindEarliestCollisionInSystem(out BallSweepResult? firstHit)
  {
    Fix64 firstHitT = Fix64.One;
    firstHit = null;
    for (int i = 0; i < _tempWorldState.BallSweepData.Count; i++)
    {
      BallSweepState a = _tempWorldState.BallSweepData[i];
      foreach (Boundary b in _tempWorldState.Boundaries)
      {
        if (!LayersDoCollide(a.layer, b.layer))
        {
          continue;
        }
        
        bool hit = BallToBoundarySweep(a, b, out BallSweepResult result);

        if (hit && result.collisionTVal < firstHitT)
        {
          firstHitT = result.collisionTVal;
          firstHit = result;
        }
      }

      for (int j = i + 1; j < _tempWorldState.BallSweepData.Count; j++)
      {
        BallSweepState b = _tempWorldState.BallSweepData[j];
        
        if (!LayersDoCollide(a.layer, b.layer))
        {
          continue;
        }

        bool hit = ProcessBallToBallSweep(a, b, out BallSweepResult result);

        if (hit && result.collisionTVal < firstHitT)
        {
          firstHitT = result.collisionTVal;
          firstHit = result;
        }
      }
    }

    return firstHitT;
  }

  private bool LayersDoCollide(CollisionLayer aLayer, CollisionLayer bLayer)
  {
    return _collidingLayers.Contains(new CollisionLayerPair(aLayer, bLayer));
  }

  private void SolveOverlaps()
  {
    int iterations = 0;
    bool overlapDetected = true;
    while (overlapDetected && iterations < 2)
    {
      overlapDetected = false;
      iterations++;
      
      for(int i = 0; i < _tempWorldState.BallSweepData.Count; i++)
      {
        BallSweepState a = _tempWorldState.BallSweepData[i];

        for(int j = 0; j < _boundaries.Length; j++)
        {
          Boundary b = _boundaries[j];
          if (!LayersDoCollide(a.layer, b.layer))
          {
            continue;
          }
          
          if (Collision2D.CircleOverlapsPolygon(new Circle(a.curPos, a.radius + C_GAP), 
                                                _boundariesAsPolygons[j], C_GAP, 
                                                out Fix64Vec2 sepVec))
          {
            overlapDetected = true;
            a.curPos += sepVec;
          }
        }
      
        for (int j = i + 1; j < _tempWorldState.BallSweepData.Count; j++)
        {
          BallSweepState b = _tempWorldState.BallSweepData[j];
          if (!LayersDoCollide(a.layer, b.layer))
          {
            continue;
          }

          if (Collision2D.CircleOverlapsCircle(new Circle(a.curPos, a.radius), new Circle(b.curPos, b.radius), C_GAP,
                                               out Fix64Vec2 aSepVec, out Fix64Vec2 bSepVec))
          {
            overlapDetected = true;
            a.curPos += aSepVec;
            b.curPos += bSepVec;
          }
        }
      }
    }
  }

  private CollisionEventComponent ProcessCollisionResponse(BallSweepResult firstHit)
  {
    if (firstHit.boundary != null)
    {
      var sweepDataA = firstHit.newSweepStateA;
      Fix64Vec2 ballStart = sweepDataA.curPos;
      Fix64Vec2 curEnd = firstHit.collisionPosA;
      
      if (sweepDataA.geoCollisionResponse == StaticGeoCollisionResponse.Slide)
      {
        //redirect motion along collision surface by taking ortho to surface normal, then inverting if wrong dir
        Fix64Vec2 surfaceParallel = new Fix64Vec2(-firstHit.collisionNormA.y, firstHit.collisionNormA.x);
        if (MathUtil.Dot(surfaceParallel, sweepDataA.normalizedDir) < Fix64.Zero)
        {
          surfaceParallel *= -1;
        }
        
        //subtract resolved movement, then scale remaining movement by the amount the vector is moving against surface
        Fix64Vec2 actualMoveDelta = curEnd - ballStart;
        sweepDataA.remainingMovement -= MathUtil.Length(actualMoveDelta);
        sweepDataA.remainingMovement *= MathUtil.Dot(sweepDataA.normalizedDir, surfaceParallel); 
          
        sweepDataA.normalizedDir = surfaceParallel;
        sweepDataA.curPos = curEnd;
      }
      else if (sweepDataA.geoCollisionResponse == StaticGeoCollisionResponse.Bounce)
      {
        //represents energy lost in collision (todo: make function of component field)
        sweepDataA.remainingMovement *= (Fix64)0.9f; 
        sweepDataA.speed *= (Fix64)0.9f;
      
        Fix64Vec2 actualMoveDelta = curEnd - ballStart;
        sweepDataA.remainingMovement -= MathUtil.Length(actualMoveDelta);

        Fix64Vec2 incomingDir = sweepDataA.normalizedDir;
        sweepDataA.normalizedDir = Collision2D.Reflect(sweepDataA.normalizedDir, firstHit.collisionNormA);

        sweepDataA.outputRotationalVelo /= new Fix64(2);
        Fix64 incomingDotOutgoing = MathUtil.Dot(incomingDir, sweepDataA.normalizedDir);
        Fix64 rollStrength = (incomingDotOutgoing + new Fix64(1)) / new Fix64(2);
        Fix64 incomingCrossOutgoing = MathUtil.Sign(MathUtil.Cross(incomingDir, sweepDataA.normalizedDir));

        sweepDataA.outputRotationalVelo += -incomingCrossOutgoing * rollStrength * sweepDataA.speed / new Fix64(20);
        
        sweepDataA.curPos = curEnd;
        sweepDataA.outputVelo = sweepDataA.normalizedDir * sweepDataA.speed;
      }
      
      CollisionEventComponent collisionEvent = new CollisionEventComponent();
      collisionEvent.collisionType = CollisionType.BallStatic;
      collisionEvent.collisionObjAEntity = firstHit.newSweepStateA.id;
      collisionEvent.collisionObjAPos = firstHit.collisionPosA;
      collisionEvent.staticCollisionType = firstHit.boundary.Value.staticColliderType;
      
      return collisionEvent;
    }
    else
    {
      var sweepDataA = firstHit.newSweepStateA;
      var sweepDataB = firstHit.newSweepStateB;
      
      Fix64Vec2 ballStartA = sweepDataA.curPos;
      Fix64Vec2 curEndA = firstHit.collisionPosA;

      Fix64Vec2 ballStartB = sweepDataB.curPos;
      Fix64Vec2 curEndB = firstHit.collisionPosB;

      Fix64Vec2 veloBallAPrior = sweepDataA.normalizedDir * sweepDataA.speed;
      Fix64Vec2 veloBallBPrior = sweepDataB.normalizedDir * sweepDataB.speed;

      Fix64Vec2 vAmvB = veloBallAPrior - veloBallBPrior;
      Fix64Vec2 vBmvA = veloBallBPrior - veloBallAPrior;

      Fix64Vec2 cAmcB = ballStartA - ballStartB;
      Fix64Vec2 cBmcA = ballStartB - ballStartA;

      Fix64 massRatioA = new Fix64(2) * sweepDataB.mass / (sweepDataA.mass + sweepDataB.mass);
      Fix64 massRatioB = new Fix64(2) * sweepDataA.mass / (sweepDataA.mass + sweepDataB.mass);

      Fix64 centDistSq = MathUtil.LengthSq(cAmcB);

      Fix64Vec2 veloBallAAfter = veloBallAPrior - (massRatioA * MathUtil.Dot(vAmvB, cAmcB) / centDistSq) * cAmcB;
      Fix64Vec2 veloBallBAfter = veloBallBPrior - (massRatioB * MathUtil.Dot(vBmvA, cBmcA) / centDistSq) * cBmcA;
      
      sweepDataA.normalizedDir = MathUtil.Normalize(veloBallAAfter);
      sweepDataB.normalizedDir = MathUtil.Normalize(veloBallBAfter);
      sweepDataA.speed = MathUtil.Length(veloBallAAfter);
      sweepDataB.speed = MathUtil.Length(veloBallBAfter);
      
      sweepDataA.remainingMovement = Fix64.Zero;
      sweepDataA.curPos = curEndA;
      sweepDataA.outputVelo = sweepDataA.normalizedDir * sweepDataA.speed;
      
      sweepDataB.remainingMovement = Fix64.Zero;
      sweepDataB.curPos = curEndB;
      sweepDataB.outputVelo = sweepDataB.normalizedDir * sweepDataB.speed;

      if (sweepDataA.layer == CollisionLayer.Ball)
      {
        sweepDataA.outputRotationalVelo /= new Fix64(2);

        Fix64Vec2 collisionVector = curEndA - curEndB;
        Fix64 speedDeltaAlongPath = sweepDataB.speed - MathUtil.Dot(sweepDataA.outputVelo, sweepDataB.normalizedDir);
        
        Fix64 rollStrength = MathUtil.Cross(sweepDataB.normalizedDir, collisionVector) * speedDeltaAlongPath * (sweepDataB.mass / new Fix64(10000));
        sweepDataA.outputRotationalVelo += rollStrength;
      }
      else if (sweepDataB.layer == CollisionLayer.Ball)
      {
        sweepDataB.outputRotationalVelo /= new Fix64(2);

        Fix64Vec2 collisionVector = curEndB - curEndA;
        Fix64 speedDeltaAlongPath = sweepDataA.speed - MathUtil.Dot(sweepDataB.outputVelo, sweepDataA.normalizedDir);
        
        Fix64 rollStrength = MathUtil.Cross(sweepDataA.normalizedDir, collisionVector) * sweepDataA.speed * (sweepDataA.mass / new Fix64(10000));
        sweepDataB.outputRotationalVelo += rollStrength;
      }

      CollisionEventComponent collisionEvent = _collisionEventPool.Get();
      collisionEvent.collisionType = CollisionType.BallBall;
      collisionEvent.collisionObjAEntity = firstHit.newSweepStateA.id;
      collisionEvent.collisionObjBEntity = firstHit.newSweepStateB.id;
      collisionEvent.collisionObjAPos = firstHit.collisionPosA;
      collisionEvent.collisionObjBPos = firstHit.collisionPosB;
      
      return collisionEvent;
    }
  }

  private bool BallToBoundarySweep(BallSweepState sweepData, Boundary boundary, out BallSweepResult result)
  {
    result = _ballSweepResultPool.Get();
    result.Init(sweepData);
    
    Fix64Vec2 ballStart = sweepData.curPos;
    Fix64Vec2 moveDelta = sweepData.normalizedDir * sweepData.remainingMovement;

    if(!Collision2D.MovingCircleAABBSweepTest(new Circle(ballStart, sweepData.radius),
                                              moveDelta,
                                              boundary.aabb,
                                              out Fix64 tVal,
                                              out Fix64Vec2 colNorm))
    {
      return false;
    }
    
    result.collisionTVal = tVal;
    result.collided = true;
    result.boundary = boundary;
    result.collisionPosA = ballStart + moveDelta * tVal;
    result.collisionNormA = colNorm;
    
    return true;
  }

  private bool ProcessBallToBallSweep(BallSweepState ballSweepStateA, 
                                                 BallSweepState ballSweepStateB, 
                                                 out BallSweepResult result)
  {
    result = _ballSweepResultPool.Get();
    result.Init(ballSweepStateA, ballSweepStateB);
    
    Circle c0 = new Circle(ballSweepStateA.curPos, ballSweepStateA.radius);
    Circle c1 = new Circle(ballSweepStateB.curPos, ballSweepStateB.radius);
    Fix64Vec2 c0MoveDelta = ballSweepStateA.normalizedDir * (ballSweepStateA.remainingMovement);
    Fix64Vec2 c1MoveDelta = ballSweepStateB.normalizedDir * (ballSweepStateB.remainingMovement);
    
    if(!Collision2D.MovingCircleMovingCircleSweepTest(c0, c1, c0MoveDelta, c1MoveDelta, out Fix64 tVal))
    {
      return false;
    }
    
    result.collisionTVal = tVal;
    result.collided = true;
    result.collisionPosA = ballSweepStateA.curPos + c0MoveDelta * tVal;
    result.collisionPosB = ballSweepStateB.curPos + c1MoveDelta * tVal;
    
    //for ball-ball collisions, we can just backtrack slightly
    result.collisionPosA = MathUtil.MoveTowards(result.collisionPosA, ballSweepStateA.curPos, C_GAP);
    result.collisionPosB = MathUtil.MoveTowards(result.collisionPosB, ballSweepStateB.curPos, C_GAP);

    return true;
  }
  
  private BallSweepState ConvertToBallSweepState(int index, BallSweepInput input)
  {
    Fix64Vec2 moveDelta = input.dtSec * input.velo;
    Fix64 remainingMovement = MathUtil.Length(moveDelta);
    Fix64 speed = MathUtil.Length(input.velo);
    
    Fix64Vec2 dir = Fix64Vec2.Zero;
    if (speed > Fix64.Zero)
    {
      dir = MathUtil.Normalize(moveDelta);
    }

    BallSweepState sweepState = _ballSweepStatePool.Get();
    sweepState.Init(input.id, index, input.geoCollisionResponse, input.startPos,
                    remainingMovement, speed, input.radius, input.mass, dir, input.layer, input.velo, input.rotVelo);
    return sweepState;
  }
  
  private static void ResetColEvent(CollisionEventComponent obj)
  {
    obj.collisionObjAEntity = default;
    obj.collisionObjBEntity = default;
    obj.collisionObjAPos = default;
    obj.collisionObjBPos = default;
    obj.collisionType = default;
  }

  private static CollisionEventComponent CreateColEvent()
  {
    return new CollisionEventComponent();
  }
}