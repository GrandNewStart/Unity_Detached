using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    public void RetrieveArms()
    {
        switch (player.GetArms())
        {
            case 1:
                switch (player.GetEnabledArms())
                {
                    case 1:
                        break;
                    case 2:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        break;
                }
                break;
            case 0:
                switch (player.GetEnabledArms())
                {
                    case 1:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        break;
                    case 2:
                        if (!player.IsLeftRetrieving())
                        {
                            player.PlayRetrieveSound();
                            RetrieveLeftArm();
                        }
                        if (!player.IsRightRetrieving())
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
        player.SetLeftRetrieving(true);
        firstArm.StartRetrieve();
    }

    public void RetrieveRightArm()
    {
        player.SetRightRetrieving(true);
        secondArm.StartRetrieve();
    }
}