using DotNetEnv;
using ResourceChainProject.Interfaces;
using ResourceChainProject.Models;
using ResourceChainProject.Storages;
using ResourceChainProject.Nodes;
using ResourceChainProject.Chains;

public class ChainResourceManager
{
    private readonly IChainResource<ExchangeRatesResponse> _chain;

    private const int MemoryExpirationSeconds = 10;
    private const int FileExpirationSeconds = 20;
    private const string JsonFilePath = "exchange_rates.json";

    public ChainResourceManager()
    {
        Env.Load();

        var apiKey = Environment.GetEnvironmentVariable("API_KEY")
            ?? throw new InvalidOperationException("API_KEY not found");

        var url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";

        var webServiceStorage = new WebServiceStorage<ExchangeRatesResponse>(url);
        var fileSystemStorage = new FileSystemStorage<ExchangeRatesResponse>(
            JsonFilePath, TimeSpan.FromSeconds(FileExpirationSeconds));
        var memoryStorage = new MemoryStorage<ExchangeRatesResponse>(
            TimeSpan.FromSeconds(MemoryExpirationSeconds));

        var node3 = new ChainResourceNode<ExchangeRatesResponse>(webServiceStorage);
        var node2 = new ChainResourceNode<ExchangeRatesResponse>(fileSystemStorage) { Next = node3 };
        var node1 = new ChainResourceNode<ExchangeRatesResponse>(memoryStorage) { Next = node2 };

        _chain = new ChainResource<ExchangeRatesResponse>(node1);
    }

    public Task<ExchangeRatesResponse?> GetRatesAsync() => _chain.GetValue();
}
