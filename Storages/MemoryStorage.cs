using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Storages
{
    public class MemoryStorage<T> : IWritableStorage<T>
    {
        private T? _value;
        private DateTime? _lastUpdated;
        private readonly TimeSpan _expirationInterval;

        private readonly ReaderWriterLockSlim _lock = new();

        public MemoryStorage(TimeSpan expirationInterval)
        {
            _expirationInterval = expirationInterval;
        }

        public bool HasValue
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _value is not null;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public bool IsExpired
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _value is null
                        || !_lastUpdated.HasValue
                        || DateTime.UtcNow > _lastUpdated.Value.Add(_expirationInterval);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public Task<T?> Read()
        {
            _lock.EnterReadLock();
            try
            {
                if (_value is null || !_lastUpdated.HasValue || DateTime.UtcNow > _lastUpdated.Value.Add(_expirationInterval))
                    return Task.FromResult<T?>(default);

                return Task.FromResult(_value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        
        public Task Write(T value)
        {
            var timestamp = DateTime.UtcNow;

            _lock.EnterWriteLock();
            try
            {
                if (_lastUpdated.HasValue && timestamp <= _lastUpdated.Value)
                {
                    return Task.CompletedTask;
                }

                _value = value;
                _lastUpdated = timestamp;
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return Task.CompletedTask;
        }
    }
}
