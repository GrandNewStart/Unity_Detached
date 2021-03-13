using System;

[Serializable]
public class GameSettings
{
    public const int KOREAN = 0;
    public const int ENGLISH = 1;
    public const int HD = 0;
    public const int SHD = 1;
    public const int FHD = 2;
    public const int QHD = 3;
    public const int UHD = 4;

    private bool isFullScreen;
    private int language;
    private int resolution;
    private float masterVolume;
    private float musicVolume;
    private float gameVolume;

    public GameSettings(
        bool isFullScreen,
        int language,
        int resolution,
        float masterVolume,
        float musicVolume,
        float gameVolume)
    {
        this.isFullScreen = isFullScreen;
        this.language = language;
        this.resolution = resolution;
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.gameVolume = gameVolume;
    }

    public void SetFullScreen(bool isFullScreen) 
    { this.isFullScreen = isFullScreen; }
    public void SetLanguage(int language) 
    { this.language = language; }
    public void SetResolution(int resolution)
    { this.resolution = resolution; }
    public void SetMasterVolume(int masterVolume) 
    { this.masterVolume = masterVolume; }
    public void SetMusicVolume(int musicVolume)
    { this.musicVolume = musicVolume; }
    public void SetGameVolume(int gameVolume)
    { this.gameVolume = gameVolume; }
    public bool IsFullScreen() 
    { return isFullScreen; }
    public int GetLanguage()
    { return language; }
    public int GetResolution() 
    { return resolution; }
    public float GetMasterVolume() 
    { return masterVolume; }
    public float GetMusicVolume()
    { return musicVolume; }
    public float GetGameVolume()
    { return gameVolume; }
}