using System;
using System.Collections.Generic;
using UnityEngine;


public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private Transform          fallCheck;
    [SerializeField] private List<GameObject>   targets;
    [SerializeField] private int                max;
    [SerializeField] private float              waitTime;
    [SerializeField] private float              regenTime;
    [SerializeField] private Color              color = Color.white;
    private int                 total = 0;
    private bool                isReady = false;
    private List<GameObject>    objects = new List<GameObject>();
    private Transform           origin;

    private void Start()
    {
        origin = transform;
        Invoke(nameof(GetReady), waitTime);
    }

    private void Update()
    {
        if (total < max)
        {
            GenerateBox();
        }
        ManageBox();
    }

    private bool FallCheck(Vector2 vector)
    {
        return (vector.y < fallCheck.position.y);
    }

    private void GenerateBox()
    {
        if (isReady)
        {
            int random              = UnityEngine.Random.Range(0, targets.Count);
            GameObject target       = targets[random];
            GameObject obj          = Instantiate(target, origin.position, Quaternion.identity);
            SpriteRenderer sprite   = obj.GetComponentInChildren<SpriteRenderer>();
            sprite.color = color;
            objects.Add(obj);
            total++;
            isReady = false;
            Invoke(nameof(GetReady), regenTime);
        }
    }

    private void ManageBox()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject obj = objects[i];
            if (FallCheck(obj.transform.position)) RemoveBox(i);
        }
    }

    private void RemoveBox(int idx)
    {
        Destroy(objects[idx]);
        objects.RemoveAt(idx);
        total--;
    }

    private void GetReady()
    {
        isReady = true;
    }

    private void OnDrawGizmos()
    {
        Vector2 start = new Vector2(fallCheck.position.x - 5 , fallCheck.position.y);
        Vector2 end = new Vector2(fallCheck.position.x + 5, fallCheck.position.y);
        Gizmos.DrawLine(start, end);
    }
}