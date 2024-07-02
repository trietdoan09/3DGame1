using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxStaminaPoint;
    public float currentStaminaPoint;
    public float maxHealthPoint;
    public float currentHealthPoint;
    public float maxManaPoint;
    public float currentManaPoint;
    public float playerAttack;
    public bool runOutStamina;
    public bool isImmortal;


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
    }
    private void CreateCharacterStatus()
    {
        maxHealthPoint = 100f;
        maxManaPoint = 100f;
        maxStaminaPoint = 100f;
        currentHealthPoint = maxHealthPoint;
        currentManaPoint = maxManaPoint;
        currentStaminaPoint = maxStaminaPoint;
        playerAttack = 20f;
    }
    public void PlayerTakeDamage(float damage)
    {
        currentHealthPoint = currentHealthPoint - damage <= 0 ? 0 : currentHealthPoint - damage;
    }
    public void PlayerBlockAttack(float damage)
    {
        if(currentStaminaPoint - damage <= 0)
        {
            runOutStamina = true;
            animator.SetBool("isKnockedOut", true);
            StartCoroutine(RunOutStamina());
        }
        currentStaminaPoint = currentStaminaPoint - damage <= 0 ? 0 : currentStaminaPoint - damage;
    }
    public IEnumerator RunOutStamina()
    {
        int coldown = 5;
        while (coldown > 0)
        {
            coldown--;
            yield return new WaitForSeconds(1f);
        }
        runOutStamina = false;
        yield return null;
        animator.SetBool("isKnockedOut", false);
        isImmortal = true;
        Invoke("Immortal", 4f);
    }
    private void Immortal()
    {
        isImmortal = false;
    }
}
