using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    public void RetrieveArms()
    {
        switch (player.arms)
        {
            case 1:
                switch (player.enabledArms)
                {
                    case 1:
                        break;
                    case 2:
                        firstArm.StartRetrieve();
                        break;
                }
                break;
            case 0:
                switch (player.enabledArms)
                {
                    case 1:
                        firstArm.StartRetrieve();
                        break;
                    case 2:
                        firstArm.StartRetrieve();
                        secondArm.StartRetrieve();
                        break;
                }
                break;
        }
    }

    public void RetrieveLeftArm()
    {
        firstArm.StartRetrieve();
    }

    public void RetrieveRightArm()
    {
        secondArm.StartRetrieve();
    }
}