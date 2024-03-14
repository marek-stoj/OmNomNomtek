using ImmSoft.UnityToolbelt.Utils;
using UnityEngine;

namespace OmNomNomtek
{
  public class EaterPocManager : MonoBehaviour
  {
    [SerializeField]
    private ThingyEater _thingyEater;

    [SerializeField]
    private GameObject _thingy;

    private void Start()
    {
      this.RunDelayed(1.0f, () =>
      {
        _thingyEater.StartFollowing(_thingy);
      });
    }
  }
}
