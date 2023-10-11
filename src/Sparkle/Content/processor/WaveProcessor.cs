using Raylib_cs;
using Sparkle.Audio;

namespace Sparkle.Content.processor; 

public class WaveProcessor : IContentProcessor {
    
    public object Load(string path) {
        return WavePlayer.Load(path);
    }

    public void Unload(object item) {
        WavePlayer.Unload((Wave) item);
    }
}