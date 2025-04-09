using System.Collections;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponScript : MonoBehaviour
{
    //public Camera playerCamera;
    
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    public bool isActiveWeapon;
    
    //Burst
    public int bulletsPerBurst = 3;
    [FormerlySerializedAs("currentBurst")] public int burstBulletsLeft;
    
    //Spread
    public float spreadIntensity;
    
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;
    
    public GameObject muzzleEffect;
    internal Animator animator;
    
    //Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum WeaponModel
    {
        M1911,
        AK74
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActiveWeapon)
        {
            
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptySoundAK74.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //Holding down left mouse button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Burst || currentShootingMode == ShootingMode.Single)
            {
                //clicking left mouse button once
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            //pressing R key
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
            }

            //Autoreload when magazine is empty
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (AmmoManager.Instance.ammoText != null)
            {
                AmmoManager.Instance.ammoText.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            }
        }
    }

    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("recoil");
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        readyToShoot = false;
        
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        //pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        //Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //Checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        
        animator.SetTrigger("reload");
        
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        
        if(Physics.Raycast(ray, out hit))
        {
            //Hitting something 
            targetPoint = hit.point;
        }
        else
        {
            //Hitting nothing
            targetPoint = ray.GetPoint(100);
        }
        
        Vector3 direction = targetPoint - bulletSpawn.position;
        
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
