using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwitchController : SwitchController
{
    public GameObject   maxHeightCheck;
    public GameObject   minHeightCheck;
    public AudioSource  operationSound;
    private Vector3     targetPosition;
    public float        speed;
    private float       maxHeight;
    private float       minHeight;

    protected override void Start()
    {
        base.Start();
        targetPosition  = target.transform.position;
        maxHeight       = maxHeightCheck.transform.position.y;
        minHeight       = minHeightCheck.transform.position.y;
        maxHeightCheck.transform.parent = null;
        minHeightCheck.transform.parent = null;
    }

    private void FixedUpdate()
    {
        Operate();
    }

    private void Operate()
    {
        targetPosition = target.transform.position;

        if (isLeftPlugged || isRightPlugged)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        if (targetPosition.y <= maxHeight)
        {
            Move(1);
            PlayOperationSound();
        }
        else
        {
            StopOperationSound();
        }
    }

    private void MoveDown()
    {
        if (targetPosition.y > minHeight)
        {
            Move(-2);
            PlayOperationSound();
        }
        else
        {
            StopOperationSound();
        }
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
        target.transform.Translate(new Vector3(0.0f, speed * dir, 0.0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            new Vector3(minHeightCheck.transform.position.x, minHeightCheck.transform.position.y, 0.0f), 
            new Vector3(maxHeightCheck.transform.position.x, maxHeightCheck.transform.position.y, 0.0f));
        Gizmos.DrawWireSphere(minHeightCheck.transform.position, 1.0f);
        Gizmos.DrawWireSphere(maxHeightCheck.transform.position, 1.0f);
    }
}
