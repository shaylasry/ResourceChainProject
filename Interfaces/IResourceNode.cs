namespace ResourceChainProject.Interfaces
{
    public interface IResourceNode<T>
    {
        IReadableStorage<T> Storage { get; }
        IResourceNode<T>? Next { get; set; }
        Task<T?> GetValue();
    }
} 