using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private GameObject _floor;

    [SerializeField]
    private float _defaultPlacementDepth = 3.0f;

    private List<InteractableThingy> _thingies;

    private InteractableThingy _interactableThingyBeingDragged;

    private void Awake()
    {
      _thingies = new List<InteractableThingy>();
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

    public bool IsBeingDragged(GameObject thingy)
    {
      return
          _interactableThingyBeingDragged != null
        &&
          _interactableThingyBeingDragged.gameObject == thingy;
    }

    public void SpawnThingy(GameObject thingyPrefab)
    {
      GameObject thingyObject =
        Instantiate(thingyPrefab, _thingiesParent.transform);

      InteractableThingy interactableThingy =
        thingyObject.GetComponentSafe<InteractableThingy>();

      interactableThingy.Init(this);

      _thingies.Add(interactableThingy);

      ThingyEater thingyEater =
        thingyObject.GetComponent<ThingyEater>();

      if (thingyEater != null)
      {
        thingyEater.Init(this);
      }

      this.RunAtEndOfFrame(() =>
      {
        interactableThingy.StartDragging();

        _interactableThingyBeingDragged = interactableThingy;
      });
    }

    // TODO: 2024-03-14 - Immortal - HI - make sure this is efficient (maybe call it only once in a while?)
    public InteractableThingy RequestThingyToSeek(ThingyEater thingyEater)
    {
      IEnumerable<InteractableThingy> potentialTargets =
        _thingies.Where(
          t => true
            && t.IsTargetable
            && !t.IsBeingDragged
            && t.gameObject != thingyEater.gameObject
        );

      // TODO: 2024-03-14 - Immortal - HI - find the closest one
      InteractableThingy thingyToSeek =
        potentialTargets.FirstOrDefault();

      return thingyToSeek;
    }

    public GameObject Floor => _floor;

    public float DefaultPlacementDepth => _defaultPlacementDepth;

    public bool IsDragging => _interactableThingyBeingDragged != null;
  }
}
