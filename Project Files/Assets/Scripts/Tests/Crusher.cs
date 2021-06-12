using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    [SerializeField] private GameManager2 gameManager;
    [SerializeField] private GameObject crusher;
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    [SerializeField] private float depth;
    [SerializeField] private float upSpeed;
    [SerializeField] private float downSpeed;
    [SerializeField] private int wait;
    private CrusherState state = CrusherState.up;
    private bool isActivated = false;
    private Vector2 topPoint;
    private Vector2 bottomPoint;
    private Vector2 position;

    private void Awake()
    {
        topPoint = top.transform.position;
        bottomPoint = bottom.transform.position;
        isActivated = true;
    }

    private void Start()
    {

    }

    private void Update()
    {
        OperateCrusher();
    }

    public void Pause()
    {
        isActivated = false;
    }

    public void Resume()
    {
        isActivated = true;
    }

    private void OperateCrusher()
    {
        position = crusher.transform.position;

        if (isActivated)
        {
            switch (state)
            {
                case CrusherState.up:
                    GoUp();
                    break;
                case CrusherState.down:
                    GoDown();
                    break;
            }
        }
    }

    private void GoUp()
    {
        if (position.y < topPoint.y)
        {
            crusher.transform.Translate(new Vector2(0, upSpeed * Time.deltaTime));
            if (position.y > topPoint.y)
            {
                crusher.transform.position = topPoint;
            }
        }
        else
        {
            state = CrusherState.wait;
            Invoke(nameof(StopWait), wait);
        }
    }

    private void GoDown()
    {
        if (position.y > bottomPoint.y)
        {
            crusher.transform.Translate(new Vector2(0, -1 * downSpeed * Time.deltaTime));
            if (position.y < bottomPoint.y)
            {
                crusher.transform.position = bottomPoint;
                state = CrusherState.up;
            }
        }
        else
        {
            state = CrusherState.up;
        }
    }

    private void StopWait()
    {
        state = CrusherState.down;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(top.transform.position, .3f);
        Gizmos.DrawLine(top.transform.position, bottom.transform.position);
        Gizmos.DrawWireSphere(bottom.transform.position, .3f);
    }
}
