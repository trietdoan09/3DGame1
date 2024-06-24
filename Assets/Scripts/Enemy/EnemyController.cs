using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyBehavious
    {
        Idle,
        Patrolling,
        FollowPlayer
    }
    public EnemyBehavious enemyBehavious;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject tempParentObject;
    public List<Transform> wayPoints = new List<Transform>();
    [SerializeField] private Transform targetTransform;
    public NavMeshAgent agent;
    private Animator animator;
    private AISensor sensor;

    public bool isAllowAttack;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AISensor>();
        targetTransform = GameObject.FindObjectOfType<CubeMovement>().transform;
        foreach(Transform child in parentObject.transform)
        {
            wayPoints.Add(child);
        }
        isAllowAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        BehaviousController();
        CheckFollowPlayer();
    }
    private void BehaviousController()
    {
        switch (enemyBehavious)
        {
            case EnemyBehavious.Idle: 
                {
                    break;
                }
            case EnemyBehavious.Patrolling:
                {
                    parentObject.transform.parent = tempParentObject.transform;
                    break;
                }
            case EnemyBehavious.FollowPlayer:
                {
                    FollowPlayer();
                    break;
                }
            default: break;
        }
    }
    private void CheckFollowPlayer()
    {
        if (!sensor.canSeePlayer && enemyBehavious == EnemyBehavious.FollowPlayer)
        {
            enemyBehavious = EnemyBehavious.Idle;
            animator.SetBool("isIdleState", true);
            agent.SetDestination(agent.transform.position);
        }
    }
    private void FollowPlayer()
    {
        agent.SetDestination(targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < 0.5f && isAllowAttack)
        {
            isAllowAttack = false;
            StartCoroutine(NormalAttack());
        }
    }
    IEnumerator NormalAttack()
    {
        Debug.Log("attack");
        animator.SetBool("normalAttack", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("normalAttack", false);
    }
}
