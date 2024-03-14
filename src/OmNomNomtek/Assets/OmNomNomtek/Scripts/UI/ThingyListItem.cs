using OmNomNomtek.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmNomNomtek.UI
{
  public class ThingyListItem : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField]
    private Image _thumbnail;

    [SerializeField]
    private GameObject _prefab;

    public void Bind(ThingyListConfig.ThingyItemConfig itemConfig)
    {
      _title.text = itemConfig.Title;
      _thumbnail.sprite = itemConfig.Thumbnail;
      _prefab = itemConfig.Prefab;
    }
  }
}
