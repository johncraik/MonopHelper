using Microsoft.AspNetCore.Http;

namespace MonopolyCL;

public interface ICsvReader<out T>
    where T : class
{
    public IEnumerable<T>? UploadFile(IFormFile file);
    public IEnumerable<T>? UploadFile(FileStream file);
}