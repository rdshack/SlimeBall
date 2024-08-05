//Generated code


using System;
using System.Collections.Generic;
using ecs;
using FixMath.NET;
using SimMath;

[SingleFrameComponent]
public class CollisionEventComponent : Component
{
    public EntityId collisionObjAEntity;
    public EntityId collisionObjBEntity;
    public Fix64Vec2 collisionObjAPos;
    public Fix64Vec2 collisionObjBPos;
    public CollisionType collisionType;
    public StaticColliderType staticCollisionType;
}
