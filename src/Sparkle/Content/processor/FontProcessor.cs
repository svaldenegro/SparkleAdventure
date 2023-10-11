using Raylib_cs;
using Sparkle.Graphics.util;

namespace Sparkle.Content.processor; 

public class FontProcessor : IContentProcessor {
    
    public object Load(string path) {
        return FontHelper.Load(path);
    }

    public void Unload(object item) {
        FontHelper.Unload((Font) item);
    }
}