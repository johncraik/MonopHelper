namespace MonopHelper.Services;

public class Version
{
    private readonly IConfiguration _config;

    public Version(IConfiguration config)
    {
        _config = config;
    }
    
    public string GetVersion()
    {
        return _config["version"] ?? "No Version Found";
    }
}