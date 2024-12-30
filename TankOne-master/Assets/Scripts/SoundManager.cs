using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;


    public AudioClip shot, droneShot, shoutGunBlust, tankMove, click, homeBackground, gameBackground, airBullet, big;


    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void PlayShot()
    {

        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(shot);
    }
    public void PlayGunBlust()
    {

        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(shoutGunBlust);
    }
   
    public void PlayHomeBackground()
    {
        if (GameData.playerState.isMusic)
        {
            audioSource.clip = homeBackground;
            audioSource.Play();
        }
        else
        {
            if(audioSource.isPlaying)
            audioSource.Stop();
        }

    }
    public void PlayGameBackground()
    {
        if (GameData.playerState.isMusic)
        {
            audioSource.clip = gameBackground;
            audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }


    }
    public void DroneShot()
    {
        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(droneShot);
    }
    public void PlayAirBullet()
    {
        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(airBullet);
    }
    public void PlayBig()
    {
        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(big);
    }
    public void PlayClickSound()
    {
        if (GameData.playerState.isSound)
            audioSource.PlayOneShot(click);
    }
}
