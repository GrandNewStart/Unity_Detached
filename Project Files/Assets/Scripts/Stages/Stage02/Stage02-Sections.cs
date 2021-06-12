using UnityEngine;

public partial class StageManager02
{
    private void ManageSections()
    {
        float playerX = player.transform.position.x;

        // at Section 01
        if (playerX < section02Point.position.x)
        {
            section01.SetActive(true);
            section02.SetActive(true);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(false);
            section07.SetActive(false);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 02
        if (playerX < section03Point.position.x)
        {
            section01.SetActive(true);
            section02.SetActive(true);
            section03.SetActive(true);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(false);
            section07.SetActive(false);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 03
        if (playerX < section04Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(true);
            section03.SetActive(true);
            section04.SetActive(true);
            section05.SetActive(false);
            section06.SetActive(false);
            section07.SetActive(false);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 04
        if (playerX < section05Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(true);
            section04.SetActive(true);
            section05.SetActive(true);
            section06.SetActive(false);
            section07.SetActive(false);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 05
        if (playerX < section06Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(true);
            section05.SetActive(true);
            section06.SetActive(true);
            section07.SetActive(false);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 06
        if (playerX < section07Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(true);
            section06.SetActive(true);
            section07.SetActive(true);
            section08.SetActive(false);
            section09.SetActive(false);
            return;
        }
        // at Section 07
        if (playerX < section08Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(true);
            section07.SetActive(true);
            section08.SetActive(true);
            section09.SetActive(false);
            return;
        }
        // at Section 08
        if (playerX < section08Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(false);
            section07.SetActive(true);
            section08.SetActive(true);
            section09.SetActive(true);
            return;
        }
    }
}