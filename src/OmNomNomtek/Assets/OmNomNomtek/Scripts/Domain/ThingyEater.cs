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
    private ThingiesContainer _thingiesContainer;

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

      if (_thingiesContainer.IsBeingCarried(this.gameObject))
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

        // lerp the rotation towards the thingy using the configured rotation speed
        this.transform.rotation = Quaternion.Lerp(
          this.transform.rotation,
          Quaternion.LookRotation(directionTowardsThingy),
          _rotationSpeed * Time.fixedDeltaTime
        );
      }
    }

    public void Init(ThingiesContainer thingiesContainer)
    {
      _thingiesContainer = thingiesContainer;

      _isInitialized = true;
    }

    public void StartRequestingForThingyToSeek()
    {
      EnsureIsInitialized();

      this.RunEverySeconds(
        _thingiesContainer.SeekRequestFrequencyInSeconds,
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
      // check if we're seeking any thingy and if we're not being carried
      if (_thingyToSeek == null || _thingiesContainer.IsBeingCarried(this.gameObject))
      {
        return;
      }

      if (collision.gameObject == _thingyToSeek.gameObject)
      {
        // TODO: 2024-03-15 - Immortal - HI - should be an event; this is simpler for now
        if (_thingiesContainer.EatThingy(this, _thingyToSeek))
        {
          StopSeeking();
        }
      }
    }

    /// <summary>
    /// This method is called every few seconds to keep requesting for a thingy to seek from the Thingies Manager.
    /// </summary>
    private void KeepRequestingThingiesToSeek()
    {
      if (_thingyToSeek != null)
      {
        return;
      }

      if (_thingiesContainer.IsBeingCarried(this.gameObject))
      {
        return;
      }

      Thingy targetToSeek =
        _thingiesContainer.RequestThingyToSeek(this);

      if (targetToSeek != null)
      {
        StartSeeking(targetToSeek);
      }
    }
  }
}
