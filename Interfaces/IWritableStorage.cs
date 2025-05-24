namespace ResourceChainProject.Interfaces
{
    public interface IWritableStorage<T> : IReadableStorage<T>
    {
        Task Write(T value);
    }
} 