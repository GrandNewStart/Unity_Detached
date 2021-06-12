using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObject2 : MonoBehaviour
{
    [SerializeField] protected GameManager2 gameManager;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Vector2 groundCheckVector;
    [SerializeField] private Collider2D deathCollider;
    public bool isDestroyed = false;
    public bool isGrounded = false;
    public Vector2 origin;
    protected Vector2 velocity;
    protected new Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    protected virtual void Update() { }

    protected virtual void FixedUpdate()
    {
        GroundCheck();
        DeathCheck();
    }

    public virtual void Pause() { }
    public virtual void Resume() { }
    public virtual void Destroy() { }
    public virtual void Restore() { }

    protected virtual void GroundCheck()
    {
        isGrounded = false;
        Collider2D col1 = Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, LayerMask.GetMask("Ground"));
        Collider2D col2 = Physics2D.OverlapBox(groundCheck.position, groundCheckVector, 0, LayerMask.GetMask("Physical Object"));
        isGrounded = col1 || col2;
    }

    protected virtual void DeathCheck()
    {
        if (deathCollider != null)
        {
            isDestroyed = deathCollider.IsTouchingLayers();
            if (isDestroyed)
            {
                Destroy();
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.SetParent(collision.collider.transform);
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) {}

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }

}
