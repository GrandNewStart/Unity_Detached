using UnityEngine;

public partial class StageManager01
{
    private void ManageSections()
    {
        float camX = camera.transform.position.x;

        bool _inSection01 = (camX < section01Point.position.x + section01Size.x / 2);
        bool _inSection02 = (camX > section02Point.position.x - section02Size.x / 2 &&
                            camX < section02Point.position.x + section02Size.x / 2);
        bool _inSection03 = (camX > section03Point.position.x - section03Size.x / 2 &&
                            camX < section03Point.position.x + section03Size.x / 2);
        bool _inSection04 = (camX > section04Point.position.x - section04Size.x / 2 &&
                            camX < section04Point.position.x + section04Size.x / 2);
        bool _inSection05 = (camX > section05Point.position.x - section05Size.x / 2 &&
                            camX < section05Point.position.x + section05Size.x / 2);

        if (inSection01 != _inSection01)
        {
            section01.SetActive(_inSection01);
            inSection01 = _inSection01;
        }
        if (inSection02 != _inSection02)
        {
            section02.SetActive(_inSection02);
            inSection02 = _inSection02;
        }
        if (inSection03 != _inSection03)
        {
            section03.SetActive(_inSection03);
            inSection03 = _inSection03;
        }
        if (inSection04 != _inSection04)
        {
            section04.SetActive(_inSection04);
            inSection04 = _inSection04;
        }
        if (inSection05 != _inSection05)
        {
            section05.SetActive(_inSection05);
            inSection05 = _inSection05;
        }
    }

}