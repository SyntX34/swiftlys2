using System.Collections.Concurrent;

namespace SwiftlyS2.Shared.Menu.Components;

internal static class MenuCollectionLoader
{
    private static readonly BlockingCollection<Func<Task>> Jobs = [];
    private static readonly Lock Gate = new();
    private static Thread? thread;

    public static void Enqueue( Func<Task> job )
    {
        lock (Gate)
        {
            thread ??= Start();
        }

        Jobs.Add(job);
    }

    private static Thread Start()
    {
        var worker = new Thread(Run) { IsBackground = true, Name = "SwiftlyS2 Menu Collection Loader" };
        worker.Start();
        return worker;
    }

    private static void Run()
    {
        foreach (var job in Jobs.GetConsumingEnumerable())
        {
            job().GetAwaiter().GetResult();
        }
    }
}
