using System.Numerics;
using JoltPhysicsSharp;
using Sparkle.Physics.layers;

namespace Sparkle.Physics; 

public class Simulation : IDisposable {

    public PhysicsSystem PhysicsSystem { get; private set; }
    public PhysicsSettings Settings { get; private set; }
    public bool HasDisposed { get; private set; }
    
    private TempAllocator _allocator;
    private JobSystemThreadPool _jobSystem;

    private BroadPhaseLayerInterfaceImpl _broadPhaseLayer;
    private ObjectVsBroadPhaseLayerFilterImpl _broadPhaseLayerFilter;
    private ObjectLayerPairFilterImpl _objectLayerPairFilter;

    private BroadPhaseLayerFilterImpl _broadPhaseLayerFilterImpl;
    private ObjectLayerFilterImpl _objectLayerFilterImpl;
    private BodyFilterImpl _bodyFilterImpl;

    public Simulation(PhysicsSettings settings) {
        Foundation.Init();
        
        Settings = settings;
        
        _allocator = new TempAllocator(10 * (int) settings.maxContactConstraints * (int) settings.maxContactConstraints);
        _jobSystem = new JobSystemThreadPool(Foundation.MaxPhysicsJobs, Foundation.MaxPhysicsBarriers);
        
        _broadPhaseLayer = new BroadPhaseLayerInterfaceImpl();
        _broadPhaseLayerFilter = new ObjectVsBroadPhaseLayerFilterImpl();
        _objectLayerPairFilter = new ObjectLayerPairFilterImpl();
        
        _broadPhaseLayerFilterImpl = new BroadPhaseLayerFilterImpl();
        _objectLayerFilterImpl = new ObjectLayerFilterImpl();
        _bodyFilterImpl = new BodyFilterImpl();
        
        PhysicsSystem = new PhysicsSystem();
        PhysicsSystem.Init(settings.maxBodies, settings.numBodyMutexes, settings.maxBodyPairs, settings.maxContactConstraints, _broadPhaseLayer, _broadPhaseLayerFilter, _objectLayerPairFilter);
        PhysicsSystem.Gravity = settings.gravity;
        PhysicsSystem.OptimizeBroadPhase();
    }

    public void Update(float timeStep, int collisionSteps) {
        PhysicsSystem.Update(timeStep, collisionSteps, _allocator, _jobSystem);
    }

    public bool RayCast(Vector3 origin, out RayCastResult result, Vector3 direction, float distance) {
        result = RayCastResult.Default;
        
        return PhysicsSystem.NarrowPhaseQuery.CastRay((Double3) origin, direction * distance, ref result, _broadPhaseLayerFilterImpl, _objectLayerFilterImpl, _bodyFilterImpl);
    }
    
    public void Dispose() {
        if (HasDisposed) return;
        
        Dispose(true);
        GC.SuppressFinalize(this);
        HasDisposed = true;
    }
    
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            Foundation.Shutdown();
            _allocator.Dispose();
            _jobSystem.Dispose();
            _broadPhaseLayer.Dispose();
            _broadPhaseLayerFilter.Dispose();
            _broadPhaseLayerFilterImpl.Dispose();
            _objectLayerFilterImpl.Dispose();
            _objectLayerPairFilter.Dispose();
            _bodyFilterImpl.Dispose();
            PhysicsSystem.Dispose();
        }
    }
}