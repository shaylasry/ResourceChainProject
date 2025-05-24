using System;
using System.Threading.Tasks;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Storages
{
    public class MemoryStorage<T> : IReadableStorage<T>, IWritableStorage<T>
    {
        private T? _value;
        private DateTime? _expirationTime;
        private readonly TimeSpan _expiration;

        public MemoryStorage(TimeSpan expiration)
        {
            _expiration = expiration;
        }

        public bool HasValue => _value is not null;

        public bool IsExpired => !_expirationTime.HasValue || DateTime.UtcNow > _expirationTime.Value;

        public Task<T?> Read()
        {
            if (IsExpired) return Task.FromResult<T?>(default);
            return Task.FromResult(_value);
        }

        public Task Write(T value)
        {
            _value = value;
            _expirationTime = DateTime.UtcNow.Add(_expiration);
            return Task.CompletedTask;
        }
    }
} 