using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageManager : GameManager
{
    [Header("Texts")]
    [SerializeField] private GameObject text;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(TransitionOut());
        ShowObject(text, text.transform.position, 3);
    }

}
