using System;
using System.Threading;

namespace SoConcorrencia;

internal sealed class DiningPhilosophers
{
    private readonly SemaphoreSlim[] _forks;
    private readonly SemaphoreSlim _waiter;
    private readonly int _cycles;
    private readonly int _philosophers;

    public DiningPhilosophers(int philosophers, int cycles)
    {
        _philosophers = philosophers;
        _cycles = cycles;
        _forks = new SemaphoreSlim[philosophers];
        for (var i = 0; i < philosophers; i++)
        {
            _forks[i] = new SemaphoreSlim(1, 1);
        }

        _waiter = new SemaphoreSlim(philosophers - 1, philosophers - 1);
    }

    public void Execute()
    {
        var threads = new Thread[_philosophers];
        var ids = new int[_philosophers];

        for (var i = 0; i < _philosophers; i++)
        {
            ids[i] = i;
            threads[i] = new Thread(Philosopher) { Name = $"Philosopher-{i + 1}" };
            threads[i].Start(ids[i]);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Execução concluída.");
    }

    private void Philosopher(object? state)
    {
        var id = (int)state!;
        var random = new Random(id * Environment.TickCount);

        for (var i = 0; i < _cycles; i++)
        {
            Console.WriteLine($"[F{id + 1}] pensando");
            Thread.Sleep(150 + random.Next(100));

            PickUpForks(id);
            Console.WriteLine($"[F{id + 1}] comendo");
            Thread.Sleep(120 + random.Next(100));
            PutDownForks(id);

            Console.WriteLine($"[F{id + 1}] terminou de comer");
            Thread.Sleep(100 + random.Next(120));
        }
    }

    private void PickUpForks(int id)
    {
        _waiter.Wait();
        _forks[id].Wait();
        _forks[(id + 1) % _philosophers].Wait();
    }

    private void PutDownForks(int id)
    {
        _forks[id].Release();
        _forks[(id + 1) % _philosophers].Release();
        _waiter.Release();
    }
}
