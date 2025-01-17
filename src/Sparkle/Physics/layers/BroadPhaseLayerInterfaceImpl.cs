using System.Diagnostics;
using JoltPhysicsSharp;

namespace Sparkle.Physics.layers; 

public class BroadPhaseLayerInterfaceImpl : BroadPhaseLayerInterface {

    private readonly BroadPhaseLayer[] _objectToBroadPhase;

    public BroadPhaseLayerInterfaceImpl() {
        _objectToBroadPhase = new BroadPhaseLayer[Layers.NumLayers];
        _objectToBroadPhase[Layers.NonMoving] = BroadPhaseLayers.NonMoving;
        _objectToBroadPhase[Layers.Moving] = BroadPhaseLayers.Moving;
    }

    protected override int GetNumBroadPhaseLayers() {
        return BroadPhaseLayers.NumLayers;
    }

    protected override BroadPhaseLayer GetBroadPhaseLayer(ObjectLayer layer) {
        Debug.Assert(layer < Layers.NumLayers);
        return _objectToBroadPhase[layer];
    }

    protected override string GetBroadPhaseLayerName(BroadPhaseLayer layer) {
        switch (layer) {
            case BroadPhaseLayers.NonMoving:
                return "NON_MOVING";
            
            case BroadPhaseLayers.Moving:
                return "MOVING";
            
            default:
                Debug.Assert(false);
                return "INVALID";
        }
    }
}