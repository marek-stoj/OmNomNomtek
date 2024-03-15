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
  public class UIController : MonoBehaviour
  {
    private const float _SidePanelCloseDurationInSeconds = 0.2f;

    [SerializeField]
    private ThingiesContainer _thingiesContainer;

    [SerializeField]
    private TMP_InputField _listFilterInputField;

    [SerializeField]
    private GameObject _sidePanel;

    [SerializeField]
    private GameObject _scrollViewContent;

    [SerializeField]
    private GameObject _listItemPrefab;

    private void Start()
    {
      _listFilterInputField.onValueChanged.AddListener(OnListFilterInputValueChanged);

      _thingiesContainer.ThingySpawned += OnThingySpawned;
      _thingiesContainer.ThingySpawnCancelled += OnThingySpawnCancelled;
      _thingiesContainer.ThingyPlaced += OnThingyPlaced;
    }

    private void OnDestroy()
    {
      _listFilterInputField.onValueChanged.RemoveListener(OnListFilterInputValueChanged);

      _thingiesContainer.ThingySpawned -= OnThingySpawned;
      _thingiesContainer.ThingySpawnCancelled -= OnThingySpawnCancelled;
      _thingiesContainer.ThingyPlaced -= OnThingyPlaced;
    }

    private void Update()
    {
      // do nothing
    }

    // TODO: 2024-03-15 - Immortal - HI - for binding we should use something like Peppermint DataBinding
    public void BindThingyList(List<ThingyListConfig.ThingyItemConfig> items)
    {
      // remove all existing list items
      _scrollViewContent
        .GetChildren()
        .ToList()
        .ForEach(Destroy);

      // create and bind new list items
      foreach (ThingyListConfig.ThingyItemConfig itemConfig in items)
      {
        GameObject listItem = Instantiate(_listItemPrefab, _scrollViewContent.transform);

        ThingyListItem thingyListItem =
          listItem.GetComponentSafe<ThingyListItem>();

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

      // activate/deactivate list items based on the filter
      _scrollViewContent
        .GetComponentsInChildren<ThingyListItem>(includeInactive: true)
        .ForEach(tli => tli.gameObject.SetActive(ifMatchesFilter(tli)));
    }

    private void OnListItemClicked(ThingyListItem thingyListItem)
    {
      if (_thingiesContainer.IsCarrying)
      {
        return;
      }

      _thingiesContainer.SpawnThingy(thingyListItem.Prefab);
    }

    private void OnThingySpawned(Thingy thingy)
    {
      ToggleSidePanel(shouldBeVisible: false);
    }

    private void OnThingySpawnCancelled()
    {
      ToggleSidePanel(shouldBeVisible: true);
    }

    private void OnThingyPlaced(Thingy thingy)
    {
      ToggleSidePanel(shouldBeVisible: true);
    }

    private void ToggleSidePanel(bool shouldBeVisible)
    {
      // slide the panel left or right based on whether it should be visible
      _sidePanel.transform.DOMoveX(
        endValue:
          shouldBeVisible
            ? 0
            : -_sidePanel.GetComponentSafe<RectTransform>().rect.width,
        duration: _SidePanelCloseDurationInSeconds
      );
    }
  }
}
