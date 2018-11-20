using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUI : StateMachineBehaviour {

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
        if ( PlayerController.Instance.hp <= 0 )
        {
            animator.SetTrigger("Dead");
        }
	}
}
