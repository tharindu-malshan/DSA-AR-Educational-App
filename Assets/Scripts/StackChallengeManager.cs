using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackChallengeManager : MonoBehaviour
{
    public GameObject challengePanel;
    public TMP_Text challengeText;
    public TMP_Text feedbackText;
    public StackManager stackManager;

    private int challengeNumber = 1;
    private bool challengeActive = false;

    private int currentOperationStep = 0;

    private readonly string[] expectedOperations =
    {
    "Push 10",
    "Push 20",
    "Pop 20",
    "Push 30"
    };

    void Start()
    {
        challengePanel.SetActive(false);
    }

    public void StartChallenge()
    {
        if (stackManager == null)
        {
            Debug.LogError("Stack Manager not assigned!");
            return;
        }

        if (stackManager.IsStackAnimating())
            return;

        challengeActive = true;
        currentOperationStep = 0;
        challengePanel.SetActive(true);
        stackManager.ResetStack();

        if (challengeNumber == 1)
        {
            challengeText.text =
                "CHALLENGE 1\n" +
                "Create a Stack with exactly 3 items.";
        }
        else
        {
            challengeText.text =
                "CHALLENGE 2\n" +
                "Push 10, Push 20, Pop, Push 30.\n" +
                "What is the TOP value?";
                feedbackText.color = Color.white;
                feedbackText.text = "Next: Push 10";
        }

        
    }

    public void CheckAnswer()
    {
        if (!challengeActive ||
            stackManager.IsStackAnimating())
            return;

        int count = stackManager.GetStackCount();

        if (challengeNumber == 1)
        {
            if (count == 3)
            {
                feedbackText.text =
                    "Challenge 1 Completed!";

                feedbackText.color = Color.green;

                challengeActive = false;
                challengeNumber = 2;
            }
            else
            {
                feedbackText.text =
                    "Not correct! Items: " + count;

                feedbackText.color = Color.red;
            }
        }
        
  else
 {
    int top = stackManager.GetTopValue();

    List<string> history =
        stackManager.GetOperationHistory();

    string[] expectedSequence =
    {
        "Push 10",
        "Push 20",
        "Pop 20",
        "Push 30"
    };

    bool correctSequence =
        history.Count == expectedSequence.Length;

    if (correctSequence)
    {
        for (int i = 0; i < expectedSequence.Length; i++)
        {
            if (history[i] != expectedSequence[i])
            {
                correctSequence = false;
                break;
            }
        }
    }

    if (correctSequence && count == 2 && top == 30)
    {
        feedbackText.text =
            "Challenge 2 Completed!\n" +
            "Correct Sequence! TOP = 30";

        feedbackText.color = Color.green;
        challengeActive = false;
    }
    else
    {
        feedbackText.text =
            "Incorrect sequence!\n" +
            "Follow: Push 10, Push 20, Pop, Push 30.\n" +
            "Press Start Challenge to retry.";

        feedbackText.color = Color.red;
    }
  }

    }

    public void CloseChallenge()
    {
        challengeActive = false;
        challengePanel.SetActive(false);
    }

    
 public void CheckOperation(string operation)
 {
    if (!challengeActive || challengeNumber != 2)
        return;

    if (currentOperationStep >= expectedOperations.Length)
        return;

    string expected = expectedOperations[currentOperationStep];

    if (operation == expected)
    {
        currentOperationStep++;

        feedbackText.color = Color.green;

        if (currentOperationStep < expectedOperations.Length)
        {
            feedbackText.text =
                "Correct! Next: " +
                expectedOperations[currentOperationStep];
        }
        else
        {
            feedbackText.text =
                "All operations correct!\n" +
                "Press Check Answer.";
        }
    }
    else
    {
        feedbackText.color = Color.red;

        feedbackText.text =
            "Wrong Operation!\n" +
            "Expected: " + expected +
            "\nYou did: " + operation +
            "\nRestart Challenge to retry.";
    }
 }

}
