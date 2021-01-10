using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController: MonoBehaviour
{
    public List<Menu> items;
    public GameObject arrow;
    public AudioSource click;
    public AudioSource page;
    private int index = 0;
    private int max = 0;
    private bool isEnabled = false;
    private bool isVisible = false;
    public enum Style { arrow, size, brightness }
    private Style style = Style.arrow;
    public enum Orientation { horizontal, vertical }
    private Orientation orientation = Orientation.vertical;

    private void Start()
    {
        click.playOnAwake = false;
        click.loop = false;
        page.playOnAwake = false;
        page.loop = false;
        max = items.Count - 1;
        HideMenu(isVisible);
    }

    private void Update()
    {
        if (isVisible)
        {
            if (isEnabled)
            {
                SelectMenu();
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

    private void MoveIndex(int dir)
    {
        if (arrow != null)
        {
            if (dir == 1)
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
            if (dir == -1)
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
                if (Input.GetButtonDown("up"))
                {
                    MoveIndex(1);
                    MoveArrow();
                    PlayClickSound();
                }

                if (Input.GetButtonDown("down"))
                {
                    MoveIndex(-1);
                    MoveArrow();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetButtonDown("left"))
                {
                    MoveIndex(-1);
                    MoveArrow();
                    PlayClickSound();
                }
                if (Input.GetButtonDown("right"))
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
        position.x += (size.x / 2 + 10);
        arrow.transform.position = position;
    }

    private void SelectBySize()
    {
        switch (orientation)
        {
            case Orientation.vertical:
                if (Input.GetButtonDown("up"))
                {
                    MoveIndex(1);
                    AdjustItemSize();
                    PlayClickSound();
                }

                if (Input.GetButtonDown("down"))
                {
                    MoveIndex(-1);
                    AdjustItemSize();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetButtonDown("left"))
                {
                    MoveIndex(-1);
                    AdjustItemSize();
                    PlayClickSound();
                }
                if (Input.GetButtonDown("right"))
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
                if (Input.GetButtonDown("up"))
                {
                    MoveIndex(1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }

                if (Input.GetButtonDown("down"))
                {
                    MoveIndex(-1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }
                break;
            case Orientation.horizontal:
                if (Input.GetButtonDown("left"))
                {
                    MoveIndex(-1);
                    AdjustItemBrightness();
                    PlayClickSound();
                }
                if (Input.GetButtonDown("right"))
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

    private void PlayClickSound()
    {
        click.Play();
    }

    private void PlayPageSound()
    {
        page.Play();
    }
}

public class Menu
{
    private int             id;
    private string          name;
    private GameObject      icon;
    private SpriteRenderer  sprite;

    public Menu(int id, string name, GameObject icon) {
        this.id = id;
        this.name = name;
        this.icon = icon;
        sprite = icon.GetComponent<SpriteRenderer>();
    }

    public string GetName()
    { return name; }

    public GameObject GetObject()
    { return icon; }

    public Transform GetTransform()
    { return icon.transform; }

    public Vector3 GetPosition()
    { return icon.transform.position; }

    public SpriteRenderer GetSprite()
    { return sprite; }
}
