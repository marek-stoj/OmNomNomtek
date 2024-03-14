using System;
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
      GameObject thingyGameObject =
        Instantiate(thingyPrefab, _thingiesParent.transform);

      ThingyEater thingyEater =
        thingyGameObject.GetComponent<ThingyEater>();

      if (thingyEater != null)
      {
        thingyEater.Init(this);
      }

      InteractableThingy interactableThingy =
        thingyGameObject.GetComponentSafe<InteractableThingy>();

      _thingies.Add(interactableThingy);

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
        _thingies.Where(t => t.IsTargetable && t.gameObject != thingyEater.gameObject);

      // TODO: 2024-03-14 - Immortal - HI - find the closest one
      InteractableThingy thingyToSeek =
        potentialTargets.FirstOrDefault();

      return thingyToSeek;
    }

    public bool IsDragging => _interactableThingyBeingDragged != null;
  }
}
