using System;

namespace OmNomNomtek.Utils
{
  public static class CommonUtils
  {
    public static Exception CreateComponentNullException(Type componentType)
    {
      return new InvalidOperationException($"Component of type '{componentType.Name}' doesn't exist.");
    }

    public static void EnsureNotNull(object obj, string name)
    {
      if (obj == null)
      {
        throw new Exception($"Object '{name}' should not be null.");
      }
    }
  }
}
