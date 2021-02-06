using UnityEngine;
using TMPro;

public class Menu
{
    public int id;
    public TextMeshProUGUI text;
    public string name;

    public Menu(int id, TextMeshProUGUI text, string name)
    {
        this.id = id;
        this.text = text;
        this.name = name;
    }

}

public interface MenuInterface
{
    void OnMenuSelected(int id);
}