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
        currentCheckpoint = 0;
        StartCoroutine(transition.SceneFadeOut(0, 0, () => { }));
    }

}
