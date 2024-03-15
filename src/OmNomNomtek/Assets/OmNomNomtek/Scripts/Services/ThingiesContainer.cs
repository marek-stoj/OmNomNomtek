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
  public class ThingiesContainer : MonoBehaviour
  {
    // TODO: 2024-03-15 - Immortal - HI - should use EventHandler<EventArgs> here
    public event Action<Thingy> ThingySpawned;
    public event Action ThingySpawnCancelled;
    public event Action<Thingy> ThingyPlaced;

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

    // TODO: 2024-03-15 - Immortal - HI - could use a HashSet/Dictionary for better performance
    private List<Thingy> _thingies;

    private Thingy _thingyBeingCarried;

    private void Awake()
    {
      _thingies = new List<Thingy>();
    }

    private void Update()
    {
      if (_thingyBeingCarried != null)
      {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
          CancelThingySpawn();
        }
        else if (Input.GetMouseButtonUp(0))
        {
          PlaceTheThingy();
        }
      }
    }

    public void SpawnThingy(GameObject thingyPrefab)
    {
      GameObject thingyObject =
        Instantiate(thingyPrefab, _thingiesParent.transform);

      Thingy thingy =
        thingyObject.GetComponentSafe<Thingy>();

      thingy.Init(this);

      _thingies.Add(thingy);

      // we could also be an Eater, so let's check if we are
      ThingyEater thingyEater =
        thingyObject.GetComponent<ThingyEater>();

      if (thingyEater != null)
      {
        thingyEater.Init(this);
      }

      thingy.StartCarrying();

      this.RunAtEndOfFrame(() =>
      {
        _thingyBeingCarried = thingy;

        if (thingyEater != null)
        {
          thingyEater.StartRequestingForThingyToSeek();
        }

        ThingySpawned?.Invoke(thingy);
      });
    }

    public bool IsBeingCarried(GameObject thingy)
    {
      return true
        && _thingyBeingCarried != null
        && _thingyBeingCarried.gameObject == thingy;
    }

    /// <remarks>
    /// Don't call in an Update(). It's not efficient. Every once in a while is fine.
    /// </remarks>
    public Thingy RequestThingyToSeek(ThingyEater thingyEater)
    {
      // filter out the thingies that are destroyed, not targetable, being carried or the thingyEater itself
      IEnumerable<Thingy> potentialTargets =
        _thingies.Where(
          t => true
            && !t.IsDestroyed()
            && t.IsTargetable
            && !t.IsBeingCarried
            && t.gameObject != thingyEater.gameObject
        );

      // sort and choose the potential targets by distance to the thingyEater
      Thingy thingyToSeek =
        potentialTargets.OrderBy(
          t => Vector3.Distance(thingyEater.transform.position, t.transform.position)
        ).FirstOrDefault();

      return thingyToSeek;
    }

    public bool EatThingy(ThingyEater thingyEater, Thingy thingyToSeek)
    {
      // NOTE: just for sanity
      if (_thingyBeingCarried == thingyToSeek)
      {
        _thingyBeingCarried = null;
      }

      // NOTE: this too
      if (thingyToSeek.IsBeingCarried)
      {
        thingyToSeek.StopCarrying();
      }

      // TODO: 2024-03-15 - Immortal - HI - could use a HashSet/Dictionary for better performance
      _thingies.Remove(thingyToSeek);

      Destroy(thingyToSeek.gameObject);

      return true;
    }

    private void PlaceTheThingy()
    {
      Thingy currentThingy = _thingyBeingCarried;

      currentThingy.StopCarrying();

      _thingyBeingCarried = null;

      ThingyPlaced?.Invoke(currentThingy);
    }

    private void CancelThingySpawn()
    {
      _thingyBeingCarried.StopCarrying();
      _thingies.Remove(_thingyBeingCarried);

      Destroy(_thingyBeingCarried.gameObject);

      _thingyBeingCarried = null;

      ThingySpawnCancelled?.Invoke();
    }

    public GameObject Floor => _floor;

    public float DefaultPlacementDepth => _defaultPlacementDepth;

    public float SeekRequestFrequencyInSeconds => _seekRequestFrequencyInSeconds;

    public float PlacementAboveFloorOffsetMultiplier => _placementAboveFloorOffsetMultiplier;

    public bool IsCarrying => _thingyBeingCarried != null;
  }
}
