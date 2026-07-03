using System;
using System.Threading;

namespace SoConcorrencia;

internal sealed class ReadersWriters
{
    private readonly int _cycles;
    private int _sharedData;
    private int _readersCount;
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly SemaphoreSlim _writer = new(1, 1);

    public ReadersWriters(int readers, int writers, int cycles)
    {
        Readers = readers;
        Writers = writers;
        _cycles = cycles;
    }

    public int Readers { get; }
    public int Writers { get; }

    public void Execute()
    {
        var threads = new Thread[Readers + Writers];
        var ids = new int[Readers + Writers];

        for (var i = 0; i < Readers; i++)
        {
            ids[i] = i + 1;
            threads[i] = new Thread(Reader) { Name = $"Reader-{ids[i]}" };
            threads[i].Start(ids[i]);
        }

        for (var i = 0; i < Writers; i++)
        {
            ids[Readers + i] = i + 1;
            threads[Readers + i] = new Thread(Writer) { Name = $"Writer-{ids[Readers + i]}" };
            threads[Readers + i].Start(ids[Readers + i]);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Execução concluída.");
    }

    private void Reader(object? state)
    {
        var id = (int)state!;
        var random = new Random(id * Environment.TickCount);

        for (var i = 0; i < _cycles; i++)
        {
            _mutex.Wait();
            _readersCount++;
            if (_readersCount == 1)
            {
                _writer.Wait();
            }
            _mutex.Release();

            Console.WriteLine($"[L{id}] lendo valor {_sharedData}");
            Thread.Sleep(80 + random.Next(80));

            _mutex.Wait();
            _readersCount--;
            if (_readersCount == 0)
            {
                _writer.Release();
            }
            _mutex.Release();

            Thread.Sleep(100 + random.Next(80));
        }
    }

    private void Writer(object? state)
    {
        var id = (int)state!;
        var random = new Random(id * Environment.TickCount ^ 54321);

        for (var i = 0; i < _cycles; i++)
        {
            _writer.Wait();
            _sharedData += id;
            Console.WriteLine($"[E{id}] escreveu valor {_sharedData}");
            Thread.Sleep(120 + random.Next(120));
            _writer.Release();
            Thread.Sleep(130 + random.Next(100));
        }
    }
}
