using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerDefendStatus
    {
        Dodge,
        Block,
        PerfectBlock
    }
    // Start is called before the first frame update
    public float maxStaminaPoint;
    public float currentStaminaPoint;
    public float maxHealthPoint;
    public float currentHealthPoint;
    public float maxManaPoint;
    public float currentManaPoint;
    public bool runOutStamina;

    public bool dodge;
    public bool block;
    public bool perfectBlock;

    Animator animator;
    private void Awake()
    {
        CreateCharacterStatus();
    }
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
    private void CreateCharacterStatus()
    {
        maxHealthPoint = 100f;
        maxManaPoint = 100f;
        maxStaminaPoint = 100f;
        currentHealthPoint = maxHealthPoint;
        currentManaPoint = maxManaPoint;
        currentStaminaPoint = maxStaminaPoint;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "EnemyWeapon")
        {   
            Debug.Log(other.gameObject.GetComponentInParent<EnemyController>().GetEnemyAttackPoint());
        }
    }
}
