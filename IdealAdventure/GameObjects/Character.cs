using System.Numerics;
using GameContent;
using JoltPhysicsSharp;
using Raylib_cs;
using Sparkle;
using Sparkle.Content;
using Sparkle.Entity;
using Sparkle.Entity.component;

namespace IdealAdventure.GameObjects;

public class Character : Entity
{
    private readonly Model _model;
    private readonly ModelRenderer _renderer;
    private readonly CapsuleShape _shape;
    private readonly Rigidbody _rigidbody;

    public Character(Vector3 position) : base(position)
    {
         _model = ContentManager.LoadModel(ContentPath.HumanPath);
         _renderer = new ModelRenderer(_model);
         AddComponent(_renderer);

         _shape = new CapsuleShape(1, 0.35f);
         _rigidbody = new Rigidbody(_shape, MotionType.Dynamic, 0, 0);

         AddComponent(_rigidbody);
    }
}