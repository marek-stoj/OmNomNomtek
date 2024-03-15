using OmNomNomtek.Config;
using OmNomNomtek.UI;
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
    public override void InstallBindings()
    {
      Container.Bind<ISampleDependency>()
        .To<SampleDependency>()
        .AsSingle()
        .NonLazy();

      // Container.Bind<UIManager>()
      //   .FromComponentInHierarchy()
      //   .AsSingle();

      // Container.Bind<ThingyListConfig>()
      //   .FromResource("OmNomNomtek/Config/DefaultThingyListConfig")
      //   .AsSingle()
      //   .NonLazy();
    }
  }
}
