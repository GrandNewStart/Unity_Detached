using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void InitAudioAttributes()
    {
        moveSound.volume = .1f;
        moveVolume = moveSound.volume;
        holdSound.volume = .1f;
        holdVolume = holdSound.volume;
    }

    public void AdjustAudio(float volume)
    {
        moveVolume = volume * 0.2f;
        holdVolume = volume * 0.3f;
        moveSound.volume = moveVolume;
        holdSound.volume = holdVolume;
    }

    private void PlayMoveSound()
    {
        if (moveSoundDelay++ > 20 && isMovable)
        {
            moveSoundDelay = 0;
            moveSound.Play();
        }
    }

    private void StopMoveSound()
    {
        moveSound.Stop();
    }

    private void PlayHoldSound()
    {
        if (holdSound.isPlaying) return;
        holdSound.Play();
    }

    private void StopHoldSound()
    {
        if (!holdSound.isPlaying) return;
        holdSound.Stop();
    }
}