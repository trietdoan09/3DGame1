using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera cameraPlayer;
    [SerializeField] private GameObject crosshair;

    [Space]
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    private float nextFire;

    [Header("VFX")]
    public GameObject hitVFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        if(Input.GetMouseButton(0) && nextFire <= 0)
        {
            nextFire = 1 / fireRate;
            Fire();
        }
    }

    public void Fire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 100f))
        {
            Instantiate(hitVFX, hit.point, Quaternion.identity);
            if (hit.transform.gameObject.GetComponent<EnemyHealth>())
            {
                hit.transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
}
