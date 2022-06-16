using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = System.Random;

public class GunProjectile : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;

    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    public int bulletsLeft, bulletsShot;

    public bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;

    //public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;
    
    public void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    public void Update()
    {
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletPerTap + "/" + magazineSize / bulletPerTap);
        }
    }

    public void Shoot()
    {
        
        Debug.Log("Ca rentre dans le shoot");
        //if (readyToShoot && !reloading && bulletsLeft > 0)
        //{
            bulletsShot = 0;
            readyToShoot = false;

            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75);
            }

            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            float x = UnityEngine.Random.Range(-spread, spread);
            float y = UnityEngine.Random.Range(-spread, spread);

            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

            currentBullet.transform.forward = directionWithSpread.normalized;
        
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up, ForceMode.Impulse);

            // if (muzzleFlash != null)
            // {
            //     Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            // }
        
            bulletsLeft--;
            bulletsShot++;

            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
            }
            
        //}
        
    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    public void Reload()
    {
        //if (bulletsLeft < magazineSize && !reloading)
        //{
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
       // }
    }

    public void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

