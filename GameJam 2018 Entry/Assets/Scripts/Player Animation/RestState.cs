using UnityEngine;
using UnityEngine.Animations;

public class RestState : StateMachineBehaviour {

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (Input.GetKey("a"))
        {
            animator.SetBool("Left", true);
        }

        else if (Input.GetKey("d"))
            animator.SetBool("Right", true);

        else if (Input.GetKey("s"))
            animator.SetBool("Down", true);

        else if (Input.GetKey("w"))
            animator.SetBool("Up", true);
    }
}
