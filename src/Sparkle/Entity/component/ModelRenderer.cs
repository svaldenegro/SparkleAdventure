using System.Numerics;
using Raylib_cs;
using Sparkle.Scene;

namespace Sparkle.Entity.component; 

public class ModelRenderer : Component {
    
    private Model _model;
    private Texture2D _texture;
    private MaterialMapIndex _materialMap;
    private Color _color;
    
    private bool _drawWires;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ModelRenderer"/>, setting the model, texture, material map, optional color, and wireframe rendering option.
    /// </summary>
    /// <param name="model">The 3D model to be rendered.</param>
    /// <param name="texture">The texture to be applied to the model.</param>
    /// <param name="materialMap">The type of material map to be used. Default is MaterialMapIndex.MATERIAL_MAP_ALBEDO.</param>
    /// <param name="color">Optional color to be applied to the model. Default is white.</param>
    /// <param name="drawWires">Optional flag to indicate whether to render the model in wireframe. Default is false.</param>
    public ModelRenderer(Model model, Texture2D? texture = null, MaterialMapIndex materialMap = MaterialMapIndex.MATERIAL_MAP_ALBEDO, Color? color = null, bool drawWires = false) {
        _model = model;
        _materialMap = materialMap;
        _color = color ?? Color.WHITE;
        _drawWires = drawWires;
        
        if (texture is not null)
        {
            _texture = texture.Value;
            Raylib.SetMaterialTexture(ref _model, 0, materialMap, ref _texture);
        }
    }
    
    protected internal override unsafe void Draw() {
        base.Draw();
        
        SceneManager.MainCamera!.BeginMode3D();
        
        Vector3 axis;
        float angle;
        
        Raymath.QuaternionToAxisAngle(Entity.rotation, &axis, &angle);
        
        if (_drawWires) {
            Raylib.DrawModelWiresEx(_model, Entity.position, axis, angle * Raylib.RAD2DEG, Entity.scale, _color);
        }
        else {
            Raylib.DrawModelEx(_model, Entity.position, axis, angle * Raylib.RAD2DEG, Entity.scale, _color);
        }
        
        SceneManager.MainCamera.EndMode3D();
    }
}