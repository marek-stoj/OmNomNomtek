using DG.Tweening;
using OmNomNomtek.Config;
using OmNomNomtek.UI;
using UnityEngine;
using Zenject;

namespace OmNomNomtek.Services
{
  public class AppManager : MonoBehaviour
  {
    [SerializeField]
    // [Inject]
    private UIManager _uiManager;

    [SerializeField]
    // [Inject]
    private ThingyListConfig _thingyListConfig;

    [Inject]
    private ISampleDependency _sampleDependency;

    // [Inject]
    // private void Construct(ISampleDependency sampleDependency)
    // {
    //   _sampleDependency = sampleDependency;
    // }

    private void Awake()
    {
      Debug.Log($"_sampleDependency: {_sampleDependency}");

      DOTween.Init();
    }

    private void Start()
    {
      _uiManager.BindThingyList(_thingyListConfig.Items);
    }

    private void Update()
    {
      // do nothing
    }
  }
}
