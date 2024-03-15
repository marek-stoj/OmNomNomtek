using System;
using OmNomNomtek.Services;
using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class Thingy : MonoBehaviour
  {
    [SerializeField]
    private bool _isTargetable;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private bool _initialIsKinematic;

    private bool _isBeingCarried;

    private bool _isInitialized;
    private ThingiesContainer _thingiesContainer;

    private void Awake()
    {
      _collider = this.GetComponentInChildrenSafe<Collider>();

      _rigidbody = this.GetComponentInChildrenSafe<Rigidbody>();
      _initialIsKinematic = _rigidbody.isKinematic;
    }

    private void Start()
    {
      EnsureIsInitialized();

      UpdatePosition();
    }

    private void Update()
    {
      if (!_isInitialized)
      {
        return;
      }

      if (_isBeingCarried)
      {
        UpdatePosition();
      }
    }

    public void Init(ThingiesContainer thingiesContainer)
    {
      _thingiesContainer = thingiesContainer;

      _isInitialized = true;
    }

    public void StartCarrying()
    {
      EnsureIsInitialized();

      _rigidbody.isKinematic = true;
      _isBeingCarried = true;

      UpdatePosition();
    }

    public void StopCarrying()
    {
      EnsureIsInitialized();

      _rigidbody.isKinematic = _initialIsKinematic;
      _isBeingCarried = false;
    }

    private void EnsureIsInitialized()
    {
      if (!_isInitialized)
      {
        string errorMessage = $"Thingy '{this.name}' is not initialized. Please call {nameof(Init)} first.";

        throw new InvalidOperationException(errorMessage);
      }
    }

    private void UpdatePosition()
    {
      EnsureIsInitialized();

      Vector3? newPosition = null;

      // first let's see if we are pointing at the floor
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(mouseRay, out RaycastHit rayHit))
      {
        if (rayHit.collider.gameObject == _thingiesContainer.Floor)
        {
          newPosition = rayHit.point;
        }
      }

      // if not pointing at the floor, we'll just use the mouse position projected with a constant depth
      if (!newPosition.HasValue)
      {
        // TODO: 2024-03-14 - Immortal - HI - we could project thingy's position to the floor
        newPosition =
          Camera.main.ScreenToWorldPoint(
            new Vector3(
              Input.mousePosition.x,
              Input.mousePosition.y,
              _thingiesContainer.DefaultPlacementDepth
            )
          );
      }

      if (newPosition.HasValue)
      {
        // TODO: 2024-03-14 - Immortal - HI - tell, don't ask
        newPosition +=
          _thingiesContainer.Floor.transform.up * _thingiesContainer.PlacementAboveFloorOffsetMultiplier * _collider.bounds.extents.y;

        this.transform.position = newPosition.Value;
      }
    }

    public bool IsTargetable => _isTargetable;

    public bool IsBeingCarried => _isBeingCarried;
  }
}
