using System.Collections.Generic;
using UnityEngine;

public class TestStageManager : GameManager
{
    [SerializeField] private GameObject truck;
    [SerializeField] private TelescopeController telescope;
    public float timeScale = 1;
    private bool conversationBegan = false;

    protected override void Start()
    {
        loadingBar.fillAmount = 0;
        OnTestStageStarted();
        currentCheckpoint = 0;
        SceneFadeStart(0, 0, null);
        InitConversation();
    }

    private void OnTestStageStarted()
    {
        InitTestSettings();
        InitPauseMenu();
        InitSettingsMenu();
        InitCheckpoints();
        InitCamera();
        DisablePastCheckpoints();

        if (isLoadingSaveData)
        {
            player.transform.position = position;
            player.enabledArms = enabledArms;
        }
    }

    private void InitConversation()
    {
        Speaker A = new Speaker(0, "A");
        Speaker B = new Speaker(1, "B");

        string a1 = "WOW! Can't believe this is actually working!";
        string b1 = "Seriously dude, it's remarkable!";
        string a2 = "Maybe now I can call myself a Unity expert.";
        string b2 = "Whoa, whoa don't get to excited. There are tons more to come!";
        string a3 = "I know. I just can't wait to achieve something really great on Unity.";
        string b3 = "Yeah, I totally agree. There's just too much to learn yet.";
        string a4 = "Come on, say something more.";
        string b4 = "This is a response test. What do you want to say?";
        
        string ra1 = "Hmmm...";
        string b5 = "'Hmmm'? That's your response?";
        string b8 = "Well, I guess that's all we have.";

        string ra2 = "I don't know...";
        string b6 = "Come on, you've got to have something in mind.";

        string ra3 = "Fuck you!";
        string b7 = "Gee...";
        string b9 = "Alright, forget it.";

        List<LineNode> nl1 = new List<LineNode>();
        List<LineNode> nl2 = new List<LineNode>();
        List<LineNode> nl3 = new List<LineNode>();
        List<LineNode> nl4 = new List<LineNode>();
        List<LineNode> nl5 = new List<LineNode>();
        List<LineNode> nl6 = new List<LineNode>();
        List<LineNode> nl7 = new List<LineNode>();
        List<LineNode> nl8 = new List<LineNode>();
        List<LineNode> nl9 = new List<LineNode>();
        List<LineNode> n20 = new List<LineNode>();
        List<string> r8 = new List<string>();

        LineNode line13 = new LineNode(B, b8, null, null);
        nl9.Add(line13);
        LineNode line12 = new LineNode(B, b9, null, null);
        n20.Add(line12);
        LineNode line11 = new LineNode(B, b7, null, n20);
        LineNode line10 = new LineNode(B, b6, null, nl9);
        LineNode line9 = new LineNode(B, b5, null, nl9);
        nl8.Add(line9);
        nl8.Add(line10);
        nl8.Add(line11);
        r8.Add(ra1);
        r8.Add(ra2);
        r8.Add(ra3);
        LineNode line8 = new LineNode(B, b4, r8, nl8);
        nl7.Add(line8);
        LineNode line7 = new LineNode(A, a4, null, nl7);
        nl6.Add(line7);
        LineNode line6 = new LineNode(B, b3, null, nl6);
        nl5.Add(line6);
        LineNode line5 = new LineNode(A, a3, null, nl5);
        nl4.Add(line5);
        LineNode line4 = new LineNode(B, b2, null, nl4);
        nl3.Add(line4);
        LineNode line3 = new LineNode(A, a2, null, nl3);
        nl2.Add(line3);
        LineNode line2 = new LineNode(B, b1, null, nl2);
        nl1.Add(line2);
        LineNode line1 = new LineNode(A, a1, null, nl1);

        conversations.Add(line1);
    }

    protected override void Update()
    {
        base.Update();
        DetectEventTrigger();
        Time.timeScale = timeScale;
    }

    private void DetectEventTrigger()
    {
        if (!conversationBegan)
        {
            bool truckReached = Physics2D.OverlapBox(
                truck.transform.position,
                new Vector2(14, 6),
                0,
                LayerMask.GetMask("Player"));
            if (truckReached)
            {
                conversationBegan = true;
                StartConversation(0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(truck.transform.position, new Vector3(14,6,0));
    }

}
