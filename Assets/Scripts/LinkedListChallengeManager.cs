
using UnityEngine;
using TMPro;

public class LinkedListChallengeManager : MonoBehaviour
{
    public GameObject challengePanel;
    public TMP_Text instructionText;
    public TMP_Text feedbackText;
    public LinkedListManager linkedListManager;
    public bool IsChallengeActive => challengeActive;

    private bool challengeActive = false;
    private int currentStep = 0;

    private readonly string[] operations =
    {
        "Insert", "Delete", "Insert"
    };

    private readonly int[] expectedValues =
    {
        40, 40, 50
    };

    void Start()
    {
        challengePanel.SetActive(false);
    }

    public void OpenChallenge()
    {
        if (!linkedListManager.IsListCreated)
        {
            Debug.LogWarning("Place LinkedListBase first!");
            return;
        }

        challengeActive = false;
        linkedListManager.ResetList();

        currentStep = 0;
        challengeActive = true;

        challengePanel.SetActive(true);
        feedbackText.text = "Challenge started!";
        feedbackText.color = Color.white;

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
                "Step " + (currentStep + 1) + ": " +
                operations[currentStep] + " " +
                expectedValues[currentStep];
        }
    }

    public bool CanPerformOperation(string operation, int value)
    {
        if (!challengeActive)
            return true;

        if (operation == operations[currentStep] &&
            value == expectedValues[currentStep])
        {
            return true;
        }

        feedbackText.text =
            "Wrong operation! Try: " +
            operations[currentStep] + " " +
            expectedValues[currentStep];

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

            instructionText.text = "Challenge Completed!";
            feedbackText.text =
                "Correct! Final List: 10, 20, 30, 50";

            feedbackText.color = Color.green;

            PlayerPrefs.SetInt(
                "LinkedListChallengeCompleted", 1
            );

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
