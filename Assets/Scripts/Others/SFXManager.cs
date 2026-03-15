using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    [SerializeField] List<AudioSource> SFX;
    public float currentVolume = 1;

    public void playWalk()
    {
        SFX[0].Play();
    }
    public void stopWalk()
    {
        SFX[0].Stop();
    }
    public void playJump()
    {
        SFX[1].Play();
    }
    public void playAttack()
    {
        SFX[3].Play();
    }
    public void playHit()
    {
        SFX[4].Play();
    }
    public void playLandingSFX()
    {
        SFX[2].Play();
    }
    public void playBow()
    {
        SFX[5].Play();
    }
    public void playArrow()
    {
        SFX[6].Play();
    }
    public void playAttack2()
    {
        SFX[7].Play();
    }
    
    public void changeVolume(float volume)
    {
        for (int i = 0; i < SFX.Capacity; i++)
        {
            SFX[i].volume = volume;
            currentVolume = volume;
        }
    }
}
