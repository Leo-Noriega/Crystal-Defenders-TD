using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab, turretPivot, turretBody, firePoint, bulletPool;
    private List<Bullet> bullets = new();
    
    private void Awake()
    {
        for (int i = 0; i < 50; i++)
        {
            var instance = Instantiate(bulletPrefab, bulletPool.transform);
            var bullet = instance.GetComponent<Bullet>();
            bullets.Add(bullet);
            instance.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        while (true)
        {
            var available = bullets.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
            if (available)
            {
                available.direction = firePoint.transform.up;
                available.transform.position = firePoint.transform.position;
                available.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
