using DG.Tweening;
using OmNomNomtek.Config;
using OmNomNomtek.UI;
using UnityEngine;

namespace OmNomNomtek.Services
{
  public class AppManager : MonoBehaviour
  {
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private ThingyListConfig _thingyListConfig;

    private void Awake()
    {
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
