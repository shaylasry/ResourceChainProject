namespace ResourceChainProject.Interfaces
{
    public interface IWritableStorage<T>
    {
        Task Write(T value);
    }
} 