using UnityEngine;

public class Menu
{
    private int id;
    private string name;
    private GameObject icon;
    private SpriteRenderer sprite;

    public Menu(int id, string name, GameObject icon)
    {
        this.id = id;
        this.name = name;
        this.icon = icon;
        sprite = icon.GetComponent<SpriteRenderer>();
    }

    public int GetId()
    { return id; }
    
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

public interface MenuInterface
{
    void OnMenuSelected(int id);
}