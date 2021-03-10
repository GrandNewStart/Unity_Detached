using System.Collections.Generic;

public class LineNode
{
    private Speaker speaker;
    private string line;
    private List<string> responses;
    private List<LineNode> nextLines;

    public LineNode(
        Speaker speaker,
        string line,
        List<string> responses,
        List<LineNode> nextLines)
    {
        this.speaker = speaker;
        this.line = line;
        if (responses == null)
        {
            this.responses = new List<string>();
        }
        else {
            this.responses = responses;
        }
        if (nextLines == null)
        {
            this.nextLines = new List<LineNode>();
        }
        else
        {
            this.nextLines = nextLines;
        }
    }

    public Speaker GetSpeaker() { return speaker; }
    public string GetLine() { return line; }
    public List<string> GetResponses() { return responses; }
    public List<LineNode> GetNextLines() { return nextLines; }
}
