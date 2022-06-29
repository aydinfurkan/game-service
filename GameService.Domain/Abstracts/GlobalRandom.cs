using System;
using System.Threading;

namespace GameService.Domain.Abstracts;

public static class GlobalRandom
{
    private static int _seed = Environment.TickCount;

    private static readonly ThreadLocal<Random> Random =
        new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

    public static Random Rand()
    {
        return Random.Value;
    }
        
}