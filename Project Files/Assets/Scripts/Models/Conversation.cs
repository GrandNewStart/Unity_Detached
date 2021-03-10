using System.Collections.Generic;

public class Conversation
{
    private int         stage;
    private int         num;
    private List<Line>  lines;

    public Conversation(int stage, int num, List<Line> lines)
    {
        this.stage  = stage;
        this.num    = num;
        this.lines  = lines;
    }

    public int GetStage() { return stage; }
    public int GetNum() { return num; }
    public List<Line> GetLines() { return lines; }
    public void SetStage(int stage) { this.stage = stage; }
    public void SetNum(int num) { this.num = num; }
    public void SetLines(List<Line> lines) { this.lines = lines; }
}