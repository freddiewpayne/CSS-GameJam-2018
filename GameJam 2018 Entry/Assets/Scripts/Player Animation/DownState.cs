﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownState : StateMachineBehaviour {

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!Input.GetKey("s"))
            animator.SetBool("Down", false);

    }
}
