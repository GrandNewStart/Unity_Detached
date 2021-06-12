using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Arm
{
    public void Fire(float firePower)
    {
        PrepareFire();
        rigidbody.AddForce(CalculateVector(firePower), ForceMode2D.Impulse);
    }

    private void PrepareFire() 
    {
        rigidbody.gravityScale  = 6;
        rigidbody.mass          = 1;

        gameObject.transform.position = CalculateStartPosition();
        gameObject.SetActive(true);

        isFired         = true;
        didTouchGround  = false;
        isMovable       = false;
        dir             = 0;
    }

    private Vector2 CalculateVector(float firePower)
    {
        Vector2 result = Vector2.zero;
        firePower *= 0.2f;
        switch (player.lastDir)
        {
            case 1:
                lastDir = 1;
                result = new Vector2(5 + firePower, 15 + firePower);
                break;
            case -1:
                lastDir = -1;
                result = new Vector2(-5 - firePower, 15 + firePower);
                break;
        }
        return result;
    }

    private Vector2 CalculateStartPosition()
    {
        Vector2 result = player.transform.position;
        switch (player.lastDir)
        {
            case 1:
                result.x += .5f;
                break;
            case -1:
                result.x -= .5f;
                break;
        }
        return result;
    }

    public void Return()
    {
        if (isReturning) return;
        if (isPlugged) { ForcePlugOut(); }

        EnableCollider(false);
        rigidbody.velocity      = Vector3.zero;
        rigidbody.gravityScale  = 0f;
        rigidbody.mass          = 0f;
        isMovable               = false;

        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        isReturning = true;

        while (isReturning)
        {
            Vector2 distance = player.transform.position - transform.position;
            Vector2 direction = distance.normalized;
            Vector2 movement = direction * retrieveSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            if (distance.magnitude < retrieveRadius)
            {
                gameObject.SetActive(false);
                EnableCollider(true);
                transform.position      = origin;
                rigidbody.gravityScale  = 1;
                rigidbody.mass          = 6;
                isFired                 = false;
                didTouchGround          = false;
                isReturning             = false;
                player.FinishRetrieval(this);

                if (hasControl)
                {
                    gameManager.SetControl(ControlIndex.player);
                }
            }

            yield return null;
        }
    }
}