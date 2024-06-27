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
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerManager = gameObject.GetComponent<PlayerManager>();
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
        var enemyDamage = other.gameObject.GetComponentInParent<EnemyController>().GetEnemyAttackPoint();
        Debug.Log(enemyDamage);
        if (other.gameObject.tag == "EnemyWeapon")
        {
            switch (defendStatus)
            {
                case PlayerDefendStatus.Block:
                    {
                        Debug.Log("Block");
                        playerManager.PlayerBlockAttack(enemyDamage);
                        break;
                    }
                case PlayerDefendStatus.PerfectBlock:
                    {
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
}
