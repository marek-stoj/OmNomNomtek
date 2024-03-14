using System;
using System.Collections.Generic;
using System.Linq;

namespace OmNomNomtek.Utils
{
  public static class CollectionExtensions
  {
    private static readonly Random _rand = new Random();

    public static int? FirstIndexOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
      int? result =
        enumerable
          .Select((item, index) => (item, index))
          .Where(t => predicate(t.item))
          .Select(t => (int?)t.index)
          .FirstOrDefault();

      return result;
    }

    public static T PickRandomItem<T>(this IList<T> list)
    {
      return list[_rand.Next(list.Count)];
    }

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
      foreach (T item in collection)
      {
        action(item);
      }
    }
  }
}
