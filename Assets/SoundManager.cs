using UnityEngine;
using static WeaponScript;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingChannel;
    public AudioSource reloadSoundAK74;
    public AudioSource emptySoundAK74;

    public AudioClip M16Shot;
    public AudioClip m1911Shot;
    
    public AudioSource reloadSound1911;
    public AudioSource emptySound1911;
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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                shootingChannel.PlayOneShot(m1911Shot);
                break;
            case WeaponModel.AK74:
                shootingChannel.PlayOneShot(M16Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.M1911:
                reloadSound1911.Play();
                break;
            case WeaponModel.AK74:
                reloadSoundAK74.Play();
                break;
        }
    }
}