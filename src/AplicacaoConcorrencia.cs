using System;
using System.Collections.Generic;
using System.Threading;

namespace SoConcorrencia;

internal sealed class ConcurrencyApp
{
    public void RunProducerConsumer(string[] args)
    {
        if (args.Length != 4 || !int.TryParse(args[0], out var producers) || !int.TryParse(args[1], out var consumers) ||
            !int.TryParse(args[2], out var itemsPerProducer) || !int.TryParse(args[3], out var bufferSize))
        {
            Console.WriteLine("Uso: producer_consumer num_producers num_consumers items_per_producer buffer_size");
            return;
        }

        if (producers <= 0 || consumers <= 0 || itemsPerProducer <= 0 || bufferSize <= 0)
        {
            Console.WriteLine("Todos os parâmetros devem ser inteiros positivos.");
            return;
        }

        var problem = new ProducerConsumer(producers, consumers, itemsPerProducer, bufferSize);
        problem.Execute();
    }

    public void RunReadersWriters(string[] args)
    {
        if (args.Length != 3 || !int.TryParse(args[0], out var readers) || !int.TryParse(args[1], out var writers) ||
            !int.TryParse(args[2], out var cycles))
        {
            Console.WriteLine("Uso: readers_writers num_readers num_writers cycles");
            return;
        }

        if (readers <= 0 || writers <= 0 || cycles <= 0)
        {
            Console.WriteLine("Todos os parâmetros devem ser inteiros positivos.");
            return;
        }

        var problem = new ReadersWriters(readers, writers, cycles);
        problem.Execute();
    }

    public void RunDiningPhilosophers(string[] args)
    {
        if (args.Length != 2 || !int.TryParse(args[0], out var philosophers) || !int.TryParse(args[1], out var cycles))
        {
            Console.WriteLine("Uso: dining_philosophers num_philosophers cycles");
            return;
        }

        if (philosophers <= 1 || cycles <= 0)
        {
            Console.WriteLine("num_philosophers deve ser maior que 1 e cycles deve ser positivo.");
            return;
        }

        var problem = new DiningPhilosophers(philosophers, cycles);
        problem.Execute();
    }
}
