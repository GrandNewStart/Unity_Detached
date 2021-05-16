using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArmController
{
    // called by PlayerController
    public void Fire(float power)
    {
        OnFire();
        rigidbody.AddForce(CalculateVector(power), ForceMode2D.Impulse);
    }

    private void OnFire()
    {
        rigidbody.gravityScale  = normalGScale;
        rigidbody.mass          = normalMass;

        gameObject.transform.position = CalculateStartPosition();
        gameObject.SetActive(true);
        EnableCollider(true);

        isOut           = true;
        isFireComplete  = false;
        dir             = 0;
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

    private Vector2 CalculateVector(float power)
    {
        Vector2 result = Vector2.zero;
        switch (player.lastDir)
        {
            case 1:
                lastDir = 1;
                result = new Vector2(5 + power, 15 + power);
                break;
            case -1:
                lastDir = -1;
                result = new Vector2(-5 - power, 15 + power);
                break;
        }
        return result;
    }

}