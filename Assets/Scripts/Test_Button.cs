using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Button : Interactable
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void HitTrigger()
    {
        anim.SetTrigger("Button Press");
    }
}
