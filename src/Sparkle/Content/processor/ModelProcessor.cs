using Raylib_cs;
using Sparkle.Graphics.util;

namespace Sparkle.Content.processor; 

public class ModelProcessor : IContentProcessor {

    public object Load(string path) {
        return ModelHelper.Load(path);
    }
    
    public void Unload(object item) {
        ModelHelper.Unload((Model) item);
    }
}