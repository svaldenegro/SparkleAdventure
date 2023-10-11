using JoltPhysicsSharp;

namespace Sparkle.Physics.layers; 

public class ObjectLayerFilterImpl : ObjectLayerFilter {

    protected override bool ShouldCollide(ObjectLayer layer) {
        return true;
    }
}