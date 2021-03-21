using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : SwitchController
{
    public GameObject   maxHeightCheck;
    public GameObject   minHeightCheck;
    public AudioSource  motorSound;
    public float        speed;
    private Vector3     targetPosition;
    private float       maxHeight;
    private float       minHeight;
    private bool        isGoingUp = true;

    protected override void Start()
    {
        base.Start();
        targetPosition  = target.transform.position;
        maxHeight       = maxHeightCheck.transform.position.y;
        minHeight       = minHeightCheck.transform.position.y;
        motorSound.transform.parent     = target.transform;
        motorSound.transform.position   = target.transform.position;
    }

    public override void OnActivation()
    {
        StartCoroutine(RaiseLift());
    }

    public override void OnDeactivation()
    {
        StartCoroutine(LowerLift());
    }

    public override void AdjustAudio(float volume)
    {
        plugInSound.volume = volume;
        plugOutSound.volume = volume;
        activationSound.volume = volume;
        deactivationSound.volume = volume;
        motorSound.volume = volume;
    }

    private IEnumerator RaiseLift()
    {
        isGoingUp = true;

        while (targetPosition.y <= maxHeight && isGoingUp)
        {
            targetPosition = target.transform.position;
            Move(1);

            if (gameManager.isPaused)
            {
                StopOperationSound();
            }
            else
            {
                PlayOperationSound();
            }
            yield return null;
        }

        target.transform.position = maxHeightCheck.transform.position;
        StopOperationSound();
    }

    private IEnumerator LowerLift()
    {
        isGoingUp = false;

        while (targetPosition.y > minHeight && !isGoingUp)
        {
            targetPosition = target.transform.position;
            Move(-2);

            if (gameManager.isPaused)
            {
                StopOperationSound();
            }
            else
            {
                PlayOperationSound();
            }
            yield return null;
        }

        target.transform.position = minHeightCheck.transform.position;
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

    private void Move(short dir)
    {
        float y = speed * dir * Time.deltaTime;
        target.transform.Translate(new Vector3(0, y, 0));
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
