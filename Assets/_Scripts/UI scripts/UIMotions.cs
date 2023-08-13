using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMotions : MonoBehaviour
{
    public float transitionTime = .3f;
    public Transform hideWindowPos, showWindowPos;
    public Transform hideButtonPos, showButtonPos;

    public Vector3 scalerUpTarget, scaleDownTarget;
    public void HideWindow(GameObject window)
    {
        LeanTween.moveX(window, hideWindowPos.position.x, transitionTime);
    }
    public void ShowWindow(GameObject window)
    {
        LeanTween.moveX(window, showWindowPos.position.x, transitionTime);
    }

    public void HideButton(GameObject window)
    {
        LeanTween.moveX(window, hideButtonPos.position.x, transitionTime);
    }
    public void ShowButton(GameObject window)
    {
        LeanTween.moveX(window, showButtonPos.position.x, transitionTime);
    }

    public void ScaleUp(GameObject window)
    {
        LeanTween.scale(window, scalerUpTarget, transitionTime);
    }
    public void ScaleDown(GameObject window)
    {
        LeanTween.scale(window, scaleDownTarget, transitionTime);
    }
}
