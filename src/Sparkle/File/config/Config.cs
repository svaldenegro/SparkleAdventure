using Newtonsoft.Json.Linq;

namespace Sparkle.File.config; 

public class Config {
    
    public string Directory { get; }
    public string Name { get; }
    public string Path { get; }
    
    private readonly Dictionary<string, object> _dictionary;
    private readonly string _encryptKey;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Config"/>, specifying the directory, name, initial dictionary of settings, and optional encryption key.
    /// </summary>
    /// <param name="directory">The directory where the config file will be located.</param>
    /// <param name="name">The name of the config file without extension.</param>
    /// <param name="dictionary">The initial dictionary of settings to be saved in the config.</param>
    /// <param name="encryptKey">Optional encryption key for securing the config file. Default is an empty string, meaning no encryption.</param>
    public Config(string directory, string name, Dictionary<string, object> dictionary, string encryptKey = "") {
        Directory = directory;
        Name = name;
        Path = FileManager.GetPath(Directory, $"{Name}.json");
        _dictionary = dictionary;
        _encryptKey = encryptKey;
        Create();
    }
    
    /// <summary>
    /// Creates or updates a configuration file with the values in the current dictionary.
    /// </summary>
    private void Create() {
        FileManager.CreateFile(Directory, $"{Name}.json");
        
        if (FileManager.IsJsonValid(Path, _encryptKey)) {
            foreach (var jsonObjectPair in FileManager.ReadJsonAsObject(Path, _encryptKey)) {
                if (!_dictionary.ContainsKey(jsonObjectPair.Key)) {
                    JObject jsonObject = FileManager.ReadJsonAsObject(Path, _encryptKey);
                    jsonObject.Remove(jsonObjectPair.Key);
                    
                    System.IO.File.WriteAllText(Path, FileManager.EncryptString(jsonObject.ToString(), _encryptKey));
                    Logger.Warn($"Value {jsonObjectPair.Key} removed from file {Name}");
                }
            }
            
            foreach (var dictionary in _dictionary) {
                if (!FileManager.ReadJsonAsObject(Path, _encryptKey).ContainsKey(dictionary.Key)) {
                    JObject jsonObject = FileManager.ReadJsonAsObject(Path, _encryptKey);

                    jsonObject[dictionary.Key] = JToken.FromObject(dictionary.Value);
                    System.IO.File.WriteAllText(Path, FileManager.EncryptString(jsonObject.ToString(), _encryptKey));
                    Logger.Info($"File {Name} updated: {dictionary.Key} = {dictionary.Value}");
                }
            }
        }
        else {
            FileManager.ClearFile(Path);
            FileManager.WriteJson(_dictionary, Path, _encryptKey);
            Logger.Warn($"Re/Wrote {Name}");
        }
    }
    
    /// <summary>
    /// Retrieves a value of a specific type from a collection of values stored as JToken objects.
    /// </summary>
    /// <param name="name">The name of the value to retrieve.</param>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <returns>The retrieved value of type T if found; otherwise, the default value or null.</returns>
    public T GetValue<T>(string name) {
        if (!GetAllValues().TryGetValue(name, out JToken? jToken)) {
            Logger.Error($"Unable to locate value [{name}]!");
        }
        
        return jToken!.Value<T>()!;
    }

    /// <summary>
    /// Retrieves all values as a JObject from a JSON file using the specified path and encryption key.
    /// </summary>
    /// <returns>A JObject containing all values from the JSON file.</returns>
    public JObject GetAllValues() {
        return FileManager.ReadJsonAsObject(Path, _encryptKey);
    }
}