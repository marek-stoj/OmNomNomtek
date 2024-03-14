using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmNomNomtek.Config
{
  [CreateAssetMenu(fileName = "XxxThingyListConfig", menuName = "OmNomNomtek/Thingy List Config")]
  public class ThingyListConfig : ScriptableObject
  {
    [Serializable]
    public struct ThingyItemConfig
    {
      public string Title;

      public Sprite Thumbnail;

      public GameObject Prefab;
    }

    [SerializeField]
    private List<ThingyItemConfig> _items;

    public List<ThingyItemConfig> Items => _items;
  }
}
