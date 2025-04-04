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

    public void SaveFile(string path)
    {
        File.Create($"{_basePath}/{path}").Dispose();
    }
}