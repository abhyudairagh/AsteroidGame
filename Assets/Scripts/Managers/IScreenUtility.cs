using System;
using UnityEngine;

namespace Managers
{
    public interface IScreenUtility
    {
        event Action OnScreenInitialised;
        event Action OnScreenSizeChanged;
        Vector2 RightTopCorner { get; }
        Vector2 LeftBottomCorner { get; }
        float GetRandomPointScreenHeight();
        float GetRandomPointScreenWidth();
        float GetRandomPointScreenHeight(float padding);
        float GetRandomPointScreenWidth(float padding);
    }
}