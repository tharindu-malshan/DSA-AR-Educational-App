
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StackQuizManager : MonoBehaviour
{
    public GameObject quizPanel;

    public TMP_Text questionText;
    public TMP_Text feedbackText;

    public Button[] answerButtons;
    public Button nextQuestionButton;

    private int currentQuestion = 0;
    private int score = 0;
    private bool answered = false;

    private string[] questions =
    {
        "What does LIFO stand for?",
        "Which operation adds an item to a Stack?",
        "Which item is removed first from a Stack?"
    };

    private string[,] answers =
    {
        { "Last In, First Out", "First In, First Out", "Last In, Last Out" },
        { "Pop", "Push", "Peek" },
        { "Bottom item", "Middle item", "Top item" }
    };

    private int[] correctAnswers = { 0, 1, 2 };

    void Start()
    {
        quizPanel.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;

            answerButtons[i].onClick.AddListener(
                () => CheckAnswer(index)
            );
        }

        nextQuestionButton.onClick.AddListener(NextQuestion);
    }

    public void OpenQuiz()
    {
        currentQuestion = 0;
        score = 0;
        quizPanel.SetActive(true);
        foreach (Button button in answerButtons)
    {
        button.gameObject.SetActive(true);
    }
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

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;

            TMP_Text buttonText =
                answerButtons[i].GetComponentInChildren<TMP_Text>();

            buttonText.text = answers[currentQuestion, i];
        }

        nextQuestionButton.gameObject.SetActive(false);
    }

    public void CheckAnswer(int selectedAnswer)
    {
        if (answered)
            return;

        answered = true;

        if (selectedAnswer == correctAnswers[currentQuestion])
        {
            score++;
            feedbackText.text = "Correct Answer!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Wrong Answer!";
            feedbackText.color = Color.red;
        }

        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }

        nextQuestionButton.gameObject.SetActive(true);
    }

    public void NextQuestion()
    {
        currentQuestion++;

        if (currentQuestion < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            questionText.text =
                "Quiz Completed!\nScore: " +
                score + "/" + questions.Length;

            feedbackText.text = "Well Done!";

            foreach (Button button in answerButtons)
            {
                button.gameObject.SetActive(false);
            }

            nextQuestionButton.gameObject.SetActive(false);

            PlayerPrefs.SetInt("StackQuizScore", score);
            PlayerPrefs.Save();
        }
    }
}
