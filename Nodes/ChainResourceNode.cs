using System.Threading.Tasks;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Nodes
{
    public class ChainResourceNode<T> : IResourceNode<T>
    {
        public IReadableStorage<T> Storage { get; }
        public IResourceNode<T>? Next { get; set; }

        public ChainResourceNode(IReadableStorage<T> storage)
        {
            Storage = storage;
        }

    public async Task<T?> GetValue() {
        #if DEBUG
            Console.WriteLine($"[DEBUG] Visiting node: {Storage.GetType().Name}");
        #endif

            if (!Storage.IsExpired)
            {
        #if DEBUG
                Console.WriteLine($"[DEBUG] Value found in: {Storage.GetType().Name}");
        #endif
                return await Storage.Read();
            }

            if (Next != null)
            {
                var value = await Next.GetValue();

                if (value != null && Storage is IWritableStorage<T> writable)
                {
        #if DEBUG
                    Console.WriteLine($"[DEBUG] Writing back to: {Storage.GetType().Name}");
        #endif
                    await writable.Write(value);
                }

                return value;
            }

        #if DEBUG
            Console.WriteLine($"[DEBUG] End of chain at: {Storage.GetType().Name}");
        #endif
            return default;
        }

    }
} 