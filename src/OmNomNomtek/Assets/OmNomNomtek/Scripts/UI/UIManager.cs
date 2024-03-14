using System.Collections.Generic;
using System.Linq;
using OmNomNomtek.Config;
using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.UI
{
  public class UIManager : MonoBehaviour
  {
    [SerializeField]
    private GameObject _scrollView;

    [SerializeField]
    private GameObject _listItemPrefab;

    private void Update()
    {
      // do nothing
    }

    public void OnListItemClicked(GameObject p)
    {
      Debug.Log($"Item clicked! p = {p}");
    }

    public void BindThingyList(List<ThingyListConfig.ThingyItemConfig> items)
    {
      GameObject scrollViewContent =
        _scrollView.FindChildByNameRecursive("Content");

      CommonUtils.EnsureNotNull(scrollViewContent, nameof(scrollViewContent));

      scrollViewContent
        .GetChildren()
        .ToList()
        .ForEach(Destroy);

      foreach (ThingyListConfig.ThingyItemConfig itemConfig in items)
      {
        Debug.Log($"Title: {itemConfig.Title}, Thumbnail: {itemConfig.Thumbnail}, Prefab: {itemConfig.Prefab}");

        GameObject listItem = Instantiate(_listItemPrefab, scrollViewContent.transform);

        listItem.GetComponentSafe<ThingyListItem>()
          .Bind(itemConfig);
      }
    }
  }
}
