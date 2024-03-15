using UnityEngine;

namespace OmNomNomtek.Utils
{
  public class DestroyAfter : MonoBehaviour
  {
    [SerializeField]
    private float _seconds = 1.0f;

    private void Start()
    {
      Destroy(this.gameObject, _seconds);
    }
  }
}
