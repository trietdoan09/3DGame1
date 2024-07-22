using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private CubeMovement playerController;
    private PlayerManager playerManager;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider FoodBar;
    [SerializeField] private Slider WaterBar;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<CubeMovement>();
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatus();
    }
    private void UpdatePlayerStatus()
    {
        staminaBar.maxValue = playerManager.maxStaminaPoint;
        staminaBar.value = playerManager.currentStaminaPoint;

        manaBar.maxValue = playerManager.maxManaPoint;
        manaBar.value = playerManager.currentManaPoint;

        healthBar.maxValue = playerManager.maxHealthPoint;
        healthBar.value = playerManager.currentHealthPoint;

        FoodBar.maxValue = playerManager.maxFoodPoint;
        FoodBar.value = playerManager.currentFoodPoint;

        WaterBar.maxValue = playerManager.maxDrinkPoint;
        WaterBar.value = playerManager.currentDrinkPoint;
    }
}
