using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Storages
{
    public class FileSystemStorage<T> : IReadableStorage<T>, IWritableStorage<T>
    {
        private readonly string _filePath;
        private readonly TimeSpan _expiration;

        public FileSystemStorage(string filePath, TimeSpan expiration)
        {
            _filePath = filePath;
            _expiration = expiration;
        }

        public bool HasValue => File.Exists(_filePath) && new FileInfo(_filePath).Length > 0;

        public bool IsExpired
        {
            get
            {
                if (!HasValue) return true;
                var lastWrite = File.GetLastWriteTimeUtc(_filePath);
                return DateTime.UtcNow > lastWrite.Add(_expiration);
            }
        }

        public async Task<T?> Read()
        {
            if (IsExpired)
                return default;

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public async Task Write(T value)
        {
            var json = JsonSerializer.Serialize(value);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
} 