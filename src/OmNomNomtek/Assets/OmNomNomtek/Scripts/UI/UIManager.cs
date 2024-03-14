using UnityEngine;

namespace OmNomNomtek.UI
{
  public class UIManager : MonoBehaviour
  {
    private void Update()
    {
      // do nothing
    }

    public void OnListItemClicked(GameObject p)
    {
      Debug.Log($"Item clicked! p = {p}");
    }
  }
}
