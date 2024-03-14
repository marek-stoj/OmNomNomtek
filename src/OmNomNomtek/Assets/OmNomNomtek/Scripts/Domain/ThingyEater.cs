using OmNomNomtek.Services;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class ThingyEater : MonoBehaviour
  {
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [SerializeField]
    private InteractableThingy _thingyToSeek;

    private ThingyInteractionsManager _thingyInteractionsManager;

    private void FixedUpdate()
    {
      if (_thingyInteractionsManager.IsBeingDragged(this.gameObject))
      {
        return;
      }

      if (_thingyToSeek == null)
      {
        // TODO: 2024-03-14 - Immortal - HI - make sure this is efficient (maybe call it only once in a while?)
        InteractableThingy targetToSeek =
          _thingyInteractionsManager.RequestThingyToSeek(this);

        if (targetToSeek != null)
        {
          StartSeeking(targetToSeek);
        }
      }
      else
      {
        Vector3 direction = _thingyToSeek.transform.position - this.transform.position;

        this.transform.position += direction.normalized * _movementSpeed * Time.fixedDeltaTime;

        // TODO: 2024-03-14 - Immortal - HI - rotation speed; lerp
        this.transform.rotation = Quaternion.LookRotation(direction);
      }
    }

    public void Init(ThingyInteractionsManager thingyInteractionsManager)
    {
      _thingyInteractionsManager = thingyInteractionsManager;
    }

    public void StartSeeking(InteractableThingy thingyToSeek)
    {
      Debug.Log($"Start seeking {thingyToSeek.gameObject.name}!");

      _thingyToSeek = thingyToSeek;
    }

    public void StopSeeking()
    {
      Debug.Log($"Stop seeking!");

      _thingyToSeek = null;
    }
  }
}
