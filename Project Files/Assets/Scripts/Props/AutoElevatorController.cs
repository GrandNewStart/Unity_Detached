using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevatorController : MonoBehaviour
{
    public bool isVertical;
    public float speed;

    public GameObject upperBound;
    public GameObject lowerBound;

    public GameObject elevator;

    private bool dir; // elevator의 이동방향, 양수일경우 위/오른쪽
    private bool activated;

    private void Start()
    {
        dir = true;
        activated = true;
    }

    private void Update()
    {
        ActivateElevator();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 

        if (collision.gameObject.Equals(upperBound))
        {
            dir = false;
        }
        else if (collision.gameObject.Equals(lowerBound))
        {
            dir = true;
        }
    }


    private void ActivateElevator()
    {
        if (activated)
        {
            float dx, dy;

            if (isVertical)
            {
                dx = 0;
                if (dir) dy = speed;
                else dy = -1 * speed;
            }
            else
            {
                dy = 0;
                if (dir) dx = speed;
                else dx = -1 * speed;
            }

            elevator.transform.Translate(new Vector2(dx * Time.deltaTime, dy * Time.deltaTime));

        }
    }

}