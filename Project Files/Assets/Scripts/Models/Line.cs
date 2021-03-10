using System.Collections.Generic;
public class Line
{
    private Speaker         speaker;
    private List<string>    texts;
    private List<string>    responses;

    public Line(
        Speaker speaker,
        List<string> texts,
        List<string> responses)
    {
        this.speaker    = speaker;
        this.texts      = texts;
        this.responses  = responses;
    }

    public Speaker GetSpeaker() { return speaker; }
    public List<string> GetTexts() { return texts; }
    public List<string> GetResponses()
    { 
        if (responses == null)
        {
            return new List<string>();
        }
        return responses;
    }
    public void SetSpeaker(Speaker speaker) { this.speaker = speaker; }
    public void SetTexts(List<string> texts) { this.texts = texts; }
    public void SetResponses(List<string> responses) { this.responses = responses; }

}