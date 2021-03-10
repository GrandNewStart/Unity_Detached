using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class GameManager
{
    protected void HideConversation()
    {
        conversation_text.text = "";
        response_text_1.text = "";
        response_text_2.text = "";
        response_text_3.text = "";
        Hide(conversationBox, null);
        Hide(responseBox, null);
        Hide(response_text_1, null);
        Hide(response_text_2, null);
        Hide(response_text_3, null);
    }

    protected void StartConversation(int i)
    {
        player.SetState(PlayerController.State.idle);
        controlIndex = CONVERSATION;
        Show(conversationBox, () => {
            LineNode conversation = conversations[i];
            StartCoroutine(ConversationRoutine(conversation));
        });
    }

    protected IEnumerator ConversationRoutine(LineNode conversation)
    {
        bool spaceKeyUp = true;

        while (true)
        {
            Speaker speaker = conversation.GetSpeaker();
            string name = speaker.GetName();
            string line = conversation.GetLine();
            string text = name + ": " + line;    
            bool isDone = false;
            int j = 1;

            while (true)
            {
                if (!isPaused)
                {
                    if (isDone)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            spaceKeyUp = false;
                            break;
                        }
                    }
                    else
                    {
                        conversation_text.text = text.Substring(0, j);
                        if (Input.GetKeyUp(KeyCode.Space))
                        {
                            spaceKeyUp = true;
                        }
                        if (j++ == text.Length)
                        {
                            isDone = true;
                        }
                        if (Input.GetKeyDown(KeyCode.Space) && spaceKeyUp)
                        {
                            conversation_text.text = text;
                            spaceKeyUp = false;
                            isDone = true;
                        }
                    }
                }
                yield return null;
            }

            List<string> responses = conversation.GetResponses();
            List<LineNode> nextLines = conversation.GetNextLines();
            if (nextLines.Count == 0) { break; }
            if (responses.Count == 0)
            {
                conversation = nextLines[0];
            }
            else
            {
                int responseIndex = 0;
                int maxIndex = -1;
                SelectResponse(responseIndex);
                if (responses.Count > 0)
                {
                    response_text_1.text = responses[0];
                    maxIndex++;
                }
                if (responses.Count > 1)
                {
                    response_text_2.text = responses[1];
                    maxIndex++;
                }
                if (responses.Count > 2)
                {
                    response_text_3.text = responses[2];
                    maxIndex++;
                }
                responseBox.alpha = 1.0f;

                bool responseSelected = false;

                while (!responseSelected)
                {
                    if (isPaused) { yield return null; }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        spaceKeyUp = true;
                    }
                    if (Input.GetKeyDown(KeyCode.W) ||
                        Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (responseIndex > 0)
                        {
                            responseIndex--;
                            SelectResponse(responseIndex);
                            conversation = nextLines[responseIndex];
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S) ||
                        Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (responseIndex < maxIndex)
                        {
                            responseIndex++;
                            SelectResponse(responseIndex);
                            conversation = nextLines[responseIndex];
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Space) && spaceKeyUp)
                    {
                        responseSelected = true;
                        Hide(responseBox, null);
                    }
                    yield return null;
                }
            }
        }

        conversation_text.text = "";
        HideConversation();
        controlIndex = PLAYER;
    }

    private void SelectResponse(int index)
    {
        switch(index)
        {
            case 0:
                SetResponseBlack(response_text_1, true);
                SetResponseBlack(response_text_2, false);
                SetResponseBlack(response_text_3, false);
                break;
            case 1:
                SetResponseBlack(response_text_1, false);
                SetResponseBlack(response_text_2, true);
                SetResponseBlack(response_text_3, false);
                break;
            case 2:
                SetResponseBlack(response_text_1, false);
                SetResponseBlack(response_text_2, false);
                SetResponseBlack(response_text_3, true);
                break;
            default:
                return;
        }
    }

    private void SetResponseBlack(TextMeshProUGUI text, bool isBlack)
    {
        Color color = text.color;
        if (isBlack)
        {
            color.a = 1;
        }
        else
        {
            color.a = 0.3f;
        }
        text.color = color;
    }

}