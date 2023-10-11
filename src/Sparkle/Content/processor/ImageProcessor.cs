using Raylib_cs;
using Sparkle.Graphics.util;

namespace Sparkle.Content.processor;

public class ImageProcessor : IContentProcessor {
    
    public object Load(string path) {
        return ImageHelper.Load(path);
    }

    public void Unload(object item) {
        ImageHelper.Unload((Image) item);
    }
}