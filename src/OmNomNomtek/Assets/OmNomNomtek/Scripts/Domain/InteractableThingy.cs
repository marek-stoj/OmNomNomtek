using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class InteractableThingy : MonoBehaviour
  {
    [SerializeField]
    private bool _isTargetable;

    private Rigidbody _rigidbody;
    private bool _initialIsKinematic;

    private bool _isBeingDragged;

    private void Awake()
    {
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

    public void StartDragging()
    {
      Debug.Log($"Start dragging {gameObject.name}!");

      _rigidbody.isKinematic = true;
      _isBeingDragged = true;

      UpdatePosition();
    }

    public void StopDragging()
    {
      Debug.Log($"Stop dragging {gameObject.name}!");

      _rigidbody.isKinematic = _initialIsKinematic;
      _isBeingDragged = false;
    }

    private void UpdatePosition()
    {
      // TODO: 2024-03-14 - Immortal - HI - snapping
      // TODO: 2024-03-14 - Immortal - HI - depth
      Vector3 newPosition =
        Camera.main.ScreenToWorldPoint(
          new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            3.0f
          )
        );

      this.transform.position = newPosition;
    }

    public bool IsTargetable => _isTargetable;
  }
}
