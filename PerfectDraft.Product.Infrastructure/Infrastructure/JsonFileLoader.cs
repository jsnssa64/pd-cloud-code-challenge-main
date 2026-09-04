using System.IO.Abstractions;
using System.Text.Json;

namespace PerfectDraft.Product.Infrastructure.Infrastructure
{
    public class JsonFileLoader(IFileSystem FileSystem) : IJsonFileLoader
    {
        public async Task<IEnumerable<T>> ReadAllJsonFileAsync<T>(string path, CancellationToken cancellationToken)
        {
            using var stream = FileSystem.File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream, cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException($"Unable to Deserialize '{path}' to {typeof(T)}");
        }
    }
}