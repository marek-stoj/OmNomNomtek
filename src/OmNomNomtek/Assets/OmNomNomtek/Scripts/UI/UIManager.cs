using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OmNomNomtek.Config;
using OmNomNomtek.Domain;
using OmNomNomtek.Services;
using OmNomNomtek.Utils;
using UnityEngine;

namespace OmNomNomtek.UI
{
  public class UIManager : MonoBehaviour
  {
    private const float _SidePanelCloseDurationInSeconds = 0.2f;

    [SerializeField]
    private ThingyInteractionsManager _thingyInteractionsManager;

    [SerializeField]
    private GameObject _scrollView;

    [SerializeField]
    private GameObject _listItemPrefab;

    private void OnEnable()
    {
      _thingyInteractionsManager.ThingySpawned += OnThingySpawned;
      _thingyInteractionsManager.ThingySpawnCancelled += OnThingySpawnCancelled;
      _thingyInteractionsManager.ThingyPlaced += OnThingyPlaced;
    }

    private void OnDisable()
    {
      _thingyInteractionsManager.ThingySpawned -= OnThingySpawned;
      _thingyInteractionsManager.ThingySpawnCancelled -= OnThingySpawnCancelled;
      _thingyInteractionsManager.ThingyPlaced -= OnThingyPlaced;
    }

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
        GameObject listItem = Instantiate(_listItemPrefab, scrollViewContent.transform);

        var thingyListItem = listItem.GetComponentSafe<ThingyListItem>();

        thingyListItem.Bind(itemConfig);

        thingyListItem.Clicked += OnListItemClicked;
      }
    }

    private void OnListItemClicked(ThingyListItem thingyListItem)
    {
      if (_thingyInteractionsManager.IsDragging)
      {
        return;
      }

      _thingyInteractionsManager.SpawnThingy(thingyListItem.Prefab);
    }

    private void OnThingySpawned(InteractableThingy thingy)
    {
      ToggleSidePanel(visible: false);
    }

    private void OnThingySpawnCancelled()
    {
      ToggleSidePanel(visible: true);
    }

    private void OnThingyPlaced(InteractableThingy thingy)
    {
      ToggleSidePanel(visible: true);
    }

    private void ToggleSidePanel(bool visible)
    {
      _scrollView.transform.DOMoveX(
        endValue:
          visible
            ? 0
            : -_scrollView.GetComponentSafe<RectTransform>().rect.width,
        duration: _SidePanelCloseDurationInSeconds
      );
    }
  }
}
