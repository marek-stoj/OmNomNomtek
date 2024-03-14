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

    [SerializeField]
    private float _seekRequestFrequencyInSeconds = 0.5f;

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
        }
        else if (Input.GetMouseButtonUp(0))
        {
          _interactableThingyBeingDragged.StopDragging();
          _interactableThingyBeingDragged = null;
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

    public bool IsDragging => _interactableThingyBeingDragged != null;
  }
}
