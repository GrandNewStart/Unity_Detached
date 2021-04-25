using UnityEngine;
using UnityEngine.Tilemaps;

public partial class StageManager02
{
    private void ManageEvents()
    {
        ManageTrapText();
        ManageMagnetText();
        ManageWareHouseTiles();
        ManageFactoryTiles();
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

    // Original1 color = (255, 192, 143, 255)
    // Original2 color = (212, 156, 113, 255)
    // Target1 color = (215, 227, 255, 255)
    // Target2 color = (149, 171, 221, 255)
    private void ManageWareHouseTiles()
    {
        Vector2 playerPos = player.transform.position;
        float distA = playerPos.x - wareHouseStartPoint.position.x;
        float distB = playerPos.x - wareHouseEndPoint.position.x;

        Color original1 = new Color(255, 192, 143, 255);
        Color original2 = new Color(212, 156, 113, 255);
        Color target1 = new Color(215, 227, 255, 255);
        Color target2 = new Color(149, 171, 221, 255);
        Color frontColor = wareHouseFront3.color;
        Color middleColor = original1;
        Color backColor = original2;
        float r1, r2;
        float g1, g2;
        float b1, b2;
        float alpha;

        if (distA < 10)
        {
            // Before entering warehouse
            if (distA < 0)
            {
                alpha = 1;
                middleColor = original1;
                backColor = original2;
            }
            // During entrance
            else
            {
                alpha = 1 - (distA * 0.1f);
                r1 = original1.r - (distA * 4);
                g1 = original1.g + (distA * 3.5f);
                b1 = original1.b + (distA * 11.2f);
                r2 = original2.r - (distA * 6.3f);
                g2 = original2.g + (distA * 1.5f);
                b2 = original2.b + (distA * 10.8f);
                middleColor.r = r1;
                middleColor.g = g1;
                middleColor.b = b1;
                backColor.r = r2;
                backColor.g = g2;
                backColor.b = b2;
            }
        }
        // After entrance
        else
        {
            // Before leaving warehouse
            if (distB < -10)
            {
                alpha = 0;
                middleColor = target1;
                backColor = target2;
            }
            // During leaving
            else if (distB >= -10 && distB < 0)
            {
                alpha = 1 + (distB * 0.1f);
                r1 = target1.r + ((distB + 10) * 4);
                g1 = target1.g - ((distB + 10) * 3.5f);
                b1 = target1.b - ((distB + 10) * 11.2f);
                r2 = target2.r + ((distB + 10) * 6.3f);
                g2 = target2.g - ((distB + 10) * 1.5f);
                b2 = target2.b - ((distB + 10) * 10.8f);
                middleColor.r = r1;
                middleColor.g = g1;
                middleColor.b = b1;
                backColor.r = r2;
                backColor.g = g2;
                backColor.b = b2;
            }
            // After leaving
            else
            {
                alpha = 1;
                middleColor = original1;
                backColor = original2;
            }
        }


        middleColor /= 255;
        backColor /= 255;
        frontColor.a = alpha;

        wareHouseBack2.color = backColor;
        wareHouseBack1.color = middleColor;
        wareHouseGround.color = middleColor;
        wareHouseTraps.color = middleColor;
        wareHouseFront1.color = middleColor;
        wareHouseFront3.color = middleColor;
        wareHouseFront3.color = frontColor;
        player.SetColor(middleColor);
        firstArm.SetColor(middleColor);
        secondArm.SetColor(middleColor);
    }

    // Original1 color = (255, 192, 143, 255)
    // Original2 color = (212, 156, 113, 255)
    // Target1 color = (255, 217, 215, 255)
    // Target2 color = (212, 169, 167, 255)
    private void ManageFactoryTiles()
    {
        Vector2 playerPos = player.transform.position;
        float distA = playerPos.x - factoryStartPoint.position.x;

        if (distA < -10) return;

        Color original1 = new Color(255, 192, 143, 255);
        Color original2 = new Color(212, 156, 113, 255);
        Color target1 = new Color(255, 217, 215, 255);
        Color target2 = new Color(212, 169, 167, 255);
        Color frontColor = factoryFront3.color;
        Color middleColor = original1;
        Color backColor = original2;
        float r1, r2;
        float g1, g2;
        float b1, b2;
        float alpha;

        if (distA < 10)
        {
            // Before entering factory
            if (distA < 0)
            {
                alpha = 1;
                middleColor = original1;
                backColor = original2;
            }
            // During entrance
            else
            {
                alpha = 1 - (distA * 0.1f);
                r1 = original1.r + (distA * 0);
                g1 = original1.g + (distA * 2.5f);
                b1 = original1.b + (distA * 7.2f);
                r2 = original2.r + (distA * 0);
                g2 = original2.g + (distA * 1.3f);
                b2 = original2.b + (distA * 5.4f);
                middleColor.r = r1;
                middleColor.g = g1;
                middleColor.b = b1;
                backColor.r = r2;
                backColor.g = g2;
                backColor.b = b2;
            }
        }
        // After entrance
        else
        {
            alpha = 0;
            middleColor = target1;
            backColor = target2;
        }

        middleColor /= 255;
        backColor /= 255;

        frontColor.a = alpha;
        factoryFront3.color = frontColor;
        factoryBack2.color = backColor;
        factoryBack1.color = middleColor;
        factoryGround.color = middleColor;
        factoryFront1.color = middleColor;
        factoryFront2.color = middleColor;
        player.SetColor(middleColor);
        firstArm.SetColor(middleColor);
        secondArm.SetColor(middleColor);
    }

}