using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{

    private int     stage;
    private int     enabledArms;
    private float   x;
    private float   y;

    public SaveData(int stage, int enabledArms, Vector3 position)
    {
        this.stage          = stage;
        this.enabledArms    = enabledArms;
        this.x              = position.x;
        this.y              = position.y;
    }

    public int GetStage() { return stage; }
    public int GetEnabledArms() { return enabledArms; }
    public Vector3 GetPosition() { return new Vector3(x, y, 0); }
}
