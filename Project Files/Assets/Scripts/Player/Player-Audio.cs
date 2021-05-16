using System.Collections;
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
        chargeSound.loop = true;
        fireVolume = fireSound.volume;
        retrieveVolume = retrieveSound.volume;

        footStepDelay = 0;
        retrieveCompleteVolume = retrieveCompleteSound.volume;
    }

    public void AdjustAudio(float volume)
    {
        chargeVolume = volume * 0.7f;
        fireVolume = volume;
        footStepVolume = volume * 0.5f;
        jumpVolume = volume;
        retrieveCompleteVolume = volume * 0.8f;
        retrieveVolume = volume * 0.2f;
    }

    private void PlayFootStepSound()
    {
        if (footStepDelay++ > 20)
        {
            footStepSound.Play();
            footStepDelay = 0;
        }
    }

    private void PlayChargeSound()
    {
        if (chargeSound.isPlaying) return;
        StartCoroutine(ChargeSoundRoutine());
    }

    private IEnumerator ChargeSoundRoutine()
    {
        chargeSound.Play();
        while (state == State.charge && !isDestroyed)
        {
            if (chargeSound.pitch < 1.5f)
            {
                //chargeSound.pitch *= 1.001f;
                chargeSound.pitch = power / 15 + 0.5f;
            }
            else
            {
                chargeSound.pitch = 1.5f;
            }
            yield return null;
        }

        chargeSound.Stop();
        chargeSound.pitch = chargeSoundOriginalPitch;
    }

    private void PlayFireSound()
    {
        fireSound.Play();
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