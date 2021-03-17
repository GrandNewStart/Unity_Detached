using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwitchController : SwitchController
{
    public GameObject   maxHeightCheck;
    public GameObject   minHeightCheck;
    public AudioSource  operationSound;
    public float        speed;
    private Vector3     targetPosition;
    private float       maxHeight;
    private float       minHeight;
    private bool        isGoingUp = true;

    private void Awake()
    {
        gameManager.lifts.Add(this);
    }

    protected override void Start()
    {
        base.Start();
        targetPosition  = target.transform.position;
        maxHeight       = maxHeightCheck.transform.position.y;
        minHeight       = minHeightCheck.transform.position.y;
        operationSound.transform.parent     = target.transform;
        operationSound.transform.position   = target.transform.position;
    }

    public override void OnActivation()
    {
        StartCoroutine(RaiseLift());
    }

    public override void OnDeactivation()
    {
        StartCoroutine(LowerLift());
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

        StopOperationSound();
    }

    private void PlayOperationSound()
    {
        if (!operationSound.isPlaying)
        {
            operationSound.Play();
        }
    }

    private void StopOperationSound()
    {
        if (operationSound.isPlaying)
        {
            operationSound.Stop();
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
