using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmNomNomtek.Utils
{
  public static class GameObjectExtensions
  {
    public static bool HasComponent<T>(this GameObject gameObject)
      where T : Component
    {
      return gameObject.GetComponent<T>() != null;
    }

    public static GameObject FindParent(this GameObject gameObject)
    {
      if (!gameObject)
      {
        return null;
      }

      Transform transform = gameObject.transform;

      if (!transform)
      {
        return null;
      }

      Transform parentTransform = transform.parent;

      if (!parentTransform)
      {
        return null;
      }

      return parentTransform.gameObject;
    }

    public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
    {
      foreach (Transform childTransform in gameObject.transform)
      {
        yield return childTransform.gameObject;
      }
    }

    public static T GetComponentSafe<T>(this GameObject gameObject)
      where T : Component
    {
      T childComponent = gameObject.GetComponent<T>();

      if (childComponent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return childComponent;
    }

    public static T GetComponentInChildrenSafe<T>(this GameObject gameObject)
      where T : Component
    {
      T descendantComponent = gameObject.GetComponentInChildren<T>();

      if (descendantComponent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return descendantComponent;
    }

    public static T GetComponentInParentSafe<T>(this GameObject gameObject)
      where T : Component
    {
      T componentInParent = gameObject.GetComponentInParent<T>();

      if (componentInParent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return componentInParent;
    }

    public static T FindComponent<T>(this GameObject gameObject)
      where T : Component
    {
      return gameObject.GetComponent<T>();
    }

    public static T FindComponentInChildren<T>(this GameObject gameObject)
      where T : Component
    {
      return gameObject.GetComponentInChildren<T>();
    }

    public static T FindComponentInParent<T>(this GameObject gameObject)
      where T : Component
    {
      return gameObject.GetComponentInParent<T>();
    }

    public static GameObject FindChildByNameRecursive(this GameObject gameObject, string name)
    {
      foreach (Transform childTransform in gameObject.transform)
      {
        GameObject childObject = childTransform.gameObject;

        if (childObject.name == name)
        {
          return childObject;
        }

        GameObject result = childObject.FindChildByNameRecursive(name);

        if (result != null)
        {
          return result;
        }
      }

      return null;
    }

    public static T EnsureComponent<T>(this GameObject gameObject)
      where T : Component
    {
      T component = gameObject.GetComponent<T>();

      if (component == null)
      {
        component = gameObject.AddComponent<T>();
      }

      return component;
    }

    /// <summary>
    /// Set the layer to the given object and the full hierarchy below it.
    /// </summary>
    /// <param name="root">Start point of the traverse</param>
    /// <param name="layer">The layer to apply</param>
    public static void SetLayerRecursively(this GameObject root, int layer)
    {
      if (root == null)
      {
        throw new ArgumentNullException(nameof(root), "Root transform can't be null.");
      }

      foreach (var child in root.transform.EnumerateHierarchy())
      {
        child.gameObject.layer = layer;
      }
    }

    /// <summary>
    /// Enumerates all children in the hierarchy starting at the root object.
    /// </summary>
    /// <remarks>'
    /// From MRTK
    /// </remarks>
    /// <param name="root">Start point of the traversion set</param>
    private static IEnumerable<Transform> EnumerateHierarchy(this Transform root)
    {
      if (root == null) { throw new ArgumentNullException("root"); }
      return root.EnumerateHierarchyCore(new List<Transform>(0));
    }

    /// <summary>
    /// Enumerates all children in the hierarchy starting at the root object except for the branches in ignore.
    /// </summary>
    /// <remarks>'
    /// From MRTK
    /// </remarks>
    /// <param name="root">Start point of the traversion set</param>
    /// <param name="ignore">Transforms and all its children to be ignored</param>
    private static IEnumerable<Transform> EnumerateHierarchyCore(this Transform root, ICollection<Transform> ignore)
    {
      var transformQueue = new Queue<Transform>();
      transformQueue.Enqueue(root);

      while (transformQueue.Count > 0)
      {
        var parentTransform = transformQueue.Dequeue();

        if (!parentTransform || ignore.Contains(parentTransform)) { continue; }

        for (var i = 0; i < parentTransform.childCount; i++)
        {
          transformQueue.Enqueue(parentTransform.GetChild(i));
        }

        yield return parentTransform;
      }
    }
  }
}
