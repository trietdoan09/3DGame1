using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public enum PlayerDefendStatus
    {
        Normal,
        Dodge,
        Block,
        PerfectBlock
    }
    [SerializeField] private PlayerDefendStatus defendStatus;

    Animator animator;
    private PlayerManager playerManager;
    [SerializeField] private bool enemyBehindPlayer;
    [SerializeField] private bool isAttack;
    public float countAttack;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerManager = gameObject.GetComponent<PlayerManager>();
        enemyBehindPlayer = false;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerParry();
        AttackSystem();
    }

    private void PlayerParry()
    {

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("isBlock", true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttack)
            {
                isAttack = true;
                animator.SetBool("isAttack", true);
            }
            else
            {
                Debug.Log("in here");
                countAttack = 1;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("isBlock", false);
        }
    }
    private void AttackSystem()
    {
        if (isAttack)
        {
            EndAttack();
        }
    }
    public void EndAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            if (countAttack > 0)
            {
                animator.SetFloat("continueAttack", countAttack);
                countAttack = 0;
            }
            else
            {
                animator.SetBool("isAttack", false);
                isAttack = false;
            }
        }
    }
    public void PerfectBlock()
    {
        defendStatus = PlayerDefendStatus.PerfectBlock;
    }
    public void NormalBlock()
    {
        defendStatus = PlayerDefendStatus.Block;
    }
    public void BlockingReturnNormal()
    {
        defendStatus = PlayerDefendStatus.Normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.position.z - transform.position.z < 0)
        {
            var royEnemy = other.transform.rotation.eulerAngles.y > 180 ? other.transform.rotation.eulerAngles.y - 360 : other.transform.rotation.eulerAngles.y;
            var royPlayer = transform.rotation.eulerAngles.y > 180 ? transform.rotation.eulerAngles.y - 360 : transform.rotation.eulerAngles.y;
            if (royEnemy > 0 && royPlayer > 0 || royEnemy < 0 && royPlayer < 0)
            {
                enemyBehindPlayer = true;
            }
            else
            {
                enemyBehindPlayer = false;
            }
        }
        if (other.gameObject.tag == "EnemyWeapon")
        {
            var enemyDamage = other.gameObject.GetComponentInParent<EnemyStatusManager>().GetEnemyAttackPoint();
            switch (defendStatus)
            {
                case PlayerDefendStatus.Block:
                    {
                        other.gameObject.GetComponentInParent<EnemyStatusManager>().IncreaseEnemyCurrentBreakPoint(playerManager.playerAttack);
                        playerManager.PlayerTakeDamage(enemyDamage/2);
                        Debug.Log("Block");
                        break;
                    }
                case PlayerDefendStatus.PerfectBlock:
                    {
                        if (enemyBehindPlayer)
                        {
                            playerManager.PlayerTakeDamage(enemyDamage);
                        }
                        else
                        {
                            other.gameObject.GetComponentInParent<EnemyStatusManager>().IncreaseEnemyCurrentBreakPoint(playerManager.playerAttack);
                        }
                        Debug.Log("PerfectBlock");
                        break;
                    }
                case PlayerDefendStatus.Dodge:
                    {

                        Debug.Log("Dodge");
                        break;
                    }
                case PlayerDefendStatus.Normal:
                    {
                        playerManager.PlayerTakeDamage(enemyDamage);
                        break;
                    }
                default: break;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.transform.position.z - transform.position.z < 0)
        {
            var royEnemy = other.transform.rotation.eulerAngles.y > 180 ? other.transform.rotation.eulerAngles.y - 360 : other.transform.rotation.eulerAngles.y;
            var royPlayer = transform.rotation.eulerAngles.y > 180 ? transform.rotation.eulerAngles.y - 360 : transform.rotation.eulerAngles.y;
            if (royEnemy > 0 && royPlayer > 0 || royEnemy < 0 && royPlayer < 0)
            {
                enemyBehindPlayer = true;
            }
            else
            {
                enemyBehindPlayer = false;
            }
        }
    }
}
