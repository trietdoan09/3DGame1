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
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerManager = gameObject.GetComponent<PlayerManager>();
        enemyBehindPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerParry();
    }

    private void PlayerParry()
    {

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("isBlock", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("isBlock", false);
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
                        if (enemyBehindPlayer)
                        {
                            playerManager.PlayerTakeDamage(enemyDamage);
                        }
                        else
                        {
                            other.gameObject.GetComponentInParent<EnemyStatusManager>().IncreaseEnemyCurrentBreakPoint(playerManager.playerAttack);
                            //playerManager.PlayerBlockAttack(enemyDamage);
                        }
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
