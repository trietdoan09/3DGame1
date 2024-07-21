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
        FollowPlayer,
        Attack
    }
    public EnemyBehavious enemyBehavious;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject tempParentObject;
    public List<Transform> wayPoints = new List<Transform>();
    // lay scripts
    public Transform targetTransform;
    public NavMeshAgent agent;
    private Animator animator;
    private AISensor sensor;
    private EnemyStatusManager enemyStatusManager;
    [SerializeField] private List<BoxCollider> activeWeapons;
    private bool enemyStune;
    public bool isAllowAttack;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AISensor>();
        enemyStatusManager = GetComponent<EnemyStatusManager>();
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
        EnemyRotateFace();
        ActiveColliderWeapons();
        BreakPointManager();
    }
    private void ActiveColliderWeapons()
    {
        if(activeWeapons.Count != 0)
        {
            if (enemyBehavious == EnemyBehavious.Attack)
            {
                foreach (var weapon in activeWeapons)
                {
                    weapon.enabled = true;
                }
            }
            else
            {
                foreach (var weapon in activeWeapons)
                {
                    weapon.enabled = false;
                }
            }
        }
    }
    private void EnemyRotateFace()
    {
        if (sensor.canSeePlayer && targetTransform!=null)
        {
            Vector3 direction = targetTransform.position - transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    private void BreakPointManager()
    {
        if (!enemyStune)
        {
            var enemyCurrentBreakPoint = enemyStatusManager.GetEnemyCurrentBreakPoint();
            var enemyMaxBreakPoint = enemyStatusManager.GetEnemyMaxBreakPoint();
            if (enemyCurrentBreakPoint >= enemyMaxBreakPoint)
            {
                //stun enemy
                Debug.Log("HEY HEY HEY You Stune");
                enemyStune = true;
                animator.SetBool("normalAttack", false);
                animator.SetBool("isStune", true);
                StartCoroutine(EnemyStune());
            }
            else
            {
                if (enemyCurrentBreakPoint <= 0)
                {
                    enemyStatusManager.SetEnemyCurrentBreakPoint(0);
                    return;
                }
                float descrease = 0;
                if(enemyMaxBreakPoint > enemyMaxBreakPoint * 0.75f)
                {
                    descrease = Time.deltaTime * 5f;
                }
                else if (enemyCurrentBreakPoint > enemyMaxBreakPoint)
                {
                    descrease = Time.deltaTime * 10f;
                }
                else
                {
                    descrease = Time.deltaTime * 15;
                }
                enemyStatusManager.DescreaseEnemyCurrentBreakPoint(descrease);
            }
        }
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
    private void CheckSeePlayer()
    {
        if (!sensor.canSeePlayer && enemyBehavious == EnemyBehavious.FollowPlayer)
        {
            enemyBehavious = EnemyBehavious.Idle;
        }
    }
    public void EndNormalAttack()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) > 0.5f)
        {
            animator.SetBool("seePlayer", true);
            animator.SetBool("normalAttack", false);
        }
    }
    private IEnumerator EnemyStune()
    {
        yield return new WaitForSeconds(5f);
        enemyStune = false;
        animator.SetBool("isStune", false);
        enemyStatusManager.SetEnemyCurrentBreakPoint(0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            Debug.Log("Hello");
        }
    }
}
