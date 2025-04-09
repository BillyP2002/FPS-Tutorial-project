using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    
    public WeaponScript hoveredWeapon = null;

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

    public void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<WeaponScript>() && objectHitByRaycast.GetComponent<WeaponScript>().isActiveAndEnabled == false)
            {
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<WeaponScript>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
    
}