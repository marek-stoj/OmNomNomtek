using ImmSoft.UnityToolbelt.Utils;
using OmNomNomtek.Services;
using Unity.VisualScripting;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class ThingyEater : MonoBehaviour
  {
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [SerializeField]
    private InteractableThingy _thingyToSeek;

    private ThingyInteractionsManager _thingyInteractionsManager;

    private void FixedUpdate()
    {
      if (_thingyInteractionsManager.IsBeingDragged(this.gameObject))
      {
        return;
      }

      // some other eater could have eaten our target thingy first,
      // so we need to stop seeking if that happens
      if (_thingyToSeek.IsDestroyed())
      {
        StopSeeking();
        return;
      }

      if (_thingyToSeek != null)
      {
        Vector3 direction = _thingyToSeek.transform.position - this.transform.position;

        this.transform.position += direction.normalized * _movementSpeed * Time.fixedDeltaTime;

        this.transform.rotation = Quaternion.Lerp(
          this.transform.rotation,
          Quaternion.LookRotation(direction),
          _rotationSpeed * Time.fixedDeltaTime
        );
      }
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (_thingyToSeek == null)
      {
        return;
      }

      if (collision.gameObject == _thingyToSeek.gameObject)
      {
        // NOTE - could be an event; this is simpler
        _thingyInteractionsManager.EatThingy(this, _thingyToSeek);

        StopSeeking();
      }
    }

    public void Init(ThingyInteractionsManager thingyInteractionsManager)
    {
      _thingyInteractionsManager = thingyInteractionsManager;
    }

    public void StartRequestingForThingyToSeek()
    {
      Debug.Log($"ThingyEater.StartRequestingForThingyToSeek!");

      this.RunEverySeconds(
        _thingyInteractionsManager.SeekRequestFrequencyInSeconds,
         KeepRequestingThingiesToSeek
      );
    }

    public void StartSeeking(InteractableThingy thingyToSeek)
    {
      Debug.Log($"ThingyEater.StartSeeking: {thingyToSeek.gameObject.name}!");

      _thingyToSeek = thingyToSeek;
    }

    public void StopSeeking()
    {
      Debug.Log($"ThingyEater.StopSeeking!");

      _thingyToSeek = null;
    }

    private void KeepRequestingThingiesToSeek()
    {
      if (_thingyToSeek != null)
      {
        return;
      }

      if (_thingyInteractionsManager.IsBeingDragged(this.gameObject))
      {
        return;
      }

      InteractableThingy targetToSeek =
        _thingyInteractionsManager.RequestThingyToSeek(this);

      if (targetToSeek != null)
      {
        StartSeeking(targetToSeek);
      }
    }
  }
}
