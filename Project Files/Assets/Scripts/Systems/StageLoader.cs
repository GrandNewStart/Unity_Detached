using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StageLoader : MonoBehaviour
{
    public GameObject       background;
    public GameObject       artWork;
    public GameObject       cube;
    public SpriteRenderer   pressAnyKeyText;
    private bool            isBackgroundLoaded = false;
    private bool            isArtworkLoaded = false;
    private bool            isLoadReady = false;
    private int             sceneIndex;

    private bool showScene;
    private bool isSceneEnd = false;
    private int currentScene = -1;
    private int maxIndex;
    [SerializeField] GameObject[] cutScene;

    private void Start()
    {
        Color color = pressAnyKeyText.color;
        color.a = 0;
        pressAnyKeyText.color = color;

        SpriteRenderer image = artWork.GetComponent<SpriteRenderer>();
        color = image.color;
        color.a = 0;
        image.color = color;

        maxIndex = cutScene.Length - 1;
    }

    public void LoadStage(int sceneIndex, bool showScene)
    {
        this.sceneIndex = sceneIndex;
        this.showScene = showScene;
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
        SpriteRenderer image = artWork.GetComponent<SpriteRenderer>();
        float currentAlpha  = image.color.a;
        float targetAlpha   = 1;

        if (currentAlpha < targetAlpha)
        {
            Color color = image.color;
            color.a += Time.deltaTime;
            image.color = color;
        }
        else
        {
            isArtworkLoaded = true;
        }
    }

    private void LoadPressAnyKey()
    {
        SpriteRenderer image = pressAnyKeyText.GetComponent<SpriteRenderer>();
        float currentAlpha = image.color.a;
        float targetAlpha = 1;

        if (currentAlpha < targetAlpha)
        {
            Color color = image.color;
            color.a += Time.deltaTime * 5;
            image.color = color;
        }
        else
        {
            isLoadReady = true;
        }
    }

    private void LoadScene()
    { 
        if(currentScene == -1)
        {
            cutScene[0].SetActive(true);
            currentScene++;
        }
        else if(currentScene == maxIndex)
        {
            isSceneEnd = true;
        }
        else if (Input.anyKeyDown)
        {
            if (currentScene < maxIndex)
            {
                print(currentScene);
                currentScene++;
                cutScene[currentScene].SetActive(true);
                if(currentScene != 0)cutScene[currentScene - 1].SetActive(false);
            }
        }
    }

    private void LoadLevel()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void Update()
    {
        RotateCube();
        LoadBackground();
        if (isBackgroundLoaded)
        {
            if (showScene)
            {
                cube.SetActive(false);
                LoadScene();
                if (isSceneEnd)
                {
                    LoadPressAnyKey();
                    cube.SetActive(true);
                    if (isLoadReady)
                    {
                        LoadLevel();
                    }
                }
            }
            else
            {
                LoadArtWork();
                if (isArtworkLoaded)
                {
                    LoadPressAnyKey();
                    if (isLoadReady)
                    {
                        LoadLevel();
                    }
                }
            }
        }
    }

    private void RotateCube()
    {
        cube.transform.Rotate(new Vector3(1.0f, 1.0f, 1.0f));
    }
}
