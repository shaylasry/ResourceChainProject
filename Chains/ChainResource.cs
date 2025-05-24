using System;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Chains
{
    public class ChainResource<T> : IChainResource<T>
    {
        private readonly IResourceNode<T> _head;

        public ChainResource(IResourceNode<T> head)
        {
            _head = head;
        }

        public async Task<T?> GetValue()
        {
            return await _head.GetValue() ?? throw new InvalidOperationException("No value available in the resource chain");
        }
    }
} 