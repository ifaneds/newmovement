using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetBools : StateMachineBehaviour
{
    float combo;
    float attackTime;
    float attackCount;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("light",false);
        animator.SetBool("heavy",false);

        attackCount = animator.GetFloat("attackCount");
        attackTime = animator.GetFloat("attackTime");







        if (animator.GetFloat("combo") < 3 || attackCount < 3)
        {
            animator.SetBool("comboFinished", false);
            animator.SetFloat("attackCount", attackCount + 1);
        } 
     
           
      

        
        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combo = Time.time - attackTime;
        

        if (animator.GetFloat("combo") >= 3 && attackCount !=0 || attackCount == 3)
        {
            animator.SetBool("comboFinished", true);
            animator.SetFloat("attackCount", 0);

        }
        animator.SetFloat("combo", combo);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetFloat("combo", combo);
        animator.SetFloat("attackTime", 0);



    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
