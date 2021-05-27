using UnityEngine;

public class TreadmillController : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource motorSound;

    private void Awake()
    {
        gameManager.treadmills.Add(this);
    }

    private void Start()
    {
        if (gameManager.isPaused) { return; }
        PlayOperationSound();
    }

    public void PlayOperationSound()
    {
        motorSound.loop = true;
        motorSound.Play();
    }

    public void StopOperationSound()
    {
        motorSound.Pause();
    }

    public void AdjustAudio(float volume)
    {
        motorSound.volume = volume;
    }
}
