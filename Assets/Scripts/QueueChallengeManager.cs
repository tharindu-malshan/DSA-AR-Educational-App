
using UnityEngine;
using TMPro;

public class QueueChallengeManager : MonoBehaviour
{
    public GameObject challengePanel;
    public TMP_Text instructionText;
    public TMP_Text feedbackText;
    public QueueManager queueManager;

    private bool challengeActive = false;
    private int currentStep = 0;

    private readonly string[] operations =
    {
        "Enqueue",
        "Enqueue",
        "Dequeue"
    };

    private readonly int[] values = { 10, 20, 10 };

    void Start()
    {
        challengePanel.SetActive(false);
    }

    public void OpenChallenge()
    {
        if (queueManager.queueBase == null)
        {
            Debug.LogWarning("Place QueueBase first!");
            return;
        }

        challengeActive = false;
        queueManager.ResetQueue();

        currentStep = 0;
        challengeActive = true;

        challengePanel.SetActive(true);
        feedbackText.text = "Challenge started!";
        ShowInstruction();
    }

    public void CloseChallenge()
    {
        challengeActive = false;
        challengePanel.SetActive(false);
    }

    void ShowInstruction()
    {
        if (currentStep < operations.Length)
        {
            instructionText.text =
                "Step " + (currentStep + 1) +
                ": " + operations[currentStep] +
                " " + values[currentStep];
        }
    }

    public bool CanPerformOperation(string operation, int value)
    {
        if (!challengeActive)
            return true;

        if (operation == operations[currentStep] &&
            value == values[currentStep])
        {
            return true;
        }

        feedbackText.text =
            "Wrong operation! Try: " +
            operations[currentStep] + " " +
            values[currentStep];

        feedbackText.color = Color.red;

        return false;
    }

    public void OperationCompleted(string operation, int value)
    {
        if (!challengeActive)
            return;

        currentStep++;

        if (currentStep >= operations.Length)
        {
            challengeActive = false;

            instructionText.text =
                "Challenge Completed!";

            feedbackText.text =
                "Correct! Final Queue: 20";

            feedbackText.color = Color.green;

            PlayerPrefs.SetInt("QueueChallengeCompleted", 1);
            PlayerPrefs.Save();
        }
        else
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            ShowInstruction();
        }
    }
}
