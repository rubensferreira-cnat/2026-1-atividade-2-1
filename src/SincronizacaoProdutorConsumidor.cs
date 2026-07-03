using System;
using System.Threading;

namespace SoConcorrencia;

internal sealed class ProducerConsumer
{
    private readonly int[] _buffer;
    private readonly int _bufferSize;
    private readonly int _itemsPerProducer;
    private int _inIndex;
    private int _outIndex;
    private readonly SemaphoreSlim _empty;
    private readonly SemaphoreSlim _full;
    private readonly object _mutex = new();

    public ProducerConsumer(int producers, int consumers, int itemsPerProducer, int bufferSize)
    {
        Producers = producers;
        Consumers = consumers;
        _itemsPerProducer = itemsPerProducer;
        _bufferSize = bufferSize;
        _buffer = new int[bufferSize];
        _empty = new SemaphoreSlim(bufferSize);
        _full = new SemaphoreSlim(0);
    }

    public int Producers { get; }
    public int Consumers { get; }

    public void Execute()
    {
        var threads = new Thread[Producers + Consumers];
        var ids = new int[Producers + Consumers];

        for (var i = 0; i < Producers; i++)
        {
            ids[i] = i + 1;
            threads[i] = new Thread(Producer) { Name = $"Producer-{ids[i]}" };
            threads[i].Start(ids[i]);
        }

        for (var i = 0; i < Consumers; i++)
        {
            ids[Producers + i] = i + 1;
            threads[Producers + i] = new Thread(Consumer) { Name = $"Consumer-{ids[Producers + i]}" };
            threads[Producers + i].Start(ids[Producers + i]);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Execução concluída.");
    }

    private void Producer(object? state)
    {
        var id = (int)state!;
        var random = new Random(id * Environment.TickCount);

        for (var i = 0; i < _itemsPerProducer; i++)
        {
            var item = id * 1000 + i;
            _empty.Wait();
            lock (_mutex)
            {
                _buffer[_inIndex] = item;
                Console.WriteLine($"[P{id}] produziu {item} no slot {_inIndex}");
                _inIndex = (_inIndex + 1) % _bufferSize;
            }

            _full.Release();
            Thread.Sleep(100 + random.Next(100));
        }
    }

    private void Consumer(object? state)
    {
        var id = (int)state!;
        var random = new Random(id * Environment.TickCount ^ 12345);

        for (var i = 0; i < _itemsPerProducer; i++)
        {
            _full.Wait();
            int item;
            lock (_mutex)
            {
                item = _buffer[_outIndex];
                Console.WriteLine($"[C{id}] consumiu {item} do slot {_outIndex}");
                _outIndex = (_outIndex + 1) % _bufferSize;
            }

            _empty.Release();
            Thread.Sleep(120 + random.Next(120));
        }
    }
}
