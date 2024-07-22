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
    public float maxFoodPoint;
    public float currentFoodPoint;
    public float maxDrinkPoint;
    public float currentDrinkPoint;
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
        StartCoroutine(PlayerHunger());
        StartCoroutine(PlayerThirsty());
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
        maxFoodPoint = 100f;
        maxDrinkPoint = 100f;
        currentHealthPoint = maxHealthPoint;
        currentManaPoint = maxManaPoint;
        currentStaminaPoint = maxStaminaPoint;
        currentFoodPoint = maxFoodPoint;
        currentDrinkPoint = maxDrinkPoint;
        playerAttack = 20f;
    }
    public void PlayerTakeDamage(float damage)
    {
        currentHealthPoint = currentHealthPoint - damage <= 0 ? 0 : currentHealthPoint - damage;
    }
    public void AddStat(float value, int stat)
    {
        switch (stat)
        {
            case 0:
                {
                    //add health
                    currentHealthPoint = currentHealthPoint + value > maxHealthPoint ? maxHealthPoint : currentHealthPoint + value;
                    break;
                }
            case 1:
                {
                    //add mana
                    currentManaPoint = currentManaPoint + value > maxManaPoint ? maxManaPoint : currentManaPoint + value;
                    break;
                }
            case 2:
                {
                    //add stamina
                    currentStaminaPoint = currentStaminaPoint + value > maxStaminaPoint ? maxStaminaPoint : currentStaminaPoint + value;
                    break;
                }
            case 3:
                {
                    //add food
                    currentFoodPoint = currentFoodPoint + value > maxFoodPoint ? maxFoodPoint : currentFoodPoint + value;
                    break;
                }
            case 4:
                {
                    currentDrinkPoint = currentDrinkPoint + value > maxDrinkPoint ? maxDrinkPoint : currentDrinkPoint + value;
                    break;
                }
            default:break;
        }
    }
    IEnumerator PlayerHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentFoodPoint = currentFoodPoint - 10 < 1 ? 0 : currentFoodPoint - 10;
            if(currentFoodPoint < 1)
            {
                currentHealthPoint -= 5f;
            }
        }
    }
    IEnumerator PlayerThirsty()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentDrinkPoint = currentDrinkPoint - 15 < 1 ? 0 : currentFoodPoint - 10;
            if(currentFoodPoint < 1)
            {
                currentHealthPoint -= 1f;
            }
        }
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
