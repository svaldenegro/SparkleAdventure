using JoltPhysicsSharp;
using Sparkle.Physics;
using Sparkle.Physics.layers;

namespace Sparkle.Entity.component; 

public class Rigidbody : Component {
    
    public Simulation Simulation => Game.Instance.Simulation;
    public BodyInterface BodyInterface => Simulation.PhysicsSystem.BodyInterface;
    
    public Shape Shape { get; private set; }
    public MotionType MotionType { get; private set; }
    public BodyID BodyId { get; private set; }
    
    public float friction;
    public float restitution;
    
    private ObjectLayer _objectLayer;
    
    public Rigidbody(Shape shape, MotionType type, float friction = 0, float restitution = 0) {
        Shape = shape;
        MotionType = type;
        this.friction = friction;
        this.restitution = restitution;
    }

    protected internal override void Init() {
        base.Init();
        
        switch (MotionType) {
            case MotionType.Static:
                _objectLayer = Layers.NonMoving;
                break;
            case MotionType.Kinematic:
                _objectLayer = Layers.Moving;
                break;
            case MotionType.Dynamic:
                _objectLayer = Layers.Moving;
                break;
        }
        
        BodyCreationSettings settings = new BodyCreationSettings(Shape, Entity.position, Entity.rotation, MotionType, _objectLayer);

        Body body = BodyInterface.CreateBody(settings);
        body.Friction = friction;
        body.Restitution = restitution;
        
        BodyInterface.AddBody(body, Activation.Activate);
        BodyId = body.ID;
    }

    protected internal override void AfterUpdate() {
        base.AfterUpdate();
        
        BodyInterface.SetPositionAndRotationWhenChanged(BodyId, new Double3(Entity.position.X, Entity.position.Y, Entity.position.Z), Entity.rotation, Activation.Activate);
    }

    protected internal override void FixedUpdate() {
        base.FixedUpdate();
        
        Entity.position = BodyInterface.GetPosition(BodyId);
        Entity.rotation = BodyInterface.GetRotation(BodyId);
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);

        if (disposing) {
            BodyInterface.RemoveBody(BodyId);
            BodyInterface.DestroyBody(BodyId);
            Shape.Dispose();
        }
    }
}