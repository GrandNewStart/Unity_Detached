using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private void InitAudioAttributes()
    {
        chargePitch = chargeSound.pitch;
        chargeSoundOriginalPitch = chargeSound.pitch;
        footStepSound.volume = .2f;
        jumpSound.volume = .8f;
        chargeSound.volume = .3f;
        fireSound.volume = 1f;
        retrieveSound.volume = .08f;
        retrieveCompleteSound.volume = .4f;
        footStepVolume = footStepSound.volume;
        jumpVolume = jumpSound.volume;
        chargeVolume = chargeSound.volume;
        fireVolume = fireSound.volume;
        retrieveVolume = retrieveSound.volume;
        footStepDelay = 0;
        retrieveCompleteVolume = retrieveCompleteSound.volume;
        isChargeSoundPlaying = false;
    }

    private void PlayFootStepSound()
    {
        if (footStepDelay++ > 20)
        {
            footStepSound.Play();
            footStepDelay = 0;
        }
    }

    private IEnumerator PlayChargeSound()
    {
        chargeSound.Play();
        chargeSound.loop = true;

        while (state == State.charge && !isDestroyed)
        {
            yield return null;
        }

        chargeSound.Stop();
        chargeSound.pitch = chargeSoundOriginalPitch;
        isChargeSoundPlaying = false;
    }

    public void PlayRetrieveSound()
    {
        retrieveSound.Play();
    }

    public void PlayRetrieveCompleteSound()
    {
        retrieveCompleteSound.Play();
    }

}