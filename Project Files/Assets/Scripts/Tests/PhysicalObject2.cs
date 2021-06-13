using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObject2 : MonoBehaviour
{
    [SerializeField] protected GameManager2 gameManager;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Vector2 groundCheckVector;
    [SerializeField] protected GameObject normal;
    [SerializeField] protected GameObject destroyed;
    [SerializeField] private Collider2D deathCollider;
    public bool shouldRestorePosition = false;
    public bool isDestroyed = false;
    public bool isGrounded = false;
    public Vector2 origin;
    protected Vector2 velocity;
    protected new Rigidbody2D rigidbody;
    protected float mass;
    protected float gravityScale;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mass = rigidbody.mass;
        gravityScale = rigidbody.gravityScale;
        origin = transform.position;
        normal.SetActive(true);
        destroyed.SetActive(false);
    }

    protected virtual void Update() 
    {
        if (!isDestroyed)
        {
            destroyed.transform.position = normal.transform.position;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isDestroyed)
        {
            GroundCheck();
            DeathCheck();
        }
    }

    public virtual void Pause() { }
    public virtual void Resume() { }
    public virtual void Destroy() 
    {
        normal.SetActive(false);
        destroyed.SetActive(true);
        rigidbody.mass = 0;
        rigidbody.gravityScale = 0;
    }
    public virtual void Restore() 
    {
        isDestroyed = false;
        normal.SetActive(true);
        destroyed.SetActive(false);
        rigidbody.mass = mass;
        rigidbody.gravityScale = gravityScale;

        if (shouldRestorePosition)
        {
            transform.position = origin;
        }
    }

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
            isDestroyed = deathCollider.IsTouching(new ContactFilter2D());
            if (isDestroyed)
            {
                Destroy();
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) 
    {
        //if (collision.collider.CompareTag("Platform"))
        //{
        //    transform.SetParent(collision.collider.transform);
        //}
    }

    protected virtual void OnCollisionStay2D(Collision2D collision) {}

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Platform"))
        //{
        //    transform.SetParent(null);
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckVector);
    }

}
