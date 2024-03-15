using ImmSoft.UnityToolbelt.Utils;
using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class ThingyEaterTester : MonoBehaviour
  {
    [SerializeField]
    private ThingyEater _thingyEater;

    [SerializeField]
    private Thingy _thingy;

    private void Start()
    {
      this.RunDelayed(1.0f, () =>
      {
        _thingyEater.StartSeeking(_thingy);
      });
    }
  }
}
