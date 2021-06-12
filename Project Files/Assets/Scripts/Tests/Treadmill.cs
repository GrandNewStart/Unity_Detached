using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    public GameManager2 gameManager;
    [Range(-3f, 3f)]
    public float treadmillSpeed;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        // Register to GameManager
    }

    private void FixedUpdate()
    {
        Vector2 pos = rigidbody.position;
        rigidbody.position += Vector2.left * treadmillSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(pos);
    }

    public void Pause()
    {
        // Pause audio or stuff
    }

    public void Resume()
    {
        // Resume audio or stuff
    }
}
