using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public GameObject   indicator;
    private int         selectedMenu = 1;
    private enum        focus { main, noSaveData, saveDataExists, settings, quit, none };
    private focus       focusStatus = focus.main;

    [Header("No Save Data")]
    public GameObject   noSaveDataDialog;
    public GameObject   menu_1;
    public GameObject   menu_1_yes;
    public GameObject   menu_1_no;
    public GameObject   menu_1_background;
    private bool        menu_1_affirmative = false;

    [Header("Save Data Exists")]
    public GameObject   saveDataExistsDialog;
    public GameObject   menu_2;
    public GameObject   menu_2_yes;
    public GameObject   menu_2_no;
    private bool        menu_2_affirmative = false;

    [Header("Settings")]
    public GameObject   settingsDialog;
    public GameObject   menu_3;

    [Header("Quit")]
    public GameObject   quitDialog;
    public GameObject   menu_4;
    public GameObject   menu_4_yes;
    public GameObject   menu_4_no;
    private bool        menu_4_affirmative = false;

    [Header("Loading Screens")]
    public GameObject loading_1;
    private StageLoader stageLoader1;

    void Start()
    {
        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Dialogs
        noSaveDataDialog    .SetActive(false);
        saveDataExistsDialog.SetActive(false);
        settingsDialog      .SetActive(false);
        quitDialog          .SetActive(false);

        // Indicator
        Vector3 origin = menu_1.transform.position;
        origin.x += 2.5f;
        indicator.transform.position = origin;

        stageLoader1 = loading_1.GetComponent<StageLoader>();
    }

    void Update()
    {
        switch (focusStatus)
        {
            case focus.main:
                MainControl();
                MainEnterKey();
                break;
            case focus.noSaveData:
                NoSaveDataControl();
                NoSaveDataEnterKey();
                break;
            case focus.saveDataExists:
                SaveDataExistsControl();
                SaveDataExistsEnterKey();
                break;
            case focus.settings:
                SettingsControl();
                break;
            case focus.quit:
                QuitControl();
                QuitEnterKey();
                break;
        }
        
    }

    private void MainControl()
    {
        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            MoveIndicator(-1);
        }
        if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
        {
            MoveIndicator(1);
        }
    }

    private void MoveIndicator(int dir)
    {
        Vector3 position;
        if (dir == 1)
        {
            switch (selectedMenu)
            {
                case 1:
                    break;
                case 2:
                    position = menu_1.transform.position;
                    position.x += 2.5f;
                    indicator.transform.position = position;
                    selectedMenu = 1;
                    break;
                case 3:
                    position = menu_2.transform.position;
                    position.x += 2.5f;
                    indicator.transform.position = position;
                    selectedMenu = 2;
                    break;
                case 4:
                    position = menu_3.transform.position;
                    position.x += 2.5f;
                    indicator.transform.position = position;
                    selectedMenu = 3;
                    break;
            }
        }
        if (dir == -1)
        {
            if (selectedMenu != 4)
            {
                switch (selectedMenu)
                {
                    case 1:
                        position = menu_2.transform.position;
                        position.x += 2.5f;
                        indicator.transform.position = position;
                        selectedMenu = 2;
                        break;
                    case 2:
                        position = menu_3.transform.position;
                        position.x += 2.5f;
                        indicator.transform.position = position;
                        selectedMenu = 3;
                        break;
                    case 3:
                        position = menu_4.transform.position;
                        position.x += 2.5f;
                        indicator.transform.position = position;
                        selectedMenu = 4;
                        break;
                    case 4:
                        break;
                }
            }
        }
    }

    private void MainEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            switch (selectedMenu)
            {
                case 1:
                    NewGame();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    Settings();
                    break;
                case 4:
                    Quit();
                    break;
            }
        }
    }

    private void NewGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            loading_1.SetActive(true);
            stageLoader1.LoadStage(1, true);
        }
        else
        {
            ToggleButton(menu_2_yes, false);
            ToggleButton(menu_2_no, true);
            saveDataExistsDialog.SetActive(true);
            focusStatus = focus.saveDataExists;
        }
    }

    private void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            ToggleButton(menu_1_yes, false);
            ToggleButton(menu_1_no, true);
            menu_1_affirmative = false;
            noSaveDataDialog.SetActive(true);
            focusStatus = focus.noSaveData;
        }
        else
        {
            GameManager.stage               = data.GetStage();
            GameManager.enabledArms         = data.GetEnabledArms();
            GameManager.position            = data.GetPosition();
            GameManager.shouldLoadSaveFile  = true;
            loading_1.SetActive(true);
            stageLoader1.LoadStage(GameManager.stage, false);
        }
    }

    private void Settings()
    {
        settingsDialog.SetActive(true);
        focusStatus = focus.settings;
    }
    
    private void Quit()
    {
        ToggleButton(menu_4_yes, false);
        ToggleButton(menu_4_no, true);
        menu_4_affirmative = false;
        quitDialog.SetActive(true);
        focusStatus = focus.quit;
    }

    private void NoSaveDataControl()
    {
        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            if (menu_1_affirmative) return;

            menu_1_affirmative = true;
            ToggleButton(menu_1_yes, true);
            ToggleButton(menu_1_no, false);
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            if (!menu_1_affirmative) return;

            menu_1_affirmative = false;
            ToggleButton(menu_1_yes, false);
            ToggleButton(menu_1_no, true);
        }
        if (Input.GetKeyDown("escape"))
        {
            focusStatus = focus.main;
            noSaveDataDialog.SetActive(false);
        }
    }

    private void NoSaveDataEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            if (menu_1_affirmative)
            {
                focusStatus = focus.none;
                loading_1.SetActive(true);
                stageLoader1.LoadStage(SaveSystem.initialStage, true);
            }
            else
            {
                focusStatus = focus.main;
            }
            noSaveDataDialog.SetActive(false);
        }
    }

    private void SaveDataExistsControl()
    {
        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            if (menu_2_affirmative) return;

            menu_2_affirmative = true;
            ToggleButton(menu_2_yes, true);
            ToggleButton(menu_2_no, false);
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            if (!menu_2_affirmative) return;

            menu_2_affirmative = false;
            ToggleButton(menu_2_yes, false);
            ToggleButton(menu_2_no, true);
        }
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("EXIT");
            focusStatus = focus.main;
            saveDataExistsDialog.SetActive(false);
        }
    }

    private void SaveDataExistsEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            if (menu_2_affirmative)
            {
                focusStatus = focus.none;
                SaveSystem.DeleteSaveFile();
                GameManager.stage               = SaveSystem.initialStage;
                GameManager.enabledArms         = SaveSystem.initialEnabledArms;
                GameManager.position            = SaveSystem.initialPosition;
                GameManager.shouldLoadSaveFile  = false;
                loading_1.SetActive(true);
                stageLoader1.LoadStage(GameManager.stage, false);
            }
            else
            {
                focusStatus = focus.main;
            }
            saveDataExistsDialog.SetActive(false);
        }
    }

    private void SettingsControl()
    {
        if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("UP");
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("DOWN");
        }
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("EXIT");
            focusStatus = focus.main;
            settingsDialog.SetActive(false);
        }
    }

    private void QuitControl()
    {
        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            if (menu_4_affirmative) return;

            menu_4_affirmative = true;
            ToggleButton(menu_4_yes, true);
            ToggleButton(menu_4_no, false);
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            if (!menu_4_affirmative) return;

            menu_4_affirmative = false;
            ToggleButton(menu_4_yes, false);
            ToggleButton(menu_4_no, true);
        }
        if (Input.GetKeyDown("escape"))
        {
            focusStatus = focus.main;
            quitDialog.SetActive(false);
        }
    }

    private void QuitEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            if (menu_4_affirmative)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
            else
            {
                focusStatus = focus.main;
                quitDialog.SetActive(false);
            }
        }
    }

    private void ToggleButton(GameObject button, bool on)
    {
        if (on)
        { button.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f); }
        else
        { button.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f); }
    }
}
