using System.Collections;
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
        if (isMoveSoundPlaying) return;
        if (!isMoving) return;

        StartCoroutine(MoveSoundRoutine());
    }

    private IEnumerator MoveSoundRoutine()
    {
        isMoveSoundPlaying = true;

        while (isMoving)
        {
            moveSound.Play();
            yield return new WaitForSeconds(0.5f);
        }

        isMoveSoundPlaying = false;
        moveSound.Stop();
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