using System;
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
    private Thingy _thingyToSeek;

    private bool _isInitialized;
    private ThingiesManager _thingiesManager;

    private void Start()
    {
      EnsureIsInitialized();
    }

    private void FixedUpdate()
    {
      if (!_isInitialized)
      {
        return;
      }

      if (_thingiesManager.IsBeingCarried(this.gameObject))
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
        Vector3 directionTowardsThingy =
          _thingyToSeek.transform.position - this.transform.position;

        Vector3 translationVector =
          directionTowardsThingy.normalized * _movementSpeed * Time.fixedDeltaTime;

        // here we're constraining the movement to the XZ plane
        this.transform.position +=
          new Vector3(
            translationVector.x,
            0.0f,
            translationVector.z);

        this.transform.rotation = Quaternion.Lerp(
          this.transform.rotation,
          Quaternion.LookRotation(directionTowardsThingy),
          _rotationSpeed * Time.fixedDeltaTime
        );
      }
    }

    public void Init(ThingiesManager thingiesManager)
    {
      _thingiesManager = thingiesManager;
    }

    public void StartRequestingForThingyToSeek()
    {
      EnsureIsInitialized();

      this.RunEverySeconds(
        _thingiesManager.SeekRequestFrequencyInSeconds,
         KeepRequestingThingiesToSeek
      );
    }

    public void StartSeeking(Thingy thingyToSeek)
    {
      EnsureIsInitialized();

      _thingyToSeek = thingyToSeek;
    }

    public void StopSeeking()
    {
      EnsureIsInitialized();

      _thingyToSeek = null;
    }

    private void EnsureIsInitialized()
    {
      if (!_isInitialized)
      {
        string errorMessage = $"ThingyEater '{this.name}' is not initialized. Please call {nameof(Init)} first.";

        throw new InvalidOperationException(errorMessage);
      }
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (_thingyToSeek == null || _thingiesManager.IsBeingCarried(this.gameObject))
      {
        return;
      }

      if (collision.gameObject == _thingyToSeek.gameObject)
      {
        // NOTE: could be an event; this is simpler
        _thingiesManager.EatThingy(this, _thingyToSeek);

        StopSeeking();
      }
    }

    private void KeepRequestingThingiesToSeek()
    {
      if (_thingyToSeek != null)
      {
        return;
      }

      if (_thingiesManager.IsBeingCarried(this.gameObject))
      {
        return;
      }

      Thingy targetToSeek =
        _thingiesManager.RequestThingyToSeek(this);

      if (targetToSeek != null)
      {
        StartSeeking(targetToSeek);
      }
    }
  }
}
