using Raylib_cs;
using Sparkle.Audio;

namespace Sparkle.Content.processor; 

public class MusicProcessor : IContentProcessor {
    
    public object Load(string path) {
        return MusicPlayer.LoadStream(path);
    }

    public void Unload(object item) {
        MusicPlayer.UnloadStream((Music) item);
    }
}