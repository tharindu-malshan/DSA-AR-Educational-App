
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueueQuizManager : MonoBehaviour
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
        "What does FIFO stand for?",
        "Which operation adds an item to a Queue?",
        "Which item is removed first from a Queue?"
    };

    private string[,] answers =
    {
        {
            "First In, First Out",
            "Last In, First Out",
            "First In, Last Out"
        },
        {
            "Dequeue",
            "Enqueue",
            "Peek"
        },
        {
            "REAR item",
            "Middle item",
            "FRONT item"
        }
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

        foreach (Button button in answerButtons)
            button.gameObject.SetActive(true);

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

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;

            TMP_Text text =
                answerButtons[i].GetComponentInChildren<TMP_Text>();

            text.text = answers[currentQuestion, i];
        }

        nextQuestionButton.gameObject.SetActive(false);
    }

    void CheckAnswer(int selectedAnswer)
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
            button.interactable = false;

        nextQuestionButton.gameObject.SetActive(true);
    }

    void NextQuestion()
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
            feedbackText.color = Color.green;

            foreach (Button button in answerButtons)
                button.gameObject.SetActive(false);

            nextQuestionButton.gameObject.SetActive(false);

            PlayerPrefs.SetInt("QueueQuizScore", score);
            PlayerPrefs.Save();
        }
    }
}
