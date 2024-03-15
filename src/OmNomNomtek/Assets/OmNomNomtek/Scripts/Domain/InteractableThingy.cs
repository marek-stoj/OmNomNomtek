using OmNomNomtek.Services;
using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class InteractableThingy : MonoBehaviour
  {
    [SerializeField]
    private bool _isTargetable;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private bool _initialIsKinematic;

    private bool _isBeingDragged;

    private ThingiesManager _thingiesManager;

    private void Awake()
    {
      _collider = this.GetComponentInChildrenSafe<Collider>();

      _rigidbody = this.GetComponentInChildrenSafe<Rigidbody>();
      _initialIsKinematic = _rigidbody.isKinematic;
    }

    private void Start()
    {
      UpdatePosition();
    }

    private void Update()
    {
      if (_isBeingDragged)
      {
        UpdatePosition();
      }
    }

    public void Init(ThingiesManager thingiesManager)
    {
      _thingiesManager = thingiesManager;
    }

    public void StartDragging()
    {
      _rigidbody.isKinematic = true;
      _isBeingDragged = true;

      UpdatePosition();
    }

    public void StopDragging()
    {
      _rigidbody.isKinematic = _initialIsKinematic;
      _isBeingDragged = false;
    }

    private void UpdatePosition()
    {
      Vector3? newPosition = null;

      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(mouseRay, out RaycastHit rayHit))
      {
        if (rayHit.collider.gameObject == _thingiesManager.Floor)
        {
          newPosition = rayHit.point;
        }
      }

      if (!newPosition.HasValue)
      {
        // TODO: 2024-03-14 - Immortal - HI - we could project thingy's position to the floor
        newPosition =
          Camera.main.ScreenToWorldPoint(
            new Vector3(
              Input.mousePosition.x,
              Input.mousePosition.y,
              _thingiesManager.DefaultPlacementDepth
            )
          );
      }

      if (newPosition.HasValue)
      {
        // TODO: 2024-03-14 - Immortal - HI - tell, don't ask
        newPosition +=
          _thingiesManager.Floor.transform.up * _thingiesManager.PlacementAboveFloorOffsetMultiplier * _collider.bounds.extents.y;

        this.transform.position = newPosition.Value;
      }
    }

    public bool IsTargetable => _isTargetable;

    public bool IsBeingDragged => _isBeingDragged;
  }
}
