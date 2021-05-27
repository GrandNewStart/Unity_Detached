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

    public const int INFINITE       = 0;
    public const int PLAYER         = 0;
    public const int FIRST_ARM      = 1;
    public const int SECOND_ARM     = 2;
    public const int UI             = 3;
    public const int OTHERS         = 4;
    public const int CONVERSATION   = 4;
    public const int DISABLED       = -1;
    public const int PAUSE          = 0;
    public const int SETTINGS       = 1;
    public const int TUTORIALS      = 2;
    
    public bool                 isPaused;
    public Image                loadingBar;
    public Image                screenMask;
    public PlayerController     player;
    public ArmController        firstArm;
    public ArmController        secondArm;

    [Header("Audios")]
    public AudioSource pageSound;
    public AudioSource clickSound;
    public AudioSource bgm;

    [Header("Props")]
    public List<SwitchController> switches = new List<SwitchController>();
    public List<CrusherController> crushers = new List<CrusherController>();
    public List<TreadmillController> treadmills = new List<TreadmillController>();
    public List<TelescopeController> telescopes = new List<TelescopeController>();

    [Header("Control")]
    public int controlIndex = 0;
    private int tempControlIndex = 0;
    private int menuIndex = 0;

    [Header("Camera")]
    public new Camera camera;
    public Transform cameraTarget;
    public bool cameraMoving = false;
    public bool cameraAdjusting = false;
    private float cameraSpeed = 130;
    public float defaultCameraSize = 8;

    [Header("Game Settings")]
    [SerializeField] protected Sprite           checkbox_checked;
    [SerializeField] protected Sprite           checkbox_unchecked;
    [SerializeField] protected TMP_FontAsset    font_english;
    [SerializeField] protected TMP_FontAsset    font_korean;
    [SerializeField] protected CanvasGroup      settingsMenu;
    [SerializeField] protected TextMeshProUGUI  settings_title;
    [SerializeField] protected TextMeshProUGUI  settings_full_screen;
    [SerializeField] protected TextMeshProUGUI  settings_windowed;
    [SerializeField] protected TextMeshProUGUI  settings_resolution;
    [SerializeField] protected TextMeshProUGUI  settings_resolution_value;
    [SerializeField] protected TextMeshProUGUI  settings_master_volume;
    [SerializeField] protected TextMeshProUGUI  settings_master_volume_value;
    [SerializeField] protected TextMeshProUGUI  settings_music_volume;
    [SerializeField] protected TextMeshProUGUI  settings_music_volume_value;
    [SerializeField] protected TextMeshProUGUI  settings_game_volume;
    [SerializeField] protected TextMeshProUGUI  settings_game_volume_value;
    [SerializeField] protected TextMeshProUGUI  settings_language;
    [SerializeField] protected TextMeshProUGUI  settings_language_value;
    [SerializeField] protected TextMeshProUGUI  settings_apply;
    [SerializeField] protected TextMeshProUGUI  settings_back;
    [SerializeField] protected Image            settings_full_screen_checkbox;
    [SerializeField] protected Image            settings_windowed_checkbox;
    [SerializeField] protected Slider           settings_master_volume_slider;
    [SerializeField] protected Slider           settings_music_volume_slider;
    [SerializeField] protected Slider           settings_game_volume_slider;
    private MenuController  settings_controller;
    private bool    tempIsFullScreen    = false;
    private int     tempResolution      = GameSettings.FHD;
    private int     tempLanguage        = GameSettings.ENGLISH;
    private float   tempMasterVolume    = 0.5f;
    private float   tempMusicVolume     = 0.5f;
    private float   tempGameVolume      = 0.5f;

    [Header("Conversation")]
    [SerializeField] protected CanvasGroup          conversationBox;
    [SerializeField] protected CanvasGroup          responseBox;
    [SerializeField] protected TextMeshProUGUI      conversation_text;
    [SerializeField] protected TextMeshProUGUI      response_text_1;
    [SerializeField] protected TextMeshProUGUI      response_text_2;
    [SerializeField] protected TextMeshProUGUI      response_text_3;
    protected List<LineNode> conversations = new List<LineNode>();

    [Header("Checkpoints")]
    public List<Checkpoint> checkpoints;
    private bool            deathDetected = false;

    [Header("UI")]
    public TextMeshProUGUI  text_continue;

    [Header("Pause Menu")]
    public CanvasGroup      pauseMenu;
    public TextMeshProUGUI  pause_resume;
    public TextMeshProUGUI  pause_settings;
    public TextMeshProUGUI  pause_tutorials;
    public TextMeshProUGUI  pause_quit;
    private MenuController  pause_controller;
    private bool            pauseMenuEnabled = true;

    [Header("Loading Screens")]
    public Image            splash_art;
    public TextMeshProUGUI  text_press_any;

    protected virtual void Awake()
    {
        cameraTarget = player.transform;
        Cursor.visible = false;
    }

    protected virtual void Start()
    {
        OnStageStarted();
    }

    protected virtual void Update()
    {
        RotateLoadingBar();
        DetectDeath();
        Control();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    protected virtual void OnStageStarted()
    {
        loadingBar.fillAmount = 0;
        InitPauseMenu();
        InitGameSettings();
        InitSettingsMenu();
        InitCheckpoints();
        InitCamera();
        DisablePastCheckpoints();
        
        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.enabledArms = enabledArms;
            player.arms = enabledArms;
        }
    }

    // Called both in Pause & ForcePause
    protected virtual void OnGamePaused() { }

    // Called both in Resume & ForceResume
    protected virtual void OnGameResumed() { }

    // Pause game with pause menu
    public void PauseGame()
    {
        OnGamePaused();
        player.OnPause();
        firstArm.OnPause();
        secondArm.OnPause();

        //DisableControl();
        controlIndex = UI;
        Time.timeScale = 0;
        isPaused = true;
        PauseAudio();
        ShowPauseMenu();
    }

    // Resume game from pause menu
    private void ResumeGame()
    {
        OnGameResumed();
        player.OnResume();
        firstArm.OnResume();
        secondArm.OnResume();

        EnableControl();
        Time.timeScale = 1;
        isPaused = false;
        ResumeAudio();
        HidePauseMenu();
    }

    // Pause game without pause menu. Force freeze controls
    public void ForcePauseGame()
    {
        OnGamePaused();
        player.OnPause();
        firstArm.OnPause();
        secondArm.OnPause();

        DisableControl();
        Time.timeScale = 0;
        isPaused = true;
        PauseAudio();
        EnablePause(false);
    }

    // Resume game from forced pause state
    public void ForceResumeGame()
    {
        OnGameResumed();
        player.OnResume();
        firstArm.OnResume();
        secondArm.OnResume();

        EnableControl();
        Time.timeScale = 1;
        isPaused = false;
        ResumeAudio();
        HidePauseMenu();
        EnablePause(true);
    }

}
