using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Physical Object"))
        {
            collision
                .collider
                .transform
                .parent
                .SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Physical Object"))
        {
            collision
                .collider
                .transform
                .parent
                .SetParent(null);
        }
    }
}
