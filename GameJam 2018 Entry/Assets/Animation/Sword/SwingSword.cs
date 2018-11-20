using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSword : StateMachineBehaviour {
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (Input.GetKey("a"))
            animator.SetBool("FacingLeft", true);
        else
            animator.SetBool("FacingLeft", false);

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Swing");       
        }
	}
}
