﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    public Boolean          isDestroyed;
    public GameObject       normalSprite;
    public GameObject       destroyedSprite;
    public new Rigidbody2D  rigidbody;
    private bool            destructionDetected = false;
    
    protected virtual void Start()
    {
        isDestroyed = false;
        rigidbody   = GetComponent<Rigidbody2D>();
        normalSprite    .SetActive(true);
        destroyedSprite .SetActive(false);
    }

    protected virtual void Update()
    {
        SpriteControl();
    }

    private void SpriteControl()
    {
        if (isDestroyed)
        {
            normalSprite.SetActive(false);
            destroyedSprite.SetActive(true);
        }
        else
        {
            normalSprite.SetActive(true);
            destroyedSprite.SetActive(false);
        }
    }

    public void MoveObject(int dir, float speed)
    {
        Vector2 newPosition     = transform.position;
        newPosition.x           += dir * speed * Time.deltaTime;
        transform.localPosition = newPosition;
    }

    public void DestroyObject()
    {
        isDestroyed = true;
        OnDestruction();
    }

    public void RecoverObject()
    {
        isDestroyed = false;
        OnRestoration();
    }

    protected virtual void OnDestruction() {}
    protected virtual void OnRestoration() {}
    public virtual void OnPause() {}
    public virtual void OnResume() {}

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.parent = collision.transform;
        }
        if (collision.collider.CompareTag("Crusher"))
        {
            DestroyObject();
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.parent = null;
        }
    }
}
