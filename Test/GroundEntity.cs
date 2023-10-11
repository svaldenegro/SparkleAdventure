using System.Numerics;
using JoltPhysicsSharp;
using Sparkle.Entity;
using Sparkle.Entity.component;

namespace Test; 

public class GroundEntity : Entity {

    public GroundEntity(Vector3 position) : base(position) {
        AddComponent(new Rigidbody(new BoxShape(new Vector3(100000, 1, 100000)), MotionType.Static));
    }
}