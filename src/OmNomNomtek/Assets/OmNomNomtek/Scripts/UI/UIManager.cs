using System.Collections.Generic;
using System.Linq;
using OmNomNomtek.Config;
using OmNomNomtek.Domain;
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

    [SerializeField]
    private GameObject _thingiesParent;

    private void Update()
    {
      // do nothing
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

        var thingyListItem = listItem.GetComponentSafe<ThingyListItem>();

        thingyListItem.Bind(itemConfig);

        thingyListItem.Clicked += OnListItemClicked;
      }
    }

    private void OnListItemClicked(ThingyListItem thingyListItem)
    {
      GameObject thingyGameObject =
        Instantiate(thingyListItem.Prefab, _thingiesParent.transform);

      InteractableThingy interactableThingy =
        thingyGameObject.GetComponentSafe<InteractableThingy>();

      interactableThingy.StartDragging();
    }
  }
}
