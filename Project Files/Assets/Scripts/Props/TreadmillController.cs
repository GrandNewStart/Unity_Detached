using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillController : MonoBehaviour
{
    public short            dir;
    public float            speed;
    public Sound            operationSound;
    public float            maxAudioDistance;
    private float           maxVolume;
    public GameObject       player;
    public GameObject       treadmillStart;
    public GameObject       treadmillEnd;
    private Vector3         treadmillStartPosition;
    private Vector3         treadmillEndPosition;
    private Vector3         playerPosition;
    private float           distance;
    private float           volume;

    private void Awake()
    {
        initSounds();
    }

    private void initSounds()
    {
        operationSound.source               = gameObject.AddComponent<AudioSource>();
        operationSound.source.clip          = operationSound.clip;
        operationSound.source.volume        = operationSound.volume;
        operationSound.source.pitch         = operationSound.pitch;
        maxVolume                           = operationSound.volume;
    }

    private void Start()
    {
        treadmillStartPosition  = treadmillStart.transform.position;
        treadmillEndPosition    = treadmillEnd.transform.position;
        PlayOperationSound();
    }

    private void Update()
    {
        playerPosition = player.transform.position;

        if (playerPosition.x > treadmillStartPosition.x && playerPosition.x < treadmillEndPosition.x)
        {
            distance = 0f;
        }
        else if (playerPosition.x <= treadmillStartPosition.x)
        {
            distance = treadmillStartPosition.x - playerPosition.x;
        }
        else if (playerPosition.x >= treadmillEndPosition.x)
        {
            distance = playerPosition.x - treadmillEndPosition.x;
        }
        AdjustVolume();
    }

    private void PlayOperationSound()
    {
        operationSound.source.loop = true;
        operationSound.source.Play();
    }

    private void StopOperationSound()
    {
        operationSound.source.Stop();
    }

    public void MuteSound(bool mute)
    {
        operationSound.source.mute = mute;
    }

    private void AdjustVolume()
    {
        if (distance > maxAudioDistance)
        {
            volume = 0f;
        }
        else if (distance == 0)
        {
            volume = maxVolume;
        }
        else
        {
            volume = maxVolume - distance/maxAudioDistance;
        }
        operationSound.volume = volume;
        operationSound.source.volume = volume;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // determine if objects on the treadmill are supposed to move
        if (collision.collider.CompareTag("Physical Object"))
        {
            GameObject objectOnTreadmill = collision.gameObject;
            Rigidbody2D rb = objectOnTreadmill.GetComponent<Rigidbody2D>();

            if (rb.velocity.magnitude < speed)
            {
                float horizontal = dir * speed * Time.deltaTime;
                float vertical = rb.velocity.y * Time.deltaTime;
                rb.velocity = new Vector3(horizontal, vertical, 0.0f);
            }
        }
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (!player.GetOnTreadMill())
            {
                player.SetOnTreadmill(true);
                player.SetTreadmillVelocity(dir * speed);
            }
        }
        if (collision.collider.CompareTag("Arm"))
        {
            ArmController hand = collision.gameObject.GetComponent<ArmController>();
            if (!hand.GetOnTreadMill())
            {
                hand.SetOnTreadmill(true);
                hand.SetTreadmillVelocity(dir * speed);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetOnTreadmill(false);
        }
        if (collision.collider.CompareTag("Arm"))
        {
            ArmController hand = collision.gameObject.GetComponent<ArmController>();
            hand.SetOnTreadmill(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(treadmillStart.transform.position, 5);
        Gizmos.DrawWireSphere(treadmillEnd.transform.position, 5);
        Gizmos.DrawLine(playerPosition, treadmillStart.transform.position);
        Gizmos.DrawLine(playerPosition, treadmillEnd.transform.position);
    }
}
