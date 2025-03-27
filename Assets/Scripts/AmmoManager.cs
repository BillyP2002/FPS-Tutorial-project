using TMPro;
using UnityEngine;


public class AmmoManager : MonoBehaviour
{
    
    //THIS DOESN'T WORK BUT I'LL FIX IT LATER
    
    public static AmmoManager Instance { get; set; }
    
    //UI
    public TextMeshProUGUI ammoText;

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
    
}