
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("⚙️ Initializing ExchangeRateResource");

        await RunTest("🧪 [Run #1] Expect: WebService → FileSystem → Memory");

        await Task.Delay(TimeSpan.FromSeconds(5));
        await RunTest("🧪 [Run #2] Expect: Memory HIT");

        await Task.Delay(TimeSpan.FromSeconds(6)); // 11 total
        await RunTest("🧪 [Run #3] Expect: FileSystem HIT, then Memory re-write");

        await Task.Delay(TimeSpan.FromSeconds(10)); // 21 total
        await RunTest("🧪 [Run #4] Expect: WebService HIT again (all expired)");

        try
        {
            File.Delete("exchange_rates.json");
            Console.WriteLine("🗑️ Deleted exchange_rates.json to simulate missing FileStorage");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Failed to delete JSON: {ex.Message}");
        }

        await RunTest("🧪 [Run #5] File deleted – expect: Memory expired → skip file → go to WebService");
    }

    static async Task RunTest(string label)
    {
        Console.WriteLine("\n-----------------------------------");
        Console.WriteLine(label);
        Console.WriteLine("-----------------------------------");

        try
        {
            var rates = await ChainResourceHolder.ExchangeRates.GetRatesAsync();

            Console.WriteLine($"✅ Base: {rates.Base}, Timestamp: {DateTimeOffset.FromUnixTimeSeconds(rates.Timestamp)}");

            foreach (var curr in new[] { "EUR", "GBP", "JPY" })
            {
                if (rates.Rates.TryGetValue(curr, out var value))
                    Console.WriteLine($"{curr}: {value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception: {ex.Message}");
        }
    }
}
