using System.Collections.Generic;
using NUnit.Framework;
using OmNomNomtek.Config;
using OmNomNomtek.UI;

// TODO: 2024-03-15 - Immortal - HI - no time for the following:
// TODO: 2024-03-15 - Immortal - HI - expand the usage of IoC container
// TODO: 2024-03-15 - Immortal - HI - write (editor mode) unit tests (can use Moq for mocking)
// TODO: 2024-03-15 - Immortal - HI - write (play mode) integrations tests
public class UIControllerTestFixture
{
  private UIController _uiController;

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    // _uiController = new UIController();
  }

  [Test]
  public void Test_test()
  {
    Assert.Pass();
  }

  [Test]
  public void Test_list_binding()
  {
    /*
    // arrange
    var itemConfigs = new List<ThingyListConfig.ThingyItemConfig>
    {
      new() {
        Title = "Test Thingy 1",
        Thumbnail = null,
        Prefab = null,
      },
      new() {
        Title = "Test Thingy 2",
        Thumbnail = null,
        Prefab = null,
      },
    };

    // act
    _uiController.BindThingyList(itemConfigs);

    // asset
    Assert.AreEqual(itemConfigs.Count, _uiController.ItemsCount);
    */
  }
}
