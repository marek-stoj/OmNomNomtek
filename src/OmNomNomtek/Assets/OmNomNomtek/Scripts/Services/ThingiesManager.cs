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
          _thingyBeingCarried.StopCarrying();
          _thingies.Remove(_thingyBeingCarried);

          Destroy(_thingyBeingCarried.gameObject);

          _thingyBeingCarried = null;

          ThingySpawnCancelled?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
          Thingy currentThingy = _thingyBeingCarried;

          currentThingy.StopCarrying();

          _thingyBeingCarried = null;

          ThingyPlaced?.Invoke(currentThingy);
        }
      }
    }

    public bool IsBeingCarried(GameObject thingy)
    {
      return true
        && _thingyBeingCarried != null
        && _thingyBeingCarried.gameObject == thingy;
    }

    public void SpawnThingy(GameObject thingyPrefab)
    {
      GameObject thingyObject =
        Instantiate(thingyPrefab, _thingiesParent.transform);

      Thingy thingy =
        thingyObject.GetComponentSafe<Thingy>();

      thingy.Init(this);

      _thingies.Add(thingy);

      ThingyEater thingyEater =
        thingyObject.GetComponent<ThingyEater>();

      if (thingyEater != null)
      {
        thingyEater.Init(this);
      }

      this.RunAtEndOfFrame(() =>
      {
        thingy.StartCarrying();

        _thingyBeingCarried = thingy;

        if (thingyEater != null)
        {
          thingyEater.StartRequestingForThingyToSeek();
        }

        ThingySpawned?.Invoke(thingy);
      });
    }

    /// <remarks>
    /// Don't call in an Update(). It's not efficient. Every once in a while is fine.
    /// </remarks>
    public Thingy RequestThingyToSeek(ThingyEater thingyEater)
    {
      IEnumerable<Thingy> potentialTargets =
        _thingies.Where(
          t => true
            && !t.IsDestroyed()
            && t.IsTargetable
            && !t.IsBeingCarried
            && t.gameObject != thingyEater.gameObject
        );

      Thingy thingyToSeek =
        potentialTargets.OrderBy(
          t => Vector3.Distance(thingyEater.transform.position, t.transform.position)
        ).FirstOrDefault();

      return thingyToSeek;
    }

    public void EatThingy(ThingyEater thingyEater, Thingy thingyToSeek)
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

      // NOTE: could use a HashSet/Dictionary for better performance
      _thingies.Remove(thingyToSeek);

      Destroy(thingyToSeek.gameObject);
    }

    public GameObject Floor => _floor;

    public float DefaultPlacementDepth => _defaultPlacementDepth;

    public float SeekRequestFrequencyInSeconds => _seekRequestFrequencyInSeconds;

    public float PlacementAboveFloorOffsetMultiplier => _placementAboveFloorOffsetMultiplier;

    public bool IsCarrying => _thingyBeingCarried != null;
  }
}
