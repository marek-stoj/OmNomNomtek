using System;
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

    [SerializeField]
    private Button _button;

    public void Bind(ThingyListConfig.ThingyItemConfig itemConfig)
    {
      _title.text = itemConfig.Title;
      _thumbnail.sprite = itemConfig.Thumbnail;
      _prefab = itemConfig.Prefab;

      _button.onClick.AddListener(() => Clicked?.Invoke(this));
    }

    public string Title => _title.text ?? "";

    public GameObject Prefab => _prefab;

    // NOTE: could use event and EventArgs here
    public event Action<ThingyListItem> Clicked;
  }
}
