﻿using UnityEngine;
using TMPro;

public partial class HomeController : MonoBehaviour
{
    private int             menuIndex;
    private int             stage;
    private const int       MENU_HOME   = 0;
    private const int       MENU_1      = 1;
    private const int       MENU_2      = 2;
    private const int       MENU_3      = 3;
    private const int       MENU_4      = 4;

    [Header("Menus")]
    private MenuController menuController;
    public TextMeshProUGUI loadGame;
    public TextMeshProUGUI newGame;
    public TextMeshProUGUI settings;
    public TextMeshProUGUI quit;

    [Header("No Save Data")]
    private MenuController menu_1_controller;
    public GameObject       menu_1_screen;
    public TextMeshProUGUI  menu_1_yes;
    public TextMeshProUGUI  menu_1_no;

    [Header("Save Data Exists")]
    private MenuController menu_2_controller;
    public GameObject       menu_2_screen;
    public TextMeshProUGUI  menu_2_yes;
    public TextMeshProUGUI  menu_2_no;

    [Header("Settings")]
    private MenuController menu_3_controller;
    public GameObject       menu_3_screen;

    [Header("Quit")]
    private MenuController menu_4_controller;
    public GameObject       menu_4_screen;
    public TextMeshProUGUI  menu_4_yes;
    public TextMeshProUGUI  menu_4_no;

    [Header("Loading Screens")]
    public GameObject loadingScreen;
    public GameObject background;
    public GameObject splashArt;
    public GameObject cube;
    public GameObject text;

    [Header("Audios")]
    public AudioSource bgm;
    public AudioSource click;
    public AudioSource page;

    private void Start()
    {
        InitAudioAttributes();
        InitMenus();
        InitMenu1();
        InitMenu2();
        InitMenu3();
        InitMenu4();
        PlayBgm();
    }

    private void Update()
    {
        Control();
        RotateCube();
    }
}
