using System;
using System.Collections.Generic;
using System.Linq;
using ImmSoft.UnityToolbelt.Utils;
using OmNomNomtek.Domain;
using OmNomNomtek.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace OmNomNomtek.Services
{
  // TODO: 2024-03-14 - Immortal - HI - keep track of the thingies that fell out of bounds - remove them from the list and stop seeking them
  public class ThingiesManager : MonoBehaviour
  {
    // NOTE: could use EventArgs
    public event Action<InteractableThingy> ThingySpawned;
    public event Action ThingySpawnCancelled;
    public event Action<InteractableThingy> ThingyPlaced;

    [SerializeField]
    private GameObject _thingiesParent;

    [SerializeField]
    private GameObject _floor;

    [SerializeField]
    private float _defaultPlacementDepth = 3.0f;

    [SerializeField]
    private float _seekRequestFrequencyInSeconds = 0.5f;

    [SerializeField]
    private float _placementAboveFloorOffsetMultiplier = 1.0f;

    private List<InteractableThingy> _thingies;

    private InteractableThingy _interactableThingyBeingDragged;

    private void Awake()
    {
      _thingies = new List<InteractableThingy>();
    }

    private void Update()
    {
      if (_interactableThingyBeingDragged != null)
      {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
          _interactableThingyBeingDragged.StopDragging();
          _thingies.Remove(_interactableThingyBeingDragged);

          Destroy(_interactableThingyBeingDragged.gameObject);

          _interactableThingyBeingDragged = null;

          ThingySpawnCancelled?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
          InteractableThingy currentThingy = _interactableThingyBeingDragged;

          currentThingy.StopDragging();

          _interactableThingyBeingDragged = null;

          ThingyPlaced?.Invoke(currentThingy);
        }
      }
    }

    public bool IsBeingDragged(GameObject thingy)
    {
      return true
        && _interactableThingyBeingDragged != null
        && _interactableThingyBeingDragged.gameObject == thingy;
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

        if (thingyEater != null)
        {
          thingyEater.StartRequestingForThingyToSeek();
        }

        ThingySpawned?.Invoke(interactableThingy);
      });
    }

    /// <remarks>
    /// Don't call in an Update(). It's not efficient. Every once in a while is fine.
    /// </remarks>
    public InteractableThingy RequestThingyToSeek(ThingyEater thingyEater)
    {
      IEnumerable<InteractableThingy> potentialTargets =
        _thingies.Where(
          t => true
            && !t.IsDestroyed()
            && t.IsTargetable
            && !t.IsBeingDragged
            && t.gameObject != thingyEater.gameObject
        );

      InteractableThingy thingyToSeek =
        potentialTargets.OrderBy(
          t => Vector3.Distance(thingyEater.transform.position, t.transform.position)
        ).FirstOrDefault();

      return thingyToSeek;
    }

    public void EatThingy(ThingyEater thingyEater, InteractableThingy thingyToSeek)
    {
      // NOTE: just for sanity
      if (_interactableThingyBeingDragged == thingyToSeek)
      {
        _interactableThingyBeingDragged = null;
      }

      // NOTE: this too
      if (thingyToSeek.IsBeingDragged)
      {
        thingyToSeek.StopDragging();
      }

      // NOTE: could use a HashSet/Dictionary for better performance
      _thingies.Remove(thingyToSeek);

      Destroy(thingyToSeek.gameObject);
    }

    public GameObject Floor => _floor;

    public float DefaultPlacementDepth => _defaultPlacementDepth;

    public float SeekRequestFrequencyInSeconds => _seekRequestFrequencyInSeconds;

    public float PlacementAboveFloorOffsetMultiplier => _placementAboveFloorOffsetMultiplier;

    public bool IsDragging => _interactableThingyBeingDragged != null;
  }
}
