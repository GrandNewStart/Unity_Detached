using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController
{
    public enum Orientation { vertical, horizontal }

    private GameObject      screen;
    private List<Menu>      menus;
    private AudioSource     nextSound;
    private AudioSource     okSound;
    private bool            isVisible = false;
    private bool            isEnabled = false;
    private int             index = 0;
    private int             max = 0;
    private Orientation     orientation = Orientation.vertical;
    private MenuInterface   menuAction;

    public MenuController(
        GameObject screen, 
        List<Menu> menus,
        MenuInterface menuAction)
    {
        this.screen = screen;
        this.menus = menus;
        this.menuAction = menuAction;
        if (menus == null) return;
        max = menus.Count - 1;
    }

    public void ControlMenu()
    {
        if (isVisible)
        {
            if (isEnabled)
            {
                SelectMenu();
                EnterMenu();
            }
        }
    }

    private void SelectMenu()
    {
        if (menus == null) return;
        if (menus.Count == 0) return;
        if (orientation == Orientation.vertical)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.S))
            {
                MoveIndex(1);
                PlayNextSound();
                AdjustAlpha();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.W))
            {
                MoveIndex(-1);
                PlayNextSound();
                AdjustAlpha();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.D))
            {
                MoveIndex(1);
                PlayNextSound();
                AdjustAlpha();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.A))
            {
                MoveIndex(-1);
                PlayNextSound();
                AdjustAlpha();
            }
        }
    }

    private void MoveIndex(int dir)
    {
        if (dir == 1)
        {
            if (index >= max)
            {
                return;
            }
            else
            {
                index++;
            }
        }
        if (dir == -1)
        {
            if (index <= 0)
            {
                return;
            }
            else
            {
                index--;
            }
        }
    }

    private void AdjustAlpha()
    {
        if (menus == null) return;
        foreach (Menu menu in menus)
        {
            Color32 color = menu.text.color;
            color.a = 50;
            menu.text.color = color;
        }

        TextMeshProUGUI text = menus[index].text;
        Color32 selectedColor = text.color;
        selectedColor.a = 255;
        text.color = selectedColor;
    }

    private void EnterMenu()
    {
        if (menus == null) return;
        if (menus.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            PlayOkSound();
            Menu menu = menus[index];
            menuAction.OnMenuSelected(menu.id);
        }
    }

    public void SetDefault()
    {
        index = 0;
        AdjustAlpha();
    }

    public void SetNextSound(AudioSource nextSound)
    {
        this.nextSound = nextSound;
        this.nextSound.playOnAwake = false;
        this.nextSound.loop = false;
    }

    public void SetOkSound(AudioSource okSound)
    {
        this.okSound = okSound;
        this.okSound.playOnAwake = false;
        this.okSound.loop = false;
    }

    public void SetOrientation(Orientation orientation)
    {
        this.orientation = orientation;
    }

    public void SetVisible(bool isVisible)
    {
        this.isVisible = isVisible;
        if (screen != null)
        {
            screen.SetActive(isVisible);
        }
        SetDefault();
    }

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }

    public void SetIndex(int index)
    {
        if (index >= menus.Count)
        {
            Debug.LogError("MENU INDEX OUT OF BOUNDS");
            return;
        }
        this.index = index;
        AdjustAlpha();
    }

    private void PlayNextSound()
    {
        if (nextSound == null) return;
        nextSound.Play();
    }

    private void PlayOkSound()
    {
        if (okSound == null) return;
        okSound.Play();
    }

}
