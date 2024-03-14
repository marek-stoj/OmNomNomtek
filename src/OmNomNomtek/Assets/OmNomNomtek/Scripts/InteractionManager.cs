using UnityEngine;

namespace OmNomNomtek
{
  public class InteractionManager : MonoBehaviour
  {
    private InteractableThingy _interactableThingyBeingDragged;

    private void Update()
    {
      if (_interactableThingyBeingDragged == null)
      {
        if (Input.GetMouseButtonUp(0))
        {
          Debug.Log($"InteractionManager.Update: Mouse button down!");

          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

          RaycastHit hit;

          if (Physics.Raycast(ray, out hit))
          {
            Debug.Log($"InteractionManager.Update: Collider hit 1! {hit.collider.gameObject.name}");

            if (hit.collider != null)
            {
              Debug.Log($"InteractionManager.Update: Collider hit 2! {hit.collider.gameObject.name}");

              InteractableThingy interactableThingy =
                hit.collider.gameObject
                  .GetComponentInParent<InteractableThingy>();

              if (interactableThingy != null)
              {
                Debug.Log($"InteractionManager.Update: InteractableThingy found on {hit.collider.gameObject.name}!");

                interactableThingy.StartDragging();
                _interactableThingyBeingDragged = interactableThingy;
              }
              else
              {
                Debug.Log($"InteractionManager.Update: No InteractableThingy found on {hit.collider.gameObject.name}!");
              }
            }
          }
          else
          {
            Debug.Log($"InteractionManager.Update: No collider hit!");
          }
        }
      }
      else
      {
        if (Input.GetMouseButtonUp(0))
        {
          Debug.Log($"InteractionManager.Update: Mouse button down!");

          if (_interactableThingyBeingDragged != null)
          {
            _interactableThingyBeingDragged.StopDragging();
            _interactableThingyBeingDragged = null;
          }
        }
      }
    }
  }
}
