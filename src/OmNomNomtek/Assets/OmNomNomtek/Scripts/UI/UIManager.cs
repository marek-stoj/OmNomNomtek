using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OmNomNomtek.Config;
using OmNomNomtek.Domain;
using OmNomNomtek.Services;
using OmNomNomtek.Utils;
using TMPro;
using UnityEngine;

namespace OmNomNomtek.UI
{
  public class UIManager : MonoBehaviour
  {
    private const float _SidePanelCloseDurationInSeconds = 0.2f;

    [SerializeField]
    private ThingyInteractionsManager _thingyInteractionsManager;

    [SerializeField]
    private TMP_InputField _listFilterInputField;

    [SerializeField]
    private GameObject _scrollView;

    [SerializeField]
    private GameObject _listItemPrefab;

    private void OnEnable()
    {
      _listFilterInputField.onValueChanged.AddListener(OnListFilterInputValueChanged);

      _thingyInteractionsManager.ThingySpawned += OnThingySpawned;
      _thingyInteractionsManager.ThingySpawnCancelled += OnThingySpawnCancelled;
      _thingyInteractionsManager.ThingyPlaced += OnThingyPlaced;
    }

    private void OnDisable()
    {
      _listFilterInputField.onValueChanged.RemoveListener(OnListFilterInputValueChanged);

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
      GameObject scrollViewContent = GetScrollViewContent();

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

    private void OnListFilterInputValueChanged(string filter)
    {
      GameObject scrollViewContent = GetScrollViewContent();

      bool filterIsEmpty = string.IsNullOrEmpty(filter);

      bool ifMatchesFilter(ThingyListItem tli) =>
           false
        || filterIsEmpty
        || tli.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;

      scrollViewContent.GetComponentsInChildren<ThingyListItem>(includeInactive: true)
        .ForEach(tli => tli.gameObject.SetActive(ifMatchesFilter(tli)));
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

    private GameObject GetScrollViewContent()
    {
      var scrollViewContent = _scrollView.FindChildByNameRecursive("Content");

      CommonUtils.EnsureNotNull(scrollViewContent, nameof(scrollViewContent));

      return scrollViewContent;
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
