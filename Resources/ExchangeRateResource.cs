using System;
using ResourceChainProject.Interfaces;
using ResourceChainProject.Models;
using ResourceChainProject.Storages;
using ResourceChainProject.Nodes;
using ResourceChainProject.Chains;
using DotNetEnv;

public static class ExchangeRateResource
{
    private static readonly IChainResource<ExchangeRatesResponse> _instance;

    private const int MemoryExpirationSeconds = 3600;
    private const int FileExpirationSeconds = 14400;
    private const string JsonFilePath = "exchange_rates.json";

    static ExchangeRateResource()
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

        var node3 = new ResourceNode<ExchangeRatesResponse>(webServiceStorage);
        var node2 = new ResourceNode<ExchangeRatesResponse>(fileSystemStorage) { Next = node3 };
        var node1 = new ResourceNode<ExchangeRatesResponse>(memoryStorage) { Next = node2 };

        _instance = new ChainResource<ExchangeRatesResponse>(node1);
    }

    public static IChainResource<ExchangeRatesResponse> Instance => _instance;
}
