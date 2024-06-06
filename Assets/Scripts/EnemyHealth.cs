using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public GameObject healthBar;

    public float maxHealth;
    private float crrHealth;

    private void Start()
    {
        maxHealth = float.Parse(healthText.text);
        crrHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.transform.rotation = Quaternion.LookRotation(healthBar.transform.position - Camera.main.transform.position);
        healthText.transform.rotation = healthBar.transform.rotation;
    }

    public void TakeDamage(int _damage)
    {
        crrHealth -= _damage;

        healthText.text = crrHealth.ToString();

        healthBar.GetComponent<Image>().fillAmount = crrHealth / maxHealth;

        if (crrHealth <= 0)
        {
            Destroy(gameObject);
            CubeMovement.score += 10;
        }
    }
}
