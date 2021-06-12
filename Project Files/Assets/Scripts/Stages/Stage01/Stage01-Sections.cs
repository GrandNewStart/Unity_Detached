using UnityEngine;

public partial class StageManager01
{
    private void ManageSections()
    {
        float camX = camera.transform.position.x;
        
        // at Section 01
        if (camX < section02Point.position.x)
        {
            section01.SetActive(true);
            section02.SetActive(true);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(false);
            return;
        }
        // at Section 02
        if (camX < section03Point.position.x)
        {
            section01.SetActive(true);
            section02.SetActive(true);
            section03.SetActive(true);
            section04.SetActive(false);
            section05.SetActive(false);
            section06.SetActive(false);
            return;
        }
        // at Section 03
        if (camX < section04Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(true);
            section03.SetActive(true);
            section04.SetActive(true);
            section05.SetActive(false);
            section06.SetActive(false);
            return;
        }
        // at Section 04
        if (camX < section05Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(true);
            section04.SetActive(true);
            section05.SetActive(true);
            section06.SetActive(false);
            return;
        }
        // at Section 05
        if (camX < section06Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(true);
            section05.SetActive(true);
            section06.SetActive(true);
            return;
        }
        // at Section 06
        if (camX > section06Point.position.x)
        {
            section01.SetActive(false);
            section02.SetActive(false);
            section03.SetActive(false);
            section04.SetActive(false);
            section05.SetActive(true);
            section06.SetActive(true);
            return;
        }
    }

}