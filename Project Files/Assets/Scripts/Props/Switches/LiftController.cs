using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : SwitchController
{
    public GameObject   maxHeightCheck;
    public GameObject   minHeightCheck;
    public AudioSource  motorSound;
    public float        speed;
    private Rigidbody2D targetRB;
    private Vector3     targetPosition;
    private float       maxHeight;
    private float       minHeight;
    private bool        isGoingUp = true;

    protected override void Start()
    {
        base.Start();
        targetRB        = target.GetComponent<Rigidbody2D>();
        targetPosition  = target.transform.position;
        maxHeight       = maxHeightCheck.transform.position.y;
        minHeight       = minHeightCheck.transform.position.y;
        motorSound.transform.parent     = target.transform;
        motorSound.transform.position   = target.transform.position;
    }

    public override void OnActivation(ArmController arm)
    {
        base.OnActivation(arm);
        StartCoroutine(RaiseLift());
    }

    public override void OnDeactivation()
    {
        base.OnDeactivation();
        StartCoroutine(LowerLift());
    }

    public override void AdjustAudio(float volume)
    {
        base.AdjustAudio(volume);
        motorSound.volume = volume;
    }

    private IEnumerator RaiseLift()
    {
        isGoingUp = true;

        while (targetPosition.y <= maxHeight && isGoingUp)
        {
            targetPosition = target.transform.position;
            targetPosition.y += speed * Time.deltaTime;
            target.transform.position = targetPosition;

            if (gameManager.isPaused)
            {
                StopOperationSound();
            }
            else
            {
                PlayOperationSound();
            }

            yield return new WaitForFixedUpdate();
        }

        targetRB.velocity = Vector2.zero;

        if (isGoingUp)
        {
            target.transform.position = maxHeightCheck.transform.position;
        }
        StopOperationSound();
    }

    private IEnumerator LowerLift()
    {
        isGoingUp = false;

        while (targetPosition.y > minHeight && !isGoingUp)
        {
            targetPosition = target.transform.position;
            targetPosition.y -= 2 * speed * Time.deltaTime;
            target.transform.position = targetPosition;

            if (gameManager.isPaused)
            {
                StopOperationSound();
            }
            else
            {
                PlayOperationSound();
            }

            yield return new WaitForFixedUpdate();
        }

        targetRB.velocity = Vector2.zero;

        if (!isGoingUp)
        {
            target.transform.position = minHeightCheck.transform.position;
        }
        StopOperationSound();
    }

    private void PlayOperationSound()
    {
        if (!motorSound.isPlaying)
        {
            motorSound.Play();
        }
    }

    private void StopOperationSound()
    {
        if (motorSound.isPlaying)
        {
            motorSound.Stop();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            new Vector3(minHeightCheck.transform.position.x, minHeightCheck.transform.position.y, 0.0f), 
            new Vector3(maxHeightCheck.transform.position.x, maxHeightCheck.transform.position.y, 0.0f));
        Gizmos.DrawWireSphere(minHeightCheck.transform.position, 0.5f);
        Gizmos.DrawWireSphere(maxHeightCheck.transform.position, 0.5f);
    }
}
