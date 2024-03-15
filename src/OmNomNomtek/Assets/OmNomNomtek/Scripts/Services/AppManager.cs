using DG.Tweening;
using OmNomNomtek.Config;
using OmNomNomtek.UI;
using UnityEngine;
using Zenject;

namespace OmNomNomtek.Services
{
  public class AppManager : MonoBehaviour
  {
    [Inject]
    private UIController _uiController;

    [Inject]
    private ThingyListConfig _thingyListConfig;

    private ISampleDependency _sampleDependency;

    [Inject]
    private void Construct(ISampleDependency sampleDependency)
    {
      _sampleDependency = sampleDependency;
    }

    private void Awake()
    {
      DOTween.Init();
    }

    private void Start()
    {
      _uiController.BindThingyList(_thingyListConfig.Items);
    }

    private void Update()
    {
      // do nothing
    }
  }
}
