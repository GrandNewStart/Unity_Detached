public partial class GameManager
{
    // Overrided from MenuInterface
    public void OnMenuSelected(int index)
    {
        switch (index)
        {
            case 0:
                // Pause -> RESUME
                ResumeGame();
                break;
            case 1:
                // Pause -> SETTINGS
                ShowSettings();
                break;
            case 2:
                // Pause -> TUTORIALS
                ShowTutorials();
                break;
            case 3:
                // Pause -> QUIT
                QuitGame();
                break;
            case 4:
                // Pause -> Settings -> FULL SCREEN
                SelectFullScreenMode();
                break;
            case 5:
                // Pause -> Settings -> WINDOWED
                SelectWindowedMode();
                break;
            case 6:
                // Pause -> Settings -> RESOLUTION
                break;
            case 7:
                // Pause -> Settings -> MASTER VOLUME
                break;
            case 8:
                // Pause -> Settings -> MUSIC VOLUME
                break;
            case 9:
                // Pause -> Settings -> GAME VOLUME
                break;
            case 10:
                // Pause -> Settings -> LANGUAGE
                break;
            case 11:
                // Pause -> Settings -> APPLY
                ApplyGameSettings();
                break;
            case 12:
                // Pause -> Settings -> BACK
                CloseSettings();
                break;
            default:
                break;
        }
    }
}