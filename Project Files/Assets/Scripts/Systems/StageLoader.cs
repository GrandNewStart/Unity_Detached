using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StageLoader : MonoBehaviour
{
    public GameObject       background;
    public GameObject       cube;
    public SpriteRenderer   pressAnyKeyText;
    public SpriteRenderer   artWork;
    private bool            isBackgroundLoaded  = false;
    private bool            isArtworkLoaded     = false;
    private bool            isLoadReady         = false;
    private int             sceneIndex;

    private void Start()
    {
        Color color;

        color = pressAnyKeyText.color;
        color.a = 0;
        pressAnyKeyText.color = color;

        color = artWork.color;
        color.a = 0;
        artWork.color = color;
    }

    private void Update()
    {
        RotateCube();
        if (isBackgroundLoaded)
        {
            if (isArtworkLoaded)
            {   
                if (isLoadReady)
                {
                    LoadLevel();
                }
                else
                {
                    LoadPressAnyKey();
                }
            }
            else
            {
                LoadArtWork();
            }
        }
        else
        {
            LoadBackground();
        }
    }

    public void LoadStage(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
    }

    private void LoadBackground()
    {
        float currentScale = background.transform.localScale.x;
        float targetScale = 5;

        if (currentScale < targetScale)
        {
            float newScale = currentScale * 1.1f;
            background.transform.localScale = new Vector3(newScale, newScale, 0);
        }
        else
        {
            isBackgroundLoaded = true;
        }
    }

    private void LoadArtWork()
    {
        float currentAlpha  = artWork.color.a;
        float targetAlpha   = 1;
        if (currentAlpha < targetAlpha)
        {
            Color color = artWork.color;
            color.a += Time.deltaTime;
            artWork.color = color;
        }
        else
        {
            isArtworkLoaded = true;
        }
    }

    private void LoadPressAnyKey()
    {
        float currentAlpha = pressAnyKeyText.color.a;
        float targetAlpha = 1;

        if (currentAlpha < targetAlpha)
        {
            Color color = pressAnyKeyText.color;
            color.a += Time.deltaTime * 5;
            pressAnyKeyText.color = color;
        }
        else
        {
            isLoadReady = true;
        }
    }

    private void LoadLevel()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1.0f, 1.0f, 1.0f));
    }
}
