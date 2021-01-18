using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController
{
    public List<Menu> items;
    public GameObject arrow;
    public AudioSource click;
    public AudioSource page;
    private MenuInterface menuAction;
    private int index = 0;
    private int max = 0;
    private bool isEnabled = false;
    private bool isVisible = false;
    public enum Style { arrow, size, brightness }
    private Style style = Style.arrow;
    public enum Orientation { horizontal, vertical }
    private Orientation orientation = Orientation.vertical;

    public MenuController(
        Orientation orientation,
        Style style,
        List<Menu> items,
        AudioSource click,
        AudioSource page,
        MenuInterface menuAction)
    {
        this.orientation = orientation;
        this.style = style;
        this.items = items;
        this.click = click;
        this.page = page;
        this.menuAction = menuAction;
        max = items.Count - 1;
        InitAudioAttributes();
    }

    private void InitAudioAttributes()
    {
        click.playOnAwake = false;
        click.loop = false;
        page.playOnAwake = false;
        page.loop = false;
    }

    public void ControlUI()
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
        switch (style)
        {
            case Style.arrow:
                SelectByArrow();
                break;
            case Style.size:
                SelectBySize();
                break;
            case Style.brightness:
                SelectByBrightness();
                break;
        }
    }

    private void EnterMenu()
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            menuAction.OnMenuSelected(index);
        }
    }

    private void MoveIndex(int dir)
    {
        if (arrow != null)
        {
            if (dir == -1)
            {
                if (index >= max)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
            if (dir == 1)
            {
                if (index <= 0)
                {
                    index = max;
                }
                else
                {
                    index--;
                }
            }

        }
    }

    private void HideMenu(bool visible)
    {
        foreach (Menu item in items)
        {
            item.GetObject().SetActive(visible);
        }
    }

    private void SelectByArrow()
    {
        switch (orientation)
        {
            case Orientation.vertical:
                if (Input.GetKeyDown(KeyCode.UpArrow) ||
                    Input.GetKeyDown(KeyCode.W))
                {
                    MoveIndex(1);
                    MoveArrow();
                    PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) ||
                    Input.GetKeyDown(KeyCode.S))
                {
                    MoveIndex(-1);
                    MoveArrow();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                    Input.GetKeyDown(KeyCode.A))
                {
                    MoveIndex(-1);
                    MoveArrow();
                    PlayClickSound();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) ||
                    Input.GetKeyDown(KeyCode.D))
                {
                    MoveIndex(1);
                    MoveArrow();
                    PlayClickSound();
                }
                break;
        }
    }

    private void MoveArrow()
    {
        Menu menu = items[index];
        Vector3 position = menu.GetPosition();
        Vector3 size = menu.GetSprite().size;
        position.x += size.x / 2;
        arrow.transform.position = position;
    }

    private void SelectBySize()
    {
        switch (orientation)
        {
            case Orientation.vertical:
                if (Input.GetKeyDown(KeyCode.UpArrow) ||
                    Input.GetKeyDown(KeyCode.W))
                {
                    MoveIndex(1);
                    AdjustItemSize();
                    PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) ||
                    Input.GetKeyDown(KeyCode.S))
                {
                    MoveIndex(-1);
                    AdjustItemSize();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                    Input.GetKeyDown(KeyCode.A))
                {
                    MoveIndex(-1);
                    AdjustItemSize();
                    PlayClickSound();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) ||
                    Input.GetKeyDown(KeyCode.D))
                {
                    MoveIndex(1);
                    AdjustItemSize();
                    PlayClickSound();
                }
                break;
        }
    }

    private void AdjustItemSize()
    {
        foreach (Menu item in items)
        {
            item.GetTransform().localScale = Vector3.one;
        }
        Menu menu = items[index];
        menu.GetObject().transform.localScale *= 2;
    }

    private void SelectByBrightness()
    {
        switch (orientation)
        {
            case Orientation.vertical:
                if (Input.GetKeyDown(KeyCode.UpArrow) ||
                    Input.GetKeyDown(KeyCode.W))
                {
                    MoveIndex(1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) ||
                    Input.GetKeyDown(KeyCode.S))
                {
                    MoveIndex(-1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                    Input.GetKeyDown(KeyCode.A))
                {
                    MoveIndex(-1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) ||
                    Input.GetKeyDown(KeyCode.D))
                {
                    MoveIndex(1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }
                break;
        }
    }

    private void AdjustItemBrightness()
    {
        foreach (Menu item in items)
        {
            Color color = item.GetSprite().color;
            color.r -= 30f;
            color.g -= 30f;
            color.b -= 30f;
            item.GetSprite().color = color;
        }
        for (int i = 0; i <= max; i++)
        {
            Menu item = items[index];
            Color color = item.GetSprite().color;
            if (i == index)
            {
                color.r += 30f;
                color.g += 30f;
                color.b += 30f;
                item.GetSprite().color = color;
                continue;
            }
            color.r -= 30f;
            color.g -= 30f;
            color.b -= 30f;
            item.GetSprite().color = color;
        }
    }

    public void SetDefault()
    {
        index = 0;
        switch (style)
        {
            case Style.arrow:
                MoveArrow();
                break;
            case Style.brightness:
                AdjustItemSize();
                break;
            case Style.size:
                AdjustItemSize();
                break;
        }
    }

    public void SetVisible(bool isVisible)
    {
        this.isVisible = isVisible;
    }

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }

    private void PlayClickSound()
    {
        click.Play();
    }

    private void PlayPageSound()
    {
        page.Play();
    }
}
