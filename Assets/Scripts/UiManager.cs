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
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.maxValue = playerController.maxStamina;
        Debug.Log(playerController.maxStamina);
        staminaBar.value = playerController.currentStamina;
        Debug.Log(playerController.currentStamina);
    }
}
