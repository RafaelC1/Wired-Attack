using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TouchHelpers {

    public static bool IsTouchingOrClickingOverUI()
    {
        return ((Application.platform == RuntimePlatform.WindowsEditor && IsClickingOverUI()) ||
                (Application.platform == RuntimePlatform.Android && IsTouchingOverUI()));
    }
    
    public static bool IsTouchingOverUI()
    {
        return !((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) &&
                 !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
    }

    public static bool IsClickingOverUI()
    {
        return !(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject());
    }
}
