using OmNomNomtek.Config;
using OmNomNomtek.UI;
using UnityEngine;
using Zenject;

namespace OmNomNomtek.Services
{
  /// <summary>
  /// Inversion of Control container.
  /// </summary>
  /// <remarks>
  /// Just some example of how to use Zenject to inject dependencies.
  /// </remarks>
  public class IoC : MonoInstaller
  {
    [SerializeField]
    private ThingyListConfig _thingyListConfig;

    public override void InstallBindings()
    {
      Container.Bind<ISampleDependency>()
        .To<SampleDependency>()
        .AsSingle()
        .NonLazy();

      Container.Bind<UIController>()
        .FromComponentInHierarchy()
        .AsSingle();

      Container.Bind<ThingyListConfig>()
        .FromScriptableObject(_thingyListConfig)
        .AsSingle();
    }
  }
}
