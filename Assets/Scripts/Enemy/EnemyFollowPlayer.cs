using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : StateMachineBehaviour
{
    AISensor sensor;
    EnemyController enemyMovement;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isIdleState", false);
        sensor = animator.GetComponent<AISensor>();
        enemyMovement = animator.GetComponent<EnemyController>();
        animator.GetComponent<EnemyController>().enemyBehavious = EnemyController.EnemyBehavious.FollowPlayer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //k thay player
        if (!sensor.canSeePlayer)
        {
            animator.SetBool("seePlayer", false);
            animator.SetBool("isIdleState", true);
            enemyMovement.agent.SetDestination(enemyMovement.agent.transform.position);
        }
        //thay player
        enemyMovement.agent.SetDestination(enemyMovement.targetTransform.position);
        float distance = Vector3.Distance(enemyMovement.transform.position, enemyMovement.targetTransform.position);
        if (distance < 0.5f && enemyMovement.isAllowAttack)
        {
            animator.SetBool("normalAttack", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
