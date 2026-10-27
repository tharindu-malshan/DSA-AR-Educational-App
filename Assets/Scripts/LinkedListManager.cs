
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinkedListManager : MonoBehaviour
{
    public LinkedListPlacementManager placementManager;
    public GameObject nodePrefab;

    public Material headMaterial;
    public Material normalMaterial;

    public LinkedListChallengeManager challengeManager;
    public bool IsListCreated => listCreated;

    
    public Material searchMaterial;
    public TMP_Text explanationText;
    public TMP_InputField searchInput;


    private List<GameObject> nodes = new List<GameObject>();
    private List<GameObject> arrows = new List<GameObject>();

    private List<int> values = new List<int>();

    
    public TMP_Text codeText;

    private bool isTraversing = false;
    private int traversalIndex = 0;


    private bool listCreated = false;
    private int nextValue = 40;

    private const int maxNodes = 5;

    void Update()
    {
        if (!listCreated &&
            placementManager != null &&
            placementManager.placedBase != null)
        {
            values.AddRange(new int[] { 10, 20, 30 });
            RefreshList();
            listCreated = true;
        }
    }

    
    public void InsertNode()
    {
    isTraversing = false;

    if (!listCreated) return;

    if (values.Count >= maxNodes)
    {
        if (explanationText != null)
            explanationText.text = "Linked List is full!";
        return;
    }

    if (challengeManager != null &&
        !challengeManager.CanPerformOperation("Insert", nextValue))
        return;

    int insertedValue = nextValue;

    values.Add(insertedValue);
    nextValue += 10;

    RefreshList();

    if (explanationText != null)
        explanationText.text =
            "Node " + insertedValue + " inserted at the end.";

    if (challengeManager != null)
        challengeManager.OperationCompleted(
            "Insert", insertedValue
        );
    }

    
    public void DeleteNode()
    {
    isTraversing = false;

    if (!listCreated) return;

    if (values.Count == 0)
    {
        if (explanationText != null)
            explanationText.text = "Linked List is empty!";
        return;
    }

    int deletedValue = values[values.Count - 1];

    if (challengeManager != null &&
        !challengeManager.CanPerformOperation("Delete", deletedValue))
        return;

    values.RemoveAt(values.Count - 1);

    RefreshList();

    if (explanationText != null)
        explanationText.text =
            "Node " + deletedValue + " deleted.";

    if (challengeManager != null)
        challengeManager.OperationCompleted(
            "Delete", deletedValue
        );
    }


    public void ResetList()
    {

        if (challengeManager != null &&
        challengeManager.IsChallengeActive)
    {
        if (explanationText != null)
        explanationText.text =
            "Reset is disabled during the challenge.";
        return;
    }
        isTraversing = false;
        if (!listCreated) return;

        values.Clear();
        values.AddRange(new int[] { 10, 20, 30 });
        nextValue = 40;

        RefreshList();

        if (explanationText != null)
        explanationText.text = "Linked List reset successfully.";
    }

    void RefreshList()
    {
        foreach (GameObject node in nodes)
            Destroy(node);

        foreach (GameObject arrow in arrows)
            Destroy(arrow);

        nodes.Clear();
        arrows.Clear();

        for (int i = 0; i < values.Count; i++)
            CreateNode(values[i], i);

        CreateArrows();
        UpdateNodeColors();
    }

    void CreateNode(int value, int index)
    {
        Transform parent =
            placementManager.placedBase.transform;

        GameObject node = Instantiate(nodePrefab, parent);

        node.transform.localPosition =
            new Vector3(-0.55f + index * 0.55f, 0.23f, 0f);

        node.transform.localRotation = Quaternion.identity;

        node.transform.localScale =
            new Vector3(0.25f, 0.25f, 0.25f);

        TMP_Text label =
            node.GetComponentInChildren<TMP_Text>();

        if (label != null)
            label.text = value.ToString();

        nodes.Add(node);
    }

    void UpdateNodeColors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            Renderer renderer =
                nodes[i].GetComponent<Renderer>();

            if (renderer != null)
                renderer.material =
                    (i == 0) ? headMaterial : normalMaterial;
        }
    }

    void CreateArrows()
    {
        Transform parent =
            placementManager.placedBase.transform;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            GameObject arrow =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            arrow.name = "Arrow_" + i;
            arrow.transform.SetParent(parent, false);

            arrow.transform.localPosition =
                new Vector3(-0.275f + i * 0.55f, 0.23f, 0f);

            arrow.transform.localScale =
                new Vector3(0.25f, 0.035f, 0.035f);

            arrows.Add(arrow);
        }
    }

    
    public void SearchNode()
    {
        isTraversing = false;
    if (!listCreated)
    {
        Debug.Log("Place LinkedListBase first!");
        return;
    }

    if (searchInput == null || explanationText == null)
        return;

    int searchValue;

    if (!int.TryParse(searchInput.text, out searchValue))
    {
        explanationText.text = "Please enter a valid number.";
        return;
    }

    UpdateNodeColors();

    for (int i = 0; i < values.Count; i++)
    {
        if (values[i] == searchValue)
        {
            Renderer renderer = nodes[i].GetComponent<Renderer>();

            if (renderer != null && searchMaterial != null)
                renderer.material = searchMaterial;

            explanationText.text =
                "Node " + searchValue +
                " found at position " + (i + 1) + "!";

            return;
        }
    }

    explanationText.text =
        "Node " + searchValue + " not found!";
    }

    
    public void StartTraversal()
    {
    if (!listCreated)
    {
        if (explanationText != null)
            explanationText.text = "Place LinkedListBase first!";
        return;
    }

    if (values.Count == 0)
    {
        if (explanationText != null)
            explanationText.text = "Linked List is empty.";
        return;
    }

    isTraversing = true;
    traversalIndex = 0;

    ShowTraversalStep();
    }
        
     
    public void NextTraversalStep()
    {
    if (!isTraversing)
        return;

    traversalIndex++;

    if (traversalIndex >= values.Count)
    {
        isTraversing = false;
        UpdateNodeColors();

        if (explanationText != null)
            explanationText.text =
                "Traversal completed! Reached NULL.";

        if (codeText != null)
            codeText.text =
                "while (current != null)\n" +
                "{\n" +
                "    Visit(current);\n" +
                "    current = current.next;\n" +
                "}\n" +
                "// Traversal complete";

        return;
    }

    ShowTraversalStep();
    }
   
   
    void ShowTraversalStep()
    {
    UpdateNodeColors();

    Renderer renderer =
        nodes[traversalIndex].GetComponent<Renderer>();

    if (renderer != null && searchMaterial != null)
        renderer.material = searchMaterial;

    if (explanationText != null)
        explanationText.text =
            "Step " + (traversalIndex + 1) +
            ": Visiting node " + values[traversalIndex];

    if (codeText != null)
        codeText.text =
            "<color=yellow>Visit(current);</color>\n" +
            "current = current.next;";
    }


}
