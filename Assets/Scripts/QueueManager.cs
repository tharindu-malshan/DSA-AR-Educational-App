
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QueueManager : MonoBehaviour
{
    public Transform queueBase;
    public TMP_Text lessonText;
    public TMP_Text codeText;

    public int maxItems = 5;
    public float animationDuration = 0.5f;

    private Queue<int> queue = new Queue<int>();
    private List<GameObject> blocks = new List<GameObject>();

    private int nextValue = 10;
    private bool isAnimating = false;

    
    private int currentStep = 0;

    private readonly string[] codeLines =
    {
    "Queue<int> queue = new Queue<int>();",
    "queue.Enqueue(value);",
    "int item = queue.Dequeue();"
    };

    public QueueChallengeManager challengeManager;


    public void Enqueue()
    {
        if (queueBase == null || isAnimating)
            return;

        if (queue.Count >= maxItems)
        {
            lessonText.text = "Queue Overflow! Queue is full.";
            return;
        }

        if (challengeManager != null &&
        !challengeManager.CanPerformOperation("Enqueue", nextValue))
        {
        return;
        }

        int value = nextValue;
        nextValue += 10;

        queue.Enqueue(value);

        if (challengeManager != null)
        challengeManager.OperationCompleted("Enqueue", value);

        GameObject block =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        block.name = "QueueBlock_" + value;

        block.transform.SetParent(queueBase, false);

        block.transform.localScale =
            new Vector3(0.22f, 0.25f, 0.35f);

        QueueBlockLabel label =
            block.AddComponent<QueueBlockLabel>();

        label.SetLabel(value);

        blocks.Add(block);
        UpdateColors();

        Vector3 targetPosition =
            GetBlockPosition(blocks.Count - 1);

        Vector3 startPosition =
            targetPosition + Vector3.up * 0.6f;

        block.transform.localPosition = startPosition;

        lessonText.text =
            "Enqueue " + value + "\nAdded at REAR.";

        ShowCode(1);

        StartCoroutine(
            AnimateMove(block.transform,
                        startPosition,
                        targetPosition)
        );
    }

    public void Dequeue()
    {
        if (queueBase == null || isAnimating)
            return;

        if (queue.Count == 0)
        {
            lessonText.text =
                "Queue Underflow! Queue is empty.";
            return;
        }
          
        if (challengeManager != null &&
        !challengeManager.CanPerformOperation("Dequeue", queue.Peek()))
        {
        return;
        }  
        int removed = queue.Dequeue();

        if (challengeManager != null)
        challengeManager.OperationCompleted("Dequeue", removed);

        GameObject frontBlock = blocks[0];
        blocks.RemoveAt(0);

        lessonText.text =
            "Dequeue " + removed +
            "\nRemoved from FRONT.";

        ShowCode(2);

        StartCoroutine(AnimateDequeue(frontBlock));
    }

    IEnumerator AnimateMove(
        Transform block,
        Vector3 start,
        Vector3 end)
    {
        isAnimating = true;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(0.01f, animationDuration)
            );

            t = Mathf.SmoothStep(0f, 1f, t);

            block.localPosition =
                Vector3.Lerp(start, end, t);

            yield return null;
        }

        block.localPosition = end;
        isAnimating = false;
    }

    IEnumerator AnimateDequeue(GameObject frontBlock)
    {
        isAnimating = true;

        Vector3 start = frontBlock.transform.localPosition;
        Vector3 end = start + Vector3.up * 0.6f;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(
                    elapsed / Mathf.Max(0.01f, animationDuration)
                )
            );

            frontBlock.transform.localPosition =
                Vector3.Lerp(start, end, t);

            yield return null;
        }

        Destroy(frontBlock);

        // Slide remaining blocks toward FRONT
        List<Vector3> startPositions = new List<Vector3>();

        foreach (GameObject block in blocks)
            startPositions.Add(block.transform.localPosition);

        elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f,
                Mathf.Clamp01(
                    elapsed / Mathf.Max(0.01f, animationDuration)
                )
            );

            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].transform.localPosition =
                    Vector3.Lerp(
                        startPositions[i],
                        GetBlockPosition(i),
                        t
                    );
            }

            yield return null;
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.localPosition =
                GetBlockPosition(i);
        }

        UpdateColors();
        isAnimating = false;
    }

    Vector3 GetBlockPosition(int index)
    {
        return new Vector3(
            -0.55f + index * 0.28f,
            0.225f,
            0f
        );
    }

    void UpdateColors()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Renderer renderer =
                blocks[i].GetComponent<Renderer>();

            if (i == 0)
                renderer.material.color = Color.green;
            else if (i == blocks.Count - 1)
                renderer.material.color =
                    new Color(1f, 0.5f, 0f);
            else
                renderer.material.color = Color.blue;
        }
    }

    public void ResetQueue()
    {
        if (isAnimating)
            return;

        foreach (GameObject block in blocks)
            Destroy(block);

        blocks.Clear();
        queue.Clear();
        nextValue = 10;

        lessonText.text = "Queue Reset!";
       ShowCode(0);
    }

    
    void ShowCode(int highlightedLine)
    {
    if (codeText == null)
        return;

    string result = "";

    for (int i = 0; i < codeLines.Length; i++)
    {
        if (i == highlightedLine)
        {
            result +=
                "<color=#00FF00><b>" +
                codeLines[i] +
                "</b></color>\n";
        }
        else
        {
            result += codeLines[i] + "\n";
        }
    }

    codeText.text = result;
    }
         
    
    public void NextStep()
    {
    if (isAnimating || queueBase == null)
        return;

    if (currentStep == 0)
    {
        ResetQueue();

        lessonText.text =
            "Step 1: Create an empty Queue.\n" +
            "Queue follows FIFO.";

        ShowCode(0);
    }
    else if (currentStep == 1)
    {
        Enqueue();

        lessonText.text =
            "Step 2: Enqueue an item at REAR.";

        ShowCode(1);
    }
    else if (currentStep == 2)
    {
        Enqueue();

        lessonText.text =
            "Step 3: Enqueue another item at REAR.";

        ShowCode(1);
    }
    else if (currentStep == 3)
    {
        Dequeue();

        lessonText.text =
            "Step 4: Remove the FRONT item.";

        ShowCode(2);
    }

    currentStep = (currentStep + 1) % 4;
    }
     
}
