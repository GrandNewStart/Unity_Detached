using UnityEngine;

public class TreadmillController : MonoBehaviour
{
    public GameManager  gameManager;
    public AudioSource  motorSound;
    public GameObject   player;
    public short        dir;
    public float        speed;

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
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            if (rb.bodyType != RigidbodyType2D.Dynamic) return;
            if (rb.velocity.magnitude < speed)
            {
                rb.velocity = new Vector2(dir * speed * Time.deltaTime, rb.velocity.y * Time.deltaTime);
            }
        }
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (!player.isOnTreadmill)
            {
                player.isOnTreadmill = true;
                player.treadmillVelocity = (dir * speed);
            }
        }
        if (collision.collider.CompareTag("Arm"))
        {
            ArmController hand = collision.gameObject.GetComponent<ArmController>();
            if (!hand.isOnTreadmill)
            {
                hand.isOnTreadmill = true;
                hand.treadmillVelocity = (dir * speed);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.isOnTreadmill = false;
        }
        if (collision.collider.CompareTag("Arm"))
        {
            ArmController arm = collision.gameObject.GetComponent<ArmController>();
            arm.isOnTreadmill = false;
        }
    }

}
