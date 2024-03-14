using System;
using System.Collections;
using UnityEngine;

namespace ImmSoft.UnityToolbelt.Utils
{
  public static class MonoBehaviourExtensions
  {
    private class EndOfFrameAction : IEnumerable
    {
      private readonly Action _action;

      public EndOfFrameAction(Action action)
      {
        _action = action;
      }

      public IEnumerator GetEnumerator()
      {
        yield return new WaitForEndOfFrame();

        _action();
      }
    }

    private class DelayedAction : IEnumerable
    {
      private readonly Action _action;
      private readonly float _delayInSeconds;

      public DelayedAction(Action action, float delayInSeconds)
      {
        _action = action;
        _delayInSeconds = delayInSeconds;
      }

      public IEnumerator GetEnumerator()
      {
        yield return new WaitForSeconds(_delayInSeconds);

        _action();
      }
    }

    private class EverySecondsAction : IEnumerable
    {
      private readonly Action _action;
      private readonly float _seconds;

      public EverySecondsAction(Action action, float seconds)
      {
        _action = action;
        _seconds = seconds;
      }

      public IEnumerator GetEnumerator()
      {
        while (true)
        {
          _action();

          yield return new WaitForSeconds(_seconds);
        }
      }
    }

    public static void RunAtEndOfFrame(this MonoBehaviour monoBehaviour, Action action)
    {
      monoBehaviour.StartCoroutine(
        new EndOfFrameAction(action)
          .GetEnumerator()
      );
    }

    public static void RunDelayed(this MonoBehaviour monoBehaviour, float delayInSeconds, Action action)
    {
      monoBehaviour.StartCoroutine(
        new DelayedAction(action, delayInSeconds)
          .GetEnumerator()
      );
    }

    public static void RunEverySeconds(this MonoBehaviour monoBehaviour, float seconds, Action action)
    {
      monoBehaviour.StartCoroutine(
        new EverySecondsAction(action, seconds)
          .GetEnumerator()
      );
    }
  }
}
