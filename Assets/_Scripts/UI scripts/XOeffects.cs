
// when placing X O in grid in Ui, there is a scale effect (smaller to big).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XOeffects : MonoBehaviour
{
    public Vector3 targetScale;
    public float transitionTime = .3f;
    
    void Start()
    {
        
        LeanTween.scale(gameObject, targetScale, transitionTime);
    }
}
