using UnityEngine;

public partial class GameManager
{
    private void RotateLoadingBar()
    {
        loadingBar.transform.Rotate(new Vector3(0, 0, 2));
    }

    public void ShowLoadingBar(float seconds)
    {
        loadingBar.fillAmount = 0.75f;
        if (seconds != INFINITE)
        {
            Invoke(nameof(HideLoadingBar), seconds);
        }
    }

    public void HideLoadingBar()
    {
        loadingBar.fillAmount = 0;
    }

}