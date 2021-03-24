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
                        if (!firstArm.isRetrieving)
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        break;
                }
                break;
            case 0:
                switch (player.enabledArms)
                {
                    case 1:
                        if (!firstArm.isRetrieving)
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        break;
                    case 2:
                        if (!firstArm.isRetrieving)
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        if (!secondArm.isRetrieving)
                        {
                            player.PlayRetrieveSound();
                            RetrieveRightArm();
                        }
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