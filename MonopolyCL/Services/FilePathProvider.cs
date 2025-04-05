using Microsoft.Extensions.Configuration;

namespace MonopolyCL.Services;

public class FilePathProvider
{
    public FilePathProvider(IConfiguration config)
    {
        _basePath = config["FILE-BASE-PATH"]!;
    }

    private readonly string _basePath;

    public string GetFile(string path) => File.ReadAllText($"{_basePath}/{path}");
    public string[] GetFiles(string path) => Directory.GetFiles($"{_basePath}/{path}");

    public string[] GetFileNames(string path, bool ensureExists = true)
    {
        if(ensureExists) EnsureFolderExists(path);
        
        var files = GetFiles(path);
        var names = new string[files.Length];
        var i = 0;
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            names[i] = name;
            i++;
        }

        return names;
    }

    public void EnsureFolderExists(string path)
    {
        var basePath = $"{_basePath}/{path}";
        var exists = Directory.Exists(basePath);
        if (!exists)
        {
            Directory.CreateDirectory(basePath);
        }
    }

    public async Task SaveFile(string folder, string file, string content)
    {
        var basePath = $"{_basePath}/{folder}";
        EnsureFolderExists(basePath);
        var path = $"{basePath}/{file}";

        await using var output = new StreamWriter(path, false);
        await output.WriteLineAsync(content);
    }
}