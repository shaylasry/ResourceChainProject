public static class ChainResourceHolder
{
    public static readonly ChainResourceManager ChainResourceManager = 
        new ChainResourceManager(TimeSpan.FromHours(1), TimeSpan.FromHours(4));

    public static readonly ChainResourceManager TestChainResourceManager = 
        new ChainResourceManager(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
}
