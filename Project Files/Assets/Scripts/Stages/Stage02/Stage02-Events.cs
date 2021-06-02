using UnityEngine;

public partial class StageManager02
{
    private void ManageEvents()
    {
        ManageTrapText();
        ManageMagnetText();
        ManageSection02Color();
        ManageSection07Color();
        ManageSection08Color();
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
    private void ManageSection02Color()
    {
        Vector2 playerPos = player.transform.position;
        float dist = playerPos.x - wareHouseStartPoint.position.x;

        Color original1 = new Color(255, 192, 143, 255);
        Color original2 = new Color(212, 156, 113, 255);
        Color target1 = new Color(215, 227, 255, 255);
        Color target2 = new Color(149, 171, 221, 255);
        Color frontColor = section02Front3.color;
        Color middleColor = original1;
        Color backColor = original2;
        float r1, r2;
        float g1, g2;
        float b1, b2;
        float alpha;

        // Before entrance
        if (dist < 0)
        {
            alpha = 1;
            middleColor = original1;
            backColor = original2;
        }
        // During entrance
        else if (dist > 0 && dist < 10)
        {
            alpha = 1 - (dist * 0.1f);
            r1 = original1.r - (dist * 4);
            g1 = original1.g + (dist * 3.5f);
            b1 = original1.b + (dist * 11.2f);
            r2 = original2.r - (dist * 6.3f);
            g2 = original2.g + (dist * 1.5f);
            b2 = original2.b + (dist * 10.8f);
            middleColor.r = r1;
            middleColor.g = g1;
            middleColor.b = b1;
            backColor.r = r2;
            backColor.g = g2;
            backColor.b = b2;
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

        section02Front4.alpha = alpha;
        section02Back2.color = backColor;
        section02Back1.color = middleColor;
        section02Ground.color = middleColor;
        section02Traps.color = middleColor;
        section02Front1.color = middleColor;
        section02Front2.color = middleColor;
        section02Front3.color = frontColor;
        player.SetColor(middleColor);
        firstArm.SetColor(middleColor);
        secondArm.SetColor(middleColor);

    }

    private void ManageSection07Color()
    {
        Vector2 playerPos = player.transform.position;
        float dist = playerPos.x - wareHouseEndPoint.position.x;

        Color original1 = new Color(255, 192, 143, 255);
        Color original2 = new Color(212, 156, 113, 255);
        Color target1   = new Color(215, 227, 255, 255);
        Color target2   = new Color(149, 171, 221, 255);
        Color frontColor;
        Color middleColor   = original1;
        Color backColor     = original2;
        float r1, r2;
        float g1, g2;
        float b1, b2;
        float alpha;

        // Before leaving warehouse
        if (dist < -10)
        {
            alpha = 0;
            middleColor = target1;
            backColor = target2;
        }
        // During leaving
        else if (dist >= -10 && dist < 0)
        {
            alpha = 1 + (dist * 0.1f);
            r1 = target1.r + ((dist + 10) * 4);
            g1 = target1.g - ((dist + 10) * 3.5f);
            b1 = target1.b - ((dist + 10) * 11.2f);
            r2 = target2.r + ((dist + 10) * 6.3f);
            g2 = target2.g - ((dist + 10) * 1.5f);
            b2 = target2.b - ((dist + 10) * 10.8f);
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

        middleColor /= 255;
        backColor /= 255;
        frontColor = middleColor;
        frontColor.a = alpha;

        section07Back2.color = backColor;
        section07Back1.color = middleColor;
        section07Ground.color = middleColor;
        section07Front1.color = middleColor;
        section07Front2.color = frontColor;
        player.SetColor(middleColor);
        firstArm.SetColor(middleColor);
        secondArm.SetColor(middleColor);
    }

    // Original1 color = (255, 192, 143, 255)
    // Original2 color = (212, 156, 113, 255)
    // Target1 color = (255, 217, 215, 255)
    // Target2 color = (212, 169, 167, 255)
    private void ManageSection08Color()
    {
        Vector2 playerPos = player.transform.position;
        float dist = playerPos.x - factoryStartPoint.position.x;

        if (dist < -10) return;

        Color original1 = new Color(255, 192, 143, 255);
        Color original2 = new Color(212, 156, 113, 255);
        Color target1 = new Color(255, 217, 215, 255);
        Color target2 = new Color(212, 169, 167, 255);
        Color frontColor = section09Front3.color;
        Color middleColor = original1;
        Color backColor = original2;
        float r1, r2;
        float g1, g2;
        float b1, b2;
        float alpha;

        if (dist < 10)
        {
            // Before enterance
            if (dist < 0)
            {
                alpha = 1;
                middleColor = original1;
                backColor = original2;
            }
            // During entrance
            else
            {
                alpha = 1 - (dist * 0.1f);
                r1 = original1.r + (dist * 0);
                g1 = original1.g + (dist * 2.5f);
                b1 = original1.b + (dist * 7.2f);
                r2 = original2.r + (dist * 0);
                g2 = original2.g + (dist * 1.3f);
                b2 = original2.b + (dist * 5.4f);
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
        section09Front4.alpha = alpha;
        section09Front3.color = frontColor;
        section09Back2.color = backColor;
        section09Back1.color = middleColor;
        section09Ground.color = middleColor;
        section09Front1.color = middleColor;
        player.SetColor(middleColor);
        firstArm.SetColor(middleColor);
        secondArm.SetColor(middleColor);
    }

}