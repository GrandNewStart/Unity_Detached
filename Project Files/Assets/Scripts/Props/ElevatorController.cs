using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{ 
    public bool isVertical;
    public float speed;

    public GameObject upperBound;
    public GameObject lowerBound;

    public GameObject elevator;
    public ElevatorSwitchController elevatorController;

    private bool dir; // elevator의 이동방향, 양수일경우 위/오른쪽
    private bool activated;

    private void Start()
    {
        dir = true;
        activated = false;  
    }

    private void Update()
    {
        SwitchCheck();
        ActivateElevator();
    }

    private void SwitchCheck()
    {
        
            if (elevatorController.getToggled())
            {
                if (dir)
                    dir = false;
                else dir = true;

                if (!activated)
                    activated = true;

                elevatorController.setToggled(false);
            }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.Equals(upperBound))
        {
  
            activated = false;
        }
        else if (collision.gameObject.Equals(lowerBound))
        {
            activated = false;
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