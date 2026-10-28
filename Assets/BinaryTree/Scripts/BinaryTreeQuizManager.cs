
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BinaryTreeQuizManager : MonoBehaviour
{
    public GameObject quizPanel;

    public TMP_Text questionText;
    public TMP_Text feedbackText;

    public Button answerA;
    public Button answerB;
    public Button answerC;
    public Button nextButton;

    private int currentQuestion = 0;
    private int score = 0;
    private bool answered = false;

    private string[] questions =
    {
        "What is the topmost node of a Binary Tree called?",
        "In a BST, smaller values are stored on which side?",
        "Which traversal follows Left, Root, Right?",
        "What is a node with no children called?",
        "Which traversal visits the Root first?"
    };

    private string[,] answers =
    {
        { "Leaf", "Root", "Child" },
        { "Left", "Right", "Both" },
        { "Preorder", "Postorder", "Inorder" },
        { "Root", "Parent", "Leaf" },
        { "Preorder", "Inorder", "Postorder" }
    };

    private int[] correctAnswers = { 1, 0, 2, 2, 0 };

    void Start()
    {
        quizPanel.SetActive(false);
    }

    public void OpenQuiz()
    {
        currentQuestion = 0;
        score = 0;

        answerA.gameObject.SetActive(true);
        answerB.gameObject.SetActive(true);
        answerC.gameObject.SetActive(true);

        quizPanel.SetActive(true);
        ShowQuestion();
    }

    public void CloseQuiz()
    {
        quizPanel.SetActive(false);
    }

    void ShowQuestion()
    {
        answered = false;

        questionText.text = questions[currentQuestion];
        feedbackText.text = "";

        answerA.GetComponentInChildren<TMP_Text>().text =
            answers[currentQuestion, 0];

        answerB.GetComponentInChildren<TMP_Text>().text =
            answers[currentQuestion, 1];

        answerC.GetComponentInChildren<TMP_Text>().text =
            answers[currentQuestion, 2];

        answerA.interactable = true;
        answerB.interactable = true;
        answerC.interactable = true;

        nextButton.gameObject.SetActive(false);
    }

    public void SelectAnswer(int answerIndex)
    {
        if (answered) return;

        answered = true;

        if (answerIndex == correctAnswers[currentQuestion])
        {
            score++;
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;
        }

        answerA.interactable = false;
        answerB.interactable = false;
        answerC.interactable = false;

        nextButton.gameObject.SetActive(true);
    }

    public void NextQuestion()
    {
        currentQuestion++;

        if (currentQuestion >= questions.Length)
        {
            questionText.text =
                "Quiz Completed! Score: " +
                score + "/" + questions.Length;

            feedbackText.text = "Well done!";
            feedbackText.color = Color.green;

            answerA.gameObject.SetActive(false);
            answerB.gameObject.SetActive(false);
            answerC.gameObject.SetActive(false);

            nextButton.gameObject.SetActive(false);

            PlayerPrefs.SetInt("BinaryTreeQuizScore", score);
            PlayerPrefs.Save();

            return;
        }

        ShowQuestion();
    }
}
