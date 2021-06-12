using UnityEngine;

public partial class StageManager02
{
    private void ManageEvents()
    {
        ManageTrapText();
        ManageMagnetText();
        ManageSection02Entrance();
        ManageSection07Exit();
        ManageSection08Entrance();
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


    private void ManageSection02Entrance()
    {
        float playerPosX = player.transform.position.x;
        float limitX = wareHouseStartPoint.transform.position.x;
        float diff = playerPosX - limitX;

        if (diff < 0)
        {
            section02Light1.intensity = 0;
            section02Light2.intensity = 0;
            section01Light.lightOrder = 1;
            section02Light1.lightOrder = 0;
            section02Light2.lightOrder = 0;
        }
        if (diff > 0 && diff < 8)
        {
            float alpha = (8 - diff) / 10;
            Color color = section02Walls.color;
            color.a = alpha;
            section02Walls.color = color;
            section02WallDecos.alpha = alpha;
            section01Light.intensity = alpha;
            section02Light1.intensity = 0.8f - alpha;
            section02Light2.intensity = 0.8f - alpha;
            section01Light.lightOrder = 0;
            section02Light1.lightOrder = 1;
            section02Light2.lightOrder = 1;
        }
        if (diff > 8)
        {
            Color color = section02Walls.color;
            color.a = 0;
            section02Walls.color = color;
            section02WallDecos.alpha = 0;
            section01Light.intensity = 0;
            section02Light1.intensity = 0.8f;
            section02Light2.intensity = 0.8f;
            section01Light.lightOrder = 0;
            section02Light1.lightOrder = 1;
            section02Light2.lightOrder = 1;
        }
    }

    private void ManageSection07Exit()
    {
        float playerPosX = player.transform.position.x;
        float limitX = wareHouseEndPoint.transform.position.x;
        float diff = playerPosX - limitX;

        if (diff < -8)
        {
            Color color = section07Walls.color;
            color.a = 0;
            section07Walls.color = color;
            section07Light1.intensity = 0.8f;
            section07Light1.lightOrder = 1;
            section07Light2.intensity = 0;
            section07Light2.lightOrder = 0;
        }
        if (diff > -8 && diff < 0)
        {
            float alpha = -1 * diff / 10;
            Color color = section07Walls.color;
            color.a = 0.8f - alpha;
            section07Walls.color = color;
            section07Light1.intensity = alpha;
            section07Light2.intensity = 0.8f - alpha;
            section07Light1.lightOrder = 1;
            section07Light2.lightOrder = 0;
        }
        if (diff > 0)
        {
            Color color = section07Walls.color;
            color.a = 1;
            section07Walls.color = color;
            section07Light1.intensity = 0;
            section07Light1.lightOrder = 0;
            section07Light2.intensity = 1;
            section07Light2.lightOrder = 1;
        }
    }

    private void ManageSection08Entrance()
    {
        float playerPosX = player.transform.position.x;
        float limitX = factoryStartPoint.transform.position.x;
        float diff = playerPosX - limitX;

        if (diff < 0)
        {
            section07Light4.intensity = 1;
            section07Light4.lightOrder = 1;
            section08Light.intensity = 0;
            section08Light.lightOrder = 0;
            Color color = section08Walls.color;
            color.a = 1;
            section08Walls.color = color;
            section08WallDecos.alpha = 1;
        }
        if (diff > 0 && diff < 8)
        {
            float alpha = (8 - diff) / 10;
            Color color = section08Walls.color;
            color.a = alpha;
            section08Walls.color = color;
            section08WallDecos.alpha = alpha;
            section07Light4.lightOrder = 0;
            section08Light.intensity = alpha;
            section08Light.lightOrder = 1;
        }
        if (diff > 8)
        {
            section07Light4.intensity = 0;
            section07Light4.lightOrder = 0;
            section08Light.intensity = 0.8f;
            section08Light.lightOrder = 1;
            Color color = section08Walls.color;
            color.a = 0;
            section08Walls.color = color;
            section08WallDecos.alpha = 0;
        }
    }

}