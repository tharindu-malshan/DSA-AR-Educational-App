
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public TMP_Text lessonText;
    public TMP_Text codeText;
    public int maxItems = 5;
    public Material blueMaterial;
    public Material greenMaterial;

    private Stack<int> stack = new Stack<int>();
    private List<string> operationHistory = new List<string>();

    public List<string> GetOperationHistory()
    {
    return new List<string>(operationHistory);
    }
    private List<GameObject> blocks = new List<GameObject>();

    private int nextValue = 10;
    private int currentStep = 0;
    private Transform arCamera;
    public Transform stackBase;

    public float animationDuration = 0.5f;
    public float animationDistance = 0.35f;

    private bool isAnimating = false;

    private readonly string[] codeLines =
    {
        "Stack<int> stack = new Stack<int>();",
        "stack.Push(value);",
        "int item = stack.Pop();"
    };

    public StackChallengeManager challengeManager;

    void Start()
    {
        if (Camera.main != null)
            arCamera = Camera.main.transform;

        ShowLesson("Stack uses LIFO: Last In, First Out.");
        ShowCode();
    }

   public void Push()
{

    if (isAnimating)
    return;

    if (stackBase == null)
    {
        ShowLesson("First tap the floor to place the Stack!");
        return;
    }

    if (stack.Count >= maxItems)
    {
        ShowLesson("Stack is full!");
        return;
    }

    stack.Push(nextValue);
    operationHistory.Add("Push " + nextValue);
    if (challengeManager != null)
  {
    challengeManager.CheckOperation("Push " + nextValue);
  }

    GameObject block = GameObject.CreatePrimitive(
        PrimitiveType.Cube
    );

    block.name = "Stack Item " + nextValue;

    // Keep the cube at its normal world scale
    block.transform.SetParent(stackBase, true);

    float blockSize = 0.18f;

// Set the cube size in world space
    block.transform.localScale = new Vector3(
    blockSize / stackBase.lossyScale.x,
    blockSize / stackBase.lossyScale.y,
    blockSize / stackBase.lossyScale.z
    );

// Find the height of the StackBase
    Renderer baseRenderer = stackBase.GetComponent<Renderer>();

    float baseTop = stackBase.position.y;

    if (baseRenderer != null)
    {
    baseTop = baseRenderer.bounds.max.y;
    }

// Position each new cube above the StackBase
    block.transform.position = new Vector3(
    stackBase.position.x,
    baseTop + blockSize / 2f + blocks.Count * blockSize,
    stackBase.position.z
    );

    StackBlockLabel blockLabel =
    block.AddComponent<StackBlockLabel>();

    blockLabel.SetValue(nextValue);

    blocks.Add(block);

    UpdateStackColors();

    Vector3 targetPosition = block.transform.position;

    StartCoroutine(AnimatePush(block, targetPosition));

    nextValue += 10;
    currentStep = 1;

    ShowLesson(
    "PUSH OPERATION\n" +
    "Value " + (nextValue - 10) + " added to the TOP.\n" +
    "Stack Size: " + stack.Count + "\n" +
    "LIFO: Last In, First Out."
   );
    ShowCode();
}

    public void Pop()
    {

        if (isAnimating)
        return;  

        if (stack.Count == 0)
        {
            ShowLesson("Stack is empty! Nothing to pop.");
            return;
        }

        int removedValue = stack.Pop();
        operationHistory.Add("Pop " + removedValue);
        if (challengeManager != null)
    {
    challengeManager.CheckOperation("Pop " + removedValue);
    }
        GameObject topBlock = blocks[blocks.Count - 1];

        blocks.RemoveAt(blocks.Count - 1);

        StartCoroutine(AnimatePop(topBlock));

        currentStep = 2;
        ShowLesson(
        "POP OPERATION\n" +
        "Removed value: " + removedValue + "\n" +
        "Stack Size: " + stack.Count + "\n" +
        "The last inserted item is removed first."
        );
        ShowCode();
    }

   
    
 public void NextStep()
 {
    // Wait until the current animation finishes
    if (isAnimating)
        return;

    // First place the StackBase on the AR floor
    if (stackBase == null)
    {
        ShowLesson("First tap the floor to place the Stack!");
        return;
    }

    // Move to the next learning step
    currentStep = (currentStep + 1) % codeLines.Length;

    if (currentStep == 0)
    {
        ResetStack();

        ShowLesson(
            "STEP 1: CREATE STACK\n" +
            "The Stack is empty.\n" +
            "Stack follows LIFO."
        );
    }
    else if (currentStep == 1)
    {
        // Perform real Push animation
        Push();
    }
    else if (currentStep == 2)
    {
        // Perform real Pop animation
        Pop();
    }

    ShowCode();
 }



    public void ResetStack()
    {

        if (isAnimating)
        return;

        foreach (GameObject block in blocks)
        {
            if (block != null)
                Destroy(block);
        }

        blocks.Clear();
        stack.Clear();
        operationHistory.Clear();
        nextValue = 10;
        currentStep = 0;

        ShowLesson("Stack reset. Press Push to begin.");
        ShowCode();
    }

    private void ShowLesson(string message)
    {
        if (lessonText != null)
            lessonText.text = message;
    }

   
    private void ShowCode()
  {
    if (codeText == null)
        return;

    string display = "";

    for (int i = 0; i < codeLines.Length; i++)
    {
        if (i == currentStep)
        {
            display +=
               "<color=#00FF88><b>> " +
                codeLines[i] +
                "</b></color>\n";
        }
        else
        {
            display +=
                "<color=#FFFFFF>" +
                codeLines[i] +
                "</color>\n";
        }
    }

    codeText.text = display;
 }


    
    
    private void UpdateStackColors()
    {
    for (int i = 0; i < blocks.Count; i++)
    {
        GameObject block = blocks[i];

        if (block == null)
            continue;

        Renderer blockRenderer = block.GetComponent<Renderer>();

        if (blockRenderer == null)
            continue;

        if (i == blocks.Count - 1)
        {
            // TOP cube = Green
            if (greenMaterial != null)
                blockRenderer.sharedMaterial = greenMaterial;
        }
        else
        {
            // Other cubes = Blue
            if (blueMaterial != null)
                blockRenderer.sharedMaterial = blueMaterial;
        }
    }
    }


    
private IEnumerator AnimatePush(
    GameObject block,
    Vector3 targetPosition)
{
    isAnimating = true;

    Vector3 startPosition =
        targetPosition + Vector3.up * animationDistance;

    block.transform.position = startPosition;

    float elapsed = 0f;

    while (elapsed < animationDuration)
    {
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(
            elapsed / animationDuration
        );

        block.transform.position = Vector3.Lerp(
            startPosition,
            targetPosition,
            t
        );

        yield return null;
    }

    block.transform.position = targetPosition;

    isAnimating = false;
}

private IEnumerator AnimatePop(GameObject block)
 {
    isAnimating = true;

    Vector3 startPosition = block.transform.position;

    Vector3 endPosition =
        startPosition + Vector3.up * animationDistance;

    float elapsed = 0f;

    while (elapsed < animationDuration)
    {
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(
            elapsed / animationDuration
        );

        block.transform.position = Vector3.Lerp(
            startPosition,
            endPosition,
            t
        );

        yield return null;
    }

    Destroy(block);

    isAnimating = false;
    UpdateStackColors();
 }

 
   public int GetStackCount()
  {
    return stack.Count;
  }
   
   
    public int GetTopValue()
    {
    if (stack.Count == 0)
        return -1;

    return stack.Peek();
    }

    public bool IsStackAnimating()
    {
    return isAnimating;
    }



}
