using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MagnetController
{
    private void PlayMoveSound(Vector2 movement)
    {
        if (movement.magnitude == 0)
        {
            StopMotorSound();
        }
        else
        {
            AdjustMotorPitch(movement);
            PlayMotorSound();
        }
    }

    private void AdjustMotorPitch(Vector2 movement)
    {
        motorSound.pitch = motorSoundPitch + 0.2f * movement.magnitude / moveSpeed / Time.deltaTime;
    }

    private void PlayMotorSound()
    {
        if (motorSound.isPlaying) return;
        motorSound.Play();
    }

    private void StopMotorSound()
    {
        motorSound.Stop();
    }

}