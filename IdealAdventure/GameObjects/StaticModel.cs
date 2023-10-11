using System.Numerics;
using JoltPhysicsSharp;
using Raylib_cs;
using Sparkle.Content;
using Sparkle.Entity;
using Sparkle.Entity.component;

namespace IdealAdventure.GameObjects;

public class StaticModel : Entity
{
    private Model _model;
    private ModelRenderer _renderer;
    private Rigidbody _rigidbody;
    
    public unsafe StaticModel(Vector3 position, string model) : base(position)
    {
        _model = ContentManager.LoadModel(model);
        _renderer = new(_model);
        AddComponent(_renderer);

        var meshes = _model.meshes;
        var trisCount = 0;
        var vertCount = 0;
        for (var i = 0; i < _model.meshCount; i++)
        {
            var mesh = meshes[i];
            trisCount += mesh.triangleCount;
            vertCount += mesh.vertexCount;
        }

        var triangles = new IndexedTriangle[trisCount];
        var vertices = new Vector3[vertCount];
        trisCount = vertCount = 0;
        
        for (var i = 0; i < _model.meshCount; i++)
        {
            var mesh = meshes[i];
            for (var j = 0; j < mesh.triangleCount; j++)
            {
                triangles[trisCount++] = new IndexedTriangle(mesh.indices[j++], mesh.indices[j++], mesh.indices[j++], 0);
            }
            for (var j = 0; j < mesh.vertexCount; j++)
            {
                int v = j * 3;
                vertices[vertCount++] = new Vector3(mesh.vertices[v], mesh.vertices[v + 1], mesh.vertices[v + 2]);
            }
        }

        fixed (Vector3* vertex = vertices)
        {
            fixed (IndexedTriangle* triangle = triangles)
            {
                var shapeSettings = new MeshShapeSettings(vertex, vertCount, triangle, trisCount);
                var shape = new MeshShape(shapeSettings);
                _rigidbody = new Rigidbody(shape, MotionType.Static);
                AddComponent(_rigidbody);
            }
        }
    }
}