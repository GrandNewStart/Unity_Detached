using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    private void InitAudioAttributes()
    {
        moveSound.volume = .1f;
        moveVolume = moveSound.volume;
    }

    public void AdjustAudio(float volume)
    {
        moveVolume = volume * 0.2f;
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
}