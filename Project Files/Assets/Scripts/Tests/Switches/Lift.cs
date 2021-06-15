using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : BaseSwitch
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject topPoint;
    [SerializeField] private GameObject bottomPoint;
    [SerializeField] private float speed;
    private Vector2 top;
    private Vector2 bottom;

    protected override void Start()
    {
        base.Start();
        
        top     = topPoint.transform.position;
        top.y   -= 1;
        top.x   = platform.transform.position.x;

        bottom      = bottomPoint.transform.position;
        bottom.y    -= 1;
        bottom.x    = platform.transform.position.x;
    }

    public override void PlugIn(Arm arm)
    {
        base.PlugIn(arm);
        sprite.sprite = sprite_plugged_green;
        StartCoroutine(RaisePlatform());
    }

    public override void PlugOut()
    {
        base.PlugOut();
        sprite.sprite = sprite_unplugged;
        StartCoroutine(LowerPlatform());
    }

    private IEnumerator RaisePlatform()
    {
        while (isPlugged)
        {
            platform.transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (platform.transform.position.y >= top.y)
            {
                platform.transform.position = top;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator LowerPlatform()
    {
        while (!isPlugged)
        {
            platform.transform.Translate(Vector2.up * 2 * -speed * Time.deltaTime);
            if (platform.transform.position.y <= bottom.y)
            {
                platform.transform.position = bottom;
                break;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(topPoint.transform.position, .3f);
        Gizmos.DrawWireSphere(bottomPoint.transform.position, .3f);
        Gizmos.DrawLine(bottomPoint.transform.position, topPoint.transform.position);
    }
}
