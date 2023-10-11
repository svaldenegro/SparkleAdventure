using JoltPhysicsSharp;

namespace Sparkle.Physics.layers; 

public class BroadPhaseLayerFilterImpl : BroadPhaseLayerFilter {
    
    protected override bool ShouldCollide(BroadPhaseLayer layer) {
        return true;
    }
}