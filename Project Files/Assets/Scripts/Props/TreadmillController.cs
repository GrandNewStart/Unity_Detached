using UnityEngine;

public class TreadmillController : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource motorSound;
    public float speed;

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Physical Object"))
        {
            collision.collider.GetComponentInParent<Transform>().Translate(new Vector2(speed/2 * Time.deltaTime, 0));
        }
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponentInParent<RectTransform>().Translate(new Vector2(speed * Time.deltaTime, 0));
        }
    }

}
