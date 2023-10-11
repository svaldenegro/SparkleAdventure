using Raylib_cs;
using Sparkle.Audio;

namespace Sparkle.Content.processor; 

public class SoundProcessor : IContentProcessor {
    
    public object Load(string path) {
        return SoundPlayer.Load(path);
    }

    public void Unload(object item) {
        SoundPlayer.Unload((Sound) item);
    }
}