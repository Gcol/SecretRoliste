using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegleAnimator : MonoBehaviour
{

    public Animator currentAnimator;
    public bool ShowRule;

    void OnMouseDown()
    {
        currentAnimator.SetBool("Show", !currentAnimator.GetBool("Show"));


    }
    // Start is called before the first frame update
    void Start()
    {
        ShowRule = false;
    }

}
