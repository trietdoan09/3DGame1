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
    public Transform targetTransform;
    public NavMeshAgent agent;
    private Animator animator;
    private AISensor sensor;

    public bool isAllowAttack;
    private float attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        attackPoint = 20f;
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
        CheckSeePlayer();
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
                    break;
                }
            default: break;
        }
    }
    public float GetEnemyAttackPoint()
    {
        return attackPoint;
    }
    private void CheckSeePlayer()
    {
        if (!sensor.canSeePlayer && enemyBehavious == EnemyBehavious.FollowPlayer)
        {
            enemyBehavious = EnemyBehavious.Idle;
        }
    }
    public void EndNormalAttack()
    {

        animator.SetBool("seePlayer", true);
        animator.SetBool("normalAttack", false);
    }

}
