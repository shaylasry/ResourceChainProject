namespace ResourceChainProject.Interfaces
{
    public interface IReadableStorage<T>
    {
        bool HasValue { get; }
        bool IsExpired { get; }
        Task<T?> Read();
    }
} 