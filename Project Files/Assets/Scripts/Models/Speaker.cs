public class Speaker
{
    private int id;
    private string name;

    public Speaker(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int GetId() { return id; }
    public string GetName() { return name; }
    public void SetId(int id) { this.id = id; }
    public void SetName(string name) { this.name = name; }
}