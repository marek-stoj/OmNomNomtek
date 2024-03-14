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
    private GameObject _sidePanel;

    [SerializeField]
    private GameObject _scrollViewContent;

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
      _scrollViewContent
        .GetChildren()
        .ToList()
        .ForEach(Destroy);

      foreach (ThingyListConfig.ThingyItemConfig itemConfig in items)
      {
        GameObject listItem = Instantiate(_listItemPrefab, _scrollViewContent.transform);

        var thingyListItem = listItem.GetComponentSafe<ThingyListItem>();

        thingyListItem.Bind(itemConfig);

        thingyListItem.Clicked += OnListItemClicked;
      }
    }

    private void OnListFilterInputValueChanged(string filter)
    {
      bool filterIsEmpty = string.IsNullOrEmpty(filter);

      bool ifMatchesFilter(ThingyListItem tli) =>
           false
        || filterIsEmpty
        || tli.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;

      _scrollViewContent
        .GetComponentsInChildren<ThingyListItem>(includeInactive: true)
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

    private void ToggleSidePanel(bool visible)
    {
      _sidePanel.transform.DOMoveX(
        endValue:
          visible
            ? 0
            : -_sidePanel.GetComponentSafe<RectTransform>().rect.width,
        duration: _SidePanelCloseDurationInSeconds
      );
    }
  }
}
