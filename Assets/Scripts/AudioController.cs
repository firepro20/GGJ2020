using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /* Singleton*/
    private static AudioController instance;
    public static AudioController Instance { get { return instance; } }

    // Class References

    // Public Variables
    public AudioClip shellExplosionClip;
    public AudioClip shotClip;
    public AudioClip shotChargeClip;
    public AudioClip tankExplosion;
    public AudioClip playerHit;
    public AudioClip pickupClip;
    public AudioClip healthClip;
    public AudioClip scoreClip;
    public AudioClip enemyHit;
    public AudioClip victorySound;

    // Private Variables
    private AudioSource mainAudioSource;
    private float volumeLevel;

    void Awake()
    {
        /* Singleton */
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        /* End of Singleton */
    }

    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        volumeLevel = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayShot()
    {
        mainAudioSource.PlayOneShot(shotClip, volumeLevel);
    }

    public void PlayShellExplosion()
    {
        mainAudioSource.PlayOneShot(shellExplosionClip, volumeLevel);
    }

    public void PlayCharge()
    {
        mainAudioSource.PlayDelayed(0.2f);
    }

    public void PlayTankExplosion()
    {
        mainAudioSource.PlayOneShot(tankExplosion, volumeLevel);
    }

    public void PlayPlayerHit()
    {
        mainAudioSource.PlayOneShot(playerHit, volumeLevel);
    }

    public void PlayPickUpOffensive()
    {
        mainAudioSource.PlayOneShot(pickupClip, volumeLevel);
    }

    public void PlayHealthPickup()
    {
        mainAudioSource.PlayOneShot(healthClip, volumeLevel);
    }

    public void PlayScoreSound()
    {
        mainAudioSource.PlayOneShot(scoreClip, volumeLevel);
    }

    public void PlayEnemyHit()
    {
        mainAudioSource.PlayOneShot(enemyHit, volumeLevel);
    }

    public void PlayVictorySound()
    {
        mainAudioSource.PlayOneShot(victorySound, volumeLevel);
    }
}
