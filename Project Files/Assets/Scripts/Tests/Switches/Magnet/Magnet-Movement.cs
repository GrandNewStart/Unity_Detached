using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Magnet
{
    private void Move()
    {
        switch (direction)
        {
            case MagnetDirection.down:
            case MagnetDirection.up:
                float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;    
                movement = Vector2.right * horizontal;
                break;
            case MagnetDirection.left:
            case MagnetDirection.right:
                float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
                movement = Vector2.up * vertical;
                break;
        }
    }

    private void UpdateMovement()
    {
        magnetRB.MovePosition(magnetRB.position + movement);

        if (target != null)
        {
            target.position = pullPoint.transform.position;
        }

        switch (direction) 
        {
            case MagnetDirection.down:
            case MagnetDirection.up:
                if (magnet.transform.position.x < startPoint.transform.position.x)
                {
                    magnet.transform.position = new Vector2(startPoint.transform.position.x, magnet.transform.position.y);
                }
                if (magnet.transform.position.x > endPoint.transform.position.x)
                {
                    magnet.transform.position = new Vector2(endPoint.transform.position.x, magnet.transform.position.y);
                }
                break;
            case MagnetDirection.left:
            case MagnetDirection.right:
                if (magnet.transform.position.y < startPoint.transform.position.y)
                {
                    magnet.transform.position = new Vector2(magnet.transform.position.x, startPoint.transform.position.y);
                }
                if (magnet.transform.position.y > endPoint.transform.position.y)
                {
                    magnet.transform.position = new Vector2(magnet.transform.position.x, endPoint.transform.position.y);
                }
                break;
        }
    }
}