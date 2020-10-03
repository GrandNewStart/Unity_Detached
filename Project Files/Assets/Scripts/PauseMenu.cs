using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] GameObject[] arrows = null;
    PlayerController playerController;
    int menuState;

    // Start is called before the first frame update
    void Start()
    {
        menuState = 0;
        arrows[0].SetActive(true);
        arrows[1].SetActive(false);
        arrows[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!GameManager.isPause)
            {
                GameManager.isPause = true;
                pauseMenu.SetActive(true);
                //playerController.SetControlling(false);

                Time.timeScale = 0f;
            }
            else
            {
                GameManager.isPause = false;
                pauseMenu.SetActive(false);
                //playerController.SetControlling(true);

                Time.timeScale = 1f;
            }
        }

        SelectMenu();
        GoMenu();
        
    }

    void SelectMenu()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int preState = menuState;
           
            menuState = (menuState + 2) % 3;
            arrows[preState].SetActive(false);
            arrows[menuState].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int preState = menuState;
            menuState = (menuState + 1) % 3;
            arrows[preState].SetActive(false);
            arrows[menuState].SetActive(true);
        }
    }

    void GoMenu()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (menuState)
            {
                case 0:
                    ClickResume();
                    break;
                case 1:
                    ClickQuit();
                    break;
                case 2:
                    ClickSettings();
                    break;
            }
        }
    }

    public void ClickResume()
    {
        Debug.Log("다시 시작");
    }

    public void ClickQuit()
    {
        Debug.Log("게임 종료");
    }

    public void ClickSettings()
    {
        Debug.Log("설정");
    }
}
