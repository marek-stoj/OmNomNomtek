using UnityEditor;
using UnityEngine;

namespace ImmSoft.UnityToolbelt.Editor
{
  public class UnityToolbeltMenu : EditorWindow
  {
    [MenuItem(Consts.MenuPath + "Find Asset by GUID")]
    public static void FindAssetByGuid()
    {
      string path = AssetDatabase.GUIDToAssetPath(new GUID("c9f956787b1d945e7b36e0516201fc76"));

      Debug.Log($"Asset path: {path}.");
    }
  }
}
