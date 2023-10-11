using System.Numerics;
using Raylib_cs;

namespace Sparkle.Graphics.util; 

public static class ShaderHelper {
    
    /// <inheritdoc cref="Raylib.LoadShader(string, string)"/>
    public static Shader Load(string vsFileName, string fsFileName) => Raylib.LoadShader(vsFileName, fsFileName);
    
    /// <inheritdoc cref="Raylib.LoadShaderFromMemory(string, string)"/>
    public static Shader LoadFromMemory(string vsFileName, string fsFileName) => Raylib.LoadShaderFromMemory(vsFileName, fsFileName);
    
    /// <inheritdoc cref="Raylib.UnloadShader"/>
    public static void Unload(Shader shader) => Raylib.UnloadShader(shader);

    
    /// <inheritdoc cref="Raylib.BeginShaderMode"/>
    public static void BeginMode(Shader shader) => Raylib.BeginShaderMode(shader);
    
    /// <inheritdoc cref="Raylib.EndShaderMode"/>
    public static void EndMode() => Raylib.EndShaderMode();

    
    /// <inheritdoc cref="Raylib.IsShaderReady"/>
    public static bool IsReady(Shader shader) => Raylib.IsShaderReady(shader);
    
    /// <inheritdoc cref="Raylib.GetShaderLocationAttrib(Shader, string)"/>
    public static int GetLocationAttribute(Shader shader, string attributeName) => Raylib.GetShaderLocationAttrib(shader, attributeName);
    
    
    /// <inheritdoc cref="Raylib.SetShaderValue"/>
    public static void SetValue<T>(Shader shader, int locIndex, T value, ShaderUniformDataType uniformType) where T : unmanaged => Raylib.SetShaderValue(shader, locIndex, value, uniformType);
    
    /// <inheritdoc cref="Raylib.SetShaderValueMatrix"/>
    public static void SetValueMatrix(Shader shader, int locIndex, Matrix4x4 mat) => Raylib.SetShaderValueMatrix(shader, locIndex, mat);
    
    /// <inheritdoc cref="Raylib.SetShaderValueTexture"/>
    public static void SetValueTexture(Shader shader, int locIndex, Texture2D texture) => Raylib.SetShaderValueTexture(shader, locIndex, texture);
}