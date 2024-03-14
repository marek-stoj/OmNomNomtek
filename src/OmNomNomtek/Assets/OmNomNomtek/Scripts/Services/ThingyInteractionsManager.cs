using ImmSoft.UnityToolbelt.Utils;
using OmNomNomtek.Domain;
using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.Services
{
  public class ThingyInteractionsManager : MonoBehaviour
  {
    [SerializeField]
    private GameObject _thingiesParent;

    private InteractableThingy _interactableThingyBeingDragged;

    public void SpawnThingy(GameObject thingyPrefab)
    {
      GameObject thingyGameObject =
        Instantiate(thingyPrefab, _thingiesParent.transform);

      InteractableThingy interactableThingy =
        thingyGameObject.GetComponentSafe<InteractableThingy>();

      interactableThingy.StartDragging();

      this.RunAtEndOfFrame(() =>
      {
        _interactableThingyBeingDragged = interactableThingy;
      });
    }

    private void Update()
    {
      if (_interactableThingyBeingDragged == null)
      {
        return;
      }

      if (Input.GetMouseButtonUp(0))
      {
        _interactableThingyBeingDragged.StopDragging();
        _interactableThingyBeingDragged = null;
      }
    }
  }
}
