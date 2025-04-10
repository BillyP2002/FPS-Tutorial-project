using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }
    

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        //DropCurrentWeapon(pickedUpWeapon);
        
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        WeaponScript weapon = pickedUpWeapon.GetComponent<WeaponScript>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    /*private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<WeaponScript>().isActiveWeapon = false;
            weaponToDrop.GetComponent<WeaponScript>().animator.enabled = false;
            
            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }*/ //Weapon pick up functionality to be added, I had issues with it so I'm coming back to it later

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            currentWeapon.isActiveWeapon = false;
        }
        
        activeWeaponSlot = weaponSlots[slotNumber];
        
        if (activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            newWeapon.isActiveWeapon = true;
        }
        
    }
}