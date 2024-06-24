using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : StateMachineBehaviour
{
    float timer;
    EnemyController enemyMovement;
    AISensor sensor;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyMovement = animator.GetComponent<EnemyController>();
        enemyMovement.enemyBehavious = EnemyController.EnemyBehavious.Patrolling;
        sensor = animator.GetComponent<AISensor>();
        timer = 0;
        enemyMovement.agent.SetDestination(enemyMovement.wayPoints[Random.Range(0, enemyMovement.wayPoints.Count)].position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!sensor.canSeePlayer)
        {
            if (enemyMovement.agent.remainingDistance <= enemyMovement.agent.stoppingDistance)
                enemyMovement.agent.SetDestination(enemyMovement.wayPoints[Random.Range(0, enemyMovement.wayPoints.Count)].position);
            timer += Time.deltaTime;
            if (timer > 10)
            {
                animator.SetBool("isPatrolling", false);
            }
        }
        else
        {
            animator.SetBool("seePlayer", true);
            enemyMovement.enemyBehavious = EnemyController.EnemyBehavious.FollowPlayer;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyMovement.agent.SetDestination(enemyMovement.agent.transform.position);
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
