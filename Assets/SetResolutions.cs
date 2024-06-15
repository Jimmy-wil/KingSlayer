using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolutions : MonoBehaviour
{
      List<int> widths = new List<int>() {720, 1080, 1200, 1440, 1550};
     List<int> heights = new List<int>() {720, 1080, 1200, 1440, 1550};
    
public void Setscreensize( int index)
{
    bool fullscreen = Screen.fullScreen;
    int width = widths[index];
    int height = heights[index];
    Screen.SetResolution(width, height, fullscreen);
}

public void SetFullScreen(bool _fullscreen)
{
    Screen.fullScreen = _fullscreen;
}
}
