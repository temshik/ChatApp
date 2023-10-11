using System.Collections.Concurrent;

namespace ChatApp.Bll.Extensions
{
    public static class ForEachExtension
    {
        //taken from https://scatteredcode.net/parallel-foreach-async-in-c/
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        await body(partition.Current).ContinueWith(t =>
                        {
                            if (t.IsFaulted && t.Exception != null)
                            {
                                Console.WriteLine(t.Exception.ToString());
                            }
                        })
                            .ConfigureAwait(false);
                    }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(dop)
                    .AsParallel()
                    .Select(AwaitPartition));
        }
    }
}
