
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BinaryTreeChallengeManager : MonoBehaviour
{
    public BinaryTreeManager treeManager;

    public GameObject challengePanel;
    public TMP_Text instructionText;
    public TMP_Text feedbackText;
    public TMP_InputField challengeInput;

    public Button submitButton;
    public Button nextButton;

    private int challengeIndex = 0;
    private bool challengeCompleted = false;

    void Start()
    {
        challengePanel.SetActive(false);
    }

    public void OpenChallenge()
    {
        if (treeManager == null || !treeManager.IsTreeReady())
        {
            Debug.Log("Place BinaryTreeBase first!");
            return;
        }

        challengeIndex = 0;
        challengePanel.SetActive(true);

        ShowChallenge();
    }

    public void CloseChallenge()
    {
        challengePanel.SetActive(false);
    }

    void ShowChallenge()
    {
        challengeCompleted = false;
        challengeInput.text = "";
        feedbackText.text = "";

        submitButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);

        if (challengeIndex == 0)
        {
            instructionText.text =
                "Challenge 1: Insert node 20 into the BST.";
        }
        else if (challengeIndex == 1)
        {
            instructionText.text =
                "Challenge 2: Delete node 70 from the BST.";
        }
    }

    public void SubmitChallenge()
    {
        if (challengeCompleted)
            return;

        if (!int.TryParse(challengeInput.text, out int value))
        {
            ShowFeedback("Enter a valid number!", false);
            return;
        }

        if (challengeIndex == 0)
        {
            if (value != 20)
            {
                ShowFeedback("Wrong! Insert node 20.", false);
                return;
            }

            if (treeManager.TreeContains(20))
            {
                ShowFeedback(
                    "Node 20 already exists. Reset the tree before retrying.",
                    false
                );
                return;
            }

            if (!treeManager.ChallengeInsert(20))
            {
                ShowFeedback("Cannot insert node 20.", false);
                return;
            }

            CompleteChallenge("Correct! Node 20 inserted.");
        }
        else if (challengeIndex == 1)
        {
            if (value != 70)
            {
                ShowFeedback("Wrong! Delete node 70.", false);
                return;
            }

            if (!treeManager.ChallengeDelete(70))
            {
                ShowFeedback("Node 70 not found!", false);
                return;
            }

            CompleteChallenge("Correct! Node 70 deleted.");
        }
    }

    void CompleteChallenge(string message)
    {
        challengeCompleted = true;

        ShowFeedback(message, true);

        submitButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);

        if (challengeIndex == 1)
            nextButton.GetComponentInChildren<TMP_Text>().text =
                "Finish";
        else
            nextButton.GetComponentInChildren<TMP_Text>().text =
                "Next Challenge";
    }

    void ShowFeedback(string message, bool correct)
    {
        feedbackText.text = message;
        feedbackText.color =
            correct ? Color.green : Color.red;
    }

    public void NextChallenge()
    {
        if (!challengeCompleted)
            return;

        challengeIndex++;

        if (challengeIndex >= 2)
        {
            instructionText.text =
                "All challenges completed!";

            feedbackText.text =
                "Great job! You completed the BST challenges.";

            feedbackText.color = Color.green;

            submitButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            PlayerPrefs.SetInt("BinaryTreeChallengeCompleted", 1);
            PlayerPrefs.Save();

            return;
        }

        ShowChallenge();
    }
}
