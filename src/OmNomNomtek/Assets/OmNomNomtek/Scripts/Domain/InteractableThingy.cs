using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class InteractableThingy : MonoBehaviour
  {
    private Rigidbody _rigidbody;
    private bool _initialIsKinematic;

    private bool _isBeingDragged;

    private void Awake()
    {
      _rigidbody = this.GetComponentInChildrenSafe<Rigidbody>();
      _initialIsKinematic = _rigidbody.isKinematic;
    }

    public void StartDragging()
    {
      Debug.Log($"Start dragging {gameObject.name}!");

      _rigidbody.isKinematic = true;
      _isBeingDragged = true;
    }

    public void StopDragging()
    {
      Debug.Log($"Stop dragging {gameObject.name}!");

      _rigidbody.isKinematic = _initialIsKinematic;
      _isBeingDragged = false;
    }

    private void Update()
    {
      if (_isBeingDragged)
      {
        Vector3 newPosition =
          Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
            Input.mousePosition.y,
            3.0f)
          );

        this.transform.position = newPosition;
      }
    }
  }
}
