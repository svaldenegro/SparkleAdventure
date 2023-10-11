using System.Numerics;

namespace Sparkle.Physics; 

public struct PhysicsSettings {

    public Vector3 gravity;
    public uint maxBodies;
    public uint numBodyMutexes;
    public uint maxBodyPairs;
    public uint maxContactConstraints;

    public PhysicsSettings() {
        gravity = new Vector3(0, -9.81F, 0);
        maxBodies = 70000;
        numBodyMutexes = 0;
        maxBodyPairs = 70000;
        maxContactConstraints = 70000;
    }
}