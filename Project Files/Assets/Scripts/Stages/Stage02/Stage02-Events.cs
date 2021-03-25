using UnityEngine;

public partial class StageManager02
{
    private void ManageEvents()
    {
        ManageTrapText();
        ManageMagnetText();
    }

    private void ManageTrapText()
    {
        Vector3 trapText1Start  = trapText1StartPoint.position;
        Vector3 trapText1End    = trapText1EndPoint.position;
        Vector3 trapText2Start  = trapText2StartPoint.position;
        Vector3 trapText2End    = trapText2EndPoint.position;
        Vector3 playerPos       = player.transform.position;

        if (trapText1Start.x < playerPos.x &&
            trapText1End.x > playerPos.x &&
            text_trap_1.color.a == 0)
        {
            Show(text_trap_1, null);
        }

        if (trapText1Start.x > playerPos.x &&
            text_trap_1.color.a == 1)
        {
            Hide(text_trap_1, null);
        }

        if (trapText1End.x < playerPos.x &&
            text_trap_1.color.a == 1)
        {
            Hide(text_trap_1, null);
        }

        if (trapText2Start.x < playerPos.x &&
            trapText2End.x > playerPos.x &&
            text_trap_2.color.a == 0)
        {
            Show(text_trap_2, null);
        }

        if (trapText2Start.x > playerPos.x &&
            text_trap_2.color.a == 1)
        {
            Hide(text_trap_2, null);
        }

        if (trapText2End.x < playerPos.x &&
            text_trap_2.color.a == 1)
        {
            Hide(text_trap_2, null);
        }
    }

    private void ManageMagnetText()
    {
        if (firstMagnet.IsPluggedIn() &&
            text_magnet.color.a == 0)
        {
            Show(text_magnet, null);
        }
        
        if (!firstMagnet.IsPluggedIn() &&
            text_magnet.color.a == 1)
        {
            Hide(text_magnet, null);
        }
    }

}
