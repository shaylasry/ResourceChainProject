namespace ResourceChainProject.Interfaces
{
    public interface IChainResource<T>
    {
        Task<T?> GetValue();
    }
} 