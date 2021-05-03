using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public partial class HomeController : MonoBehaviour
{
    private int             menuIndex;
    private int             stage;
    private const int       MENU_HOME   = 0;
    private const int       MENU_1      = 1;
    private const int       MENU_2      = 2;
    private const int       MENU_3      = 3;
    private const int       MENU_4      = 4;

    public Image                background;
    public TMPro.TMP_FontAsset  font_english;
    public TMPro.TMP_FontAsset  font_korean;
    public Sprite               checkbox_unchecked;
    public Sprite               checkbox_checked;

    [Header("Menus")]
    private MenuController menuController;
    public TextMeshProUGUI loadGame;
    public TextMeshProUGUI newGame;
    public TextMeshProUGUI settings;
    public TextMeshProUGUI quit;

    [Header("No Save Data")]
    private MenuController  menu_1_controller;
    public GameObject       menu_1_screen;
    public TextMeshProUGUI  menu_1_message;
    public TextMeshProUGUI  menu_1_yes;
    public TextMeshProUGUI  menu_1_no;

    [Header("Save Data Exists")]
    private MenuController  menu_2_controller;
    public GameObject       menu_2_screen;
    public TextMeshProUGUI  menu_2_message;
    public TextMeshProUGUI  menu_2_yes;
    public TextMeshProUGUI  menu_2_no;

    [Header("Settings")]
    private MenuController  menu_3_controller;
    public GameObject       menu_3_screen;
    public TextMeshProUGUI  menu_3_message;
    public TextMeshProUGUI  menu_3_full_screen;
    public TextMeshProUGUI  menu_3_windowed;
    public TextMeshProUGUI  menu_3_resolution;
    public TextMeshProUGUI  menu_3_resolution_value;
    public TextMeshProUGUI  menu_3_master_volume;
    public TextMeshProUGUI  menu_3_master_volume_value;
    public TextMeshProUGUI  menu_3_music_volume;
    public TextMeshProUGUI  menu_3_music_volume_value;
    public TextMeshProUGUI  menu_3_game_volume;
    public TextMeshProUGUI  menu_3_game_volume_value;
    public TextMeshProUGUI  menu_3_language;
    public TextMeshProUGUI  menu_3_language_value;
    public TextMeshProUGUI  menu_3_apply;
    public TextMeshProUGUI  menu_3_back;
    public Image            menu_3_full_screen_checkbox;
    public Image            menu_3_windowed_checkbox;
    public Slider           menu_3_master_volume_slider;
    public Slider           menu_3_music_volume_slider;
    public Slider           menu_3_game_volume_slider;
    private bool            tempIsFullScreen = true;
    private int             tempLanguage = GameSettings.ENGLISH;
    private int             tempResolution = GameSettings.FHD;
    private float           tempMasterVolume = 0.5f;
    private float           tempMusicVolume = 0.5f;
    private float           tempGameVolume = 0.5f;

    [Header("Quit")]
    private MenuController  menu_4_controller;
    public GameObject       menu_4_screen;
    public TextMeshProUGUI  menu_4_message;
    public TextMeshProUGUI  menu_4_yes;
    public TextMeshProUGUI  menu_4_no;

    [Header("Loading Screens")]
    public Image            chap_1_splash;
    public TextMeshProUGUI  press_any_key;

    [Header("Audios")]
    public AudioSource bgm;
    public AudioSource click;
    public AudioSource page;

    private void Start()
    {
        //SaveSystem.DeleteSaveFile();
        //GameSettings settings = new GameSettings(true, GameSettings.KOREAN, GameSettings.FHD, 0.5f, 0.5f, 0.5f);
        //SaveSystem.SaveSettings(settings);
        DetectResolutions();
        InitGameSettings();
        InitAudioAttributes();
        InitMenus();
        InitMenu1();
        InitMenu2();
        InitMenu3();
        InitMenu4();
        PlayBgm();
        CrossfadeStart(0, 0, null);
        Cursor.visible = false;
    }

    private void Update()
    {
        Control();
    }
}
