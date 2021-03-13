using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public partial class GameManager : MonoBehaviour
{
    public static bool      isLoadingSaveData = false;
    public static int       stage;
    public static int       enabledArms;
    public static int       currentCheckpoint;
    public static Vector3   position;

    public const int        INFINITE    = 0;
    public const int        PLAYER      = 0;
    public const int        FIRST_ARM   = 1;
    public const int        SECOND_ARM  = 2;
    public const int        UI          = 3;
    public const int        CONVERSATION = 4;
    public const int        DISABLED    = -1;
    public const int        PAUSE       = 0;
    public const int        SETTINGS    = 1;
    public const int        TUTORIALS   = 2;
    
    public bool                 isPaused;
    public GameObject           cube;
    public Image                mask;
    public PlayerController     player;
    public ArmController        firstArm;
    public ArmController        secondArm;
    public new Camera           camera;
    public AudioSource          pageSound;
    public AudioSource          clickSound;
    public AudioSource          bgm;
    public Sprite               checkbox_checked;
    public Sprite               checkbox_unchecked;
    public TMP_FontAsset        font_english;
    public TMP_FontAsset        font_korean;
    public List<DoorSwitchController> doors = new List<DoorSwitchController>();
    public List<LiftSwitchController> lifts = new List<LiftSwitchController>();
    public List<CrusherController> crushers = new List<CrusherController>();
    public List<TreadmillController> treadmills = new List<TreadmillController>();
    public List<TelescopeController> telescopes = new List<TelescopeController>();
    public Transform            cameraTarget;
    private bool                cameraMoving        = false;
    public int                  controlIndex        = 0;
    private int                 tempControlIndex    = 0;
    private float               cameraSpeed         = 130;
    private int                 menuIndex           = 0;

    [Header("Game Settings")]
    private MenuController  settings_controller;
    public GameObject       settings_screen;
    public TextMeshProUGUI  settings_title;
    public TextMeshProUGUI  settings_full_screen;
    public TextMeshProUGUI  settings_windowed;
    public TextMeshProUGUI  settings_resolution;
    public TextMeshProUGUI  settings_resolution_value;
    public TextMeshProUGUI  settings_master_volume;
    public TextMeshProUGUI  settings_master_volume_value;
    public TextMeshProUGUI  settings_music_volume;
    public TextMeshProUGUI  settings_music_volume_value;
    public TextMeshProUGUI  settings_game_volume;
    public TextMeshProUGUI  settings_game_volume_value;
    public TextMeshProUGUI  settings_language;
    public TextMeshProUGUI  settings_language_value;
    public TextMeshProUGUI  settings_apply;
    public TextMeshProUGUI  settings_back;
    public Image            settings_full_screen_checkbox;
    public Image            settings_windowed_checkbox;
    public Slider           settings_master_volume_slider;
    public Slider           settings_music_volume_slider;
    public Slider           settings_game_volume_slider;
    private bool    tempIsFullScreen    = false;
    private int     tempResolution      = GameSettings.FHD;
    private int     tempLanguage        = GameSettings.ENGLISH;
    private float   tempMasterVolume    = 0.5f;
    private float   tempMusicVolume     = 0.5f;
    private float   tempGameVolume      = 0.5f;

    [Header("Conversation")]
    protected List<LineNode> conversations = new List<LineNode>();
    public CanvasGroup          conversationBox;
    public CanvasGroup          responseBox;
    public TextMeshProUGUI      conversation_text;
    public TextMeshProUGUI      response_text_1;
    public TextMeshProUGUI      response_text_2;
    public TextMeshProUGUI      response_text_3;

    [Header("Checkpoints")]
    public List<Checkpoint> checkpoints;
    private bool            deathDetected = false;

    [Header("Cut Scenes")]
    public TextMeshProUGUI  text_continue;

    [Header("Pause Menu")]
    public GameObject       pauseMenu;
    public TextMeshProUGUI  pause_resume;
    public TextMeshProUGUI  pause_settings;
    public TextMeshProUGUI  pause_tutorials;
    public TextMeshProUGUI  pause_quit;
    private MenuController  pause_controller;
    private bool            pauseMenuEnabled = true;

    [Header("Loading Screens")]
    public Image            splash_art;
    public TextMeshProUGUI  press_any_key;

    protected virtual void Start()
    {
        cube.SetActive(false);
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateCube();
        DetectDeath();
        Control();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    protected virtual void OnStageStarted()
    {
        InitGameSettings();
        InitPauseMenu();
        InitSettingsMenu();
        InitCheckpoints();
        InitCamera();
        DisablePastCheckpoints();
        
        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.EnableArms(enabledArms);
        }
    }

    // Called both in Pause & ForcePause
    protected virtual void OnGamePaused()
    {
        DisableControl();
        Time.timeScale = 0;
        isPaused = true;

        player.OnPause();
        firstArm.OnPause();
        secondArm.OnPause();
        PauseAudio();
    }

    // Called both in Resume & ForceResume
    protected virtual void OnGameResumed()
    {
        EnableControl();
        Time.timeScale = 1;
        isPaused = false;

        player.OnResume();
        firstArm.OnResume();
        secondArm.OnResume();
        ResumeAudio();
    }

    // Pause game with pause menu
    public void PauseGame()
    {
        tempControlIndex = controlIndex;
        controlIndex = UI;
        OnGamePaused();
        ShowPauseMenu();
    }

    // Resume game from pause menu
    private void ResumeGame()
    {
        OnGameResumed();
        HidePauseMenu();
    }

    // Pause game without pause menu. Force freeze controls
    public void ForcePauseGame()
    {
        OnGamePaused();
        EnablePause(false);
    }

    // Resume game from forced pause state
    public void ForceResumeGame()
    {
        OnGameResumed();
        EnablePause(true);
    }

}
