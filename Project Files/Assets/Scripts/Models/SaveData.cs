using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{

    private int     stage;
    private int     index;
    private int     enabledArms;
    private float   x;
    private float   y;

    public SaveData(int stage, int index, int enabledArms, Vector3 position)
    {
        this.stage          = stage;
        this.index          = index;
        this.enabledArms    = enabledArms;
        this.x              = position.x;
        this.y              = position.y;
    }

    public int GetStage() { return stage; }
    public int GetIndex() { return index;  }
    public int GetEnabledArms() { return enabledArms; }
    public Vector3 GetPosition() { return new Vector3(x, y, 0); }
}
