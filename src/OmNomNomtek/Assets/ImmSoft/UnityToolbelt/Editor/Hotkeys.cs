using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ImmSoft.UnityToolbelt.Editor
{
  public class Hotkeys : ScriptableObject
  {
    private static MethodInfo _clearConsoleMethod;

    [MenuItem(Consts.MenuPath + "Save All #%&s")]
    public static void SaveAll()
    {
      DoSaveAll();
    }

    [MenuItem(Consts.MenuPath + "Clear Console #%&c")]
    public static void ClearConsole()
    {
      DoClearConsole();
    }

    [MenuItem(Consts.MenuPath + "Build #%d")]
    public static void Build()
    {
      DoClearConsole();
      DoSaveAll();

      BuildPlayerOptions buildPlayerOptions =
        BuildPlayerWindow.DefaultBuildMethods
          .GetBuildPlayerOptions(
            defaultBuildPlayerOptions: new BuildPlayerOptions()
          );

      BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
    }

    private static void DoSaveAll()
    {
      AssetDatabase.SaveAssets();
      EditorSceneManager.SaveOpenScenes();
      EditorApplication.ExecuteMenuItem("File/Save Project");

      Debug.Log("Saved All");
    }

    private static void DoClearConsole()
    {
      ClearConsoleMethod.Invoke(new object(), null);
    }

    private static MethodInfo ClearConsoleMethod
    {
      get
      {
        if (_clearConsoleMethod == null)
        {
          Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
          Type logEntries = assembly.GetType("UnityEditor.LogEntries");

          _clearConsoleMethod = logEntries.GetMethod("Clear");
        }

        return _clearConsoleMethod;
      }
    }
  }
}
