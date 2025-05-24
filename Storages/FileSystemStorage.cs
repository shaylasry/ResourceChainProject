using System.Text.Json;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Storages
{
    public class FileSystemStorage<T> : IWritableStorage<T>
    {
        private readonly string _filePath;
        private readonly TimeSpan _expirationInterval;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public FileSystemStorage(string filePath, TimeSpan expirationInterval)
        {
            _filePath = filePath;
            _expirationInterval = expirationInterval;
        }

        public bool HasValue => File.Exists(_filePath) && new FileInfo(_filePath).Length > 0;

        public bool IsExpired
        {
            get
            {
                if (!HasValue) return true;
                var lastWrite = File.GetLastWriteTimeUtc(_filePath);
                return DateTime.UtcNow > lastWrite.Add(_expirationInterval);
            }
        }

        public async Task<T?> Read()
        {
            await _lock.WaitAsync();
            try
            {
                if (IsExpired)
                    return default;

                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task Write(T value)
        {
            await _lock.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(value);
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
