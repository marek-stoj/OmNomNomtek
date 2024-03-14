using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek
{
  public class InteractableThingy : MonoBehaviour
  {
    private Rigidbody _rigidbody;

    private bool _isBeingDragged;

    private void Awake()
    {
      _rigidbody = this.GetComponentInChildrenSafe<Rigidbody>();
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

      _rigidbody.isKinematic = false;
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

        Debug.Log($"New position: {newPosition}!");

        this.transform.position = newPosition;
      }
    }
  }
}
