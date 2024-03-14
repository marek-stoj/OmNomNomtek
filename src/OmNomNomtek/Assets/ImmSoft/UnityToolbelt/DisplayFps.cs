using System;
using TMPro;
using UnityEngine;

namespace ImmSoft.UnityToolbelt
{
  /// <summary>
  /// This script calculate the current fps and show it to a text ui.
  /// </summary>
  /// <remarks>
  /// Credits: http://answers.unity.com/comments/1538513/view.html
  /// </remarks>
  public class DisplayFps : MonoBehaviour
  {
    public TMP_Text fpsText;
    public string formatedString = "{value} FPS";
    public float updateRateInSeconds = 0.25f;

    private int _frameCount = 0;
    private float _dt = 0.0f;
    private float _fps = 0.0f;

    private void Update()
    {
      _frameCount++;
      _dt += Time.unscaledDeltaTime;

      if (_dt > updateRateInSeconds)
      {
        _fps = _frameCount / _dt;
        _frameCount = 0;
        _dt -= updateRateInSeconds;
      }

      fpsText.text = formatedString.Replace("{value}", Math.Round(_fps, 1).ToString("0.0"));
    }
  }
}
