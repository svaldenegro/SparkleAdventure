using JoltPhysicsSharp;

namespace Sparkle.Physics.layers; 

public class BodyFilterImpl : BodyFilter {
    
    protected override bool ShouldCollide(BodyID bodyId) {
        return true;
    }

    protected override bool ShouldCollideLocked(Body body) {
        return !body.IsSensor;
    }
}