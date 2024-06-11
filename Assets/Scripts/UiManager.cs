using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private CubeMovement playerController;
    [SerializeField] private Slider staminaBar;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<CubeMovement>();
        staminaBar.maxValue = playerController.maxStamina;
        staminaBar.value = playerController.currentStamina;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.value = playerController.currentStamina;
    }
}
