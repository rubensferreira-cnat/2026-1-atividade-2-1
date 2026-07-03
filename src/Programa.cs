using System;

namespace SoConcorrencia;

internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        var command = args[0].ToLowerInvariant();
        var parameters = args.Length > 1 ? args[1..] : Array.Empty<string>();

        var app = new ConcurrencyApp();
        switch (command)
        {
            case "producer_consumer":
            case "produtor_consumidor":
                app.RunProducerConsumer(parameters);
                break;
            case "readers_writers":
            case "leitores_escritores":
                app.RunReadersWriters(parameters);
                break;
            case "dining_philosophers":
            case "jantar_filosofos":
                app.RunDiningPhilosophers(parameters);
                break;
            default:
                Console.WriteLine($"Comando desconhecido: {command}");
                PrintUsage();
                break;
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Uso:");
        Console.WriteLine("  so_concorrencia producer_consumer <num_producers> <num_consumers> <items_per_producer> <buffer_size>");
        Console.WriteLine("  so_concorrencia readers_writers <num_readers> <num_writers> <cycles>");
        Console.WriteLine("  so_concorrencia dining_philosophers <num_philosophers> <cycles>");
    }
}
