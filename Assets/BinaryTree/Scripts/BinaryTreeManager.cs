
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BinaryTreeManager : MonoBehaviour
{
    public BinaryTreePlacementManager placementManager;
    public GameObject nodePrefab;

    public Material rootMaterial;
    public Material normalMaterial;

    public TMP_InputField valueInput;
    public TMP_Text explanationText;
    
    public Material searchMaterial;

    public TMP_Text codeText;

    private List<int> traversalValues = new List<int>();
    private int traversalIndex = 0;
    private bool isTraversing = false;
    private string traversalType = "";

    private class TreeNode
    {
        public int value;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(int value)
        {
            this.value = value;
        }
    }

    private TreeNode root;
    private bool treeCreated = false;

    private readonly List<GameObject> visualObjects =
        new List<GameObject>();

    void Update()
    {
        if (!treeCreated &&
            placementManager != null &&
            placementManager.placedBase != null)
        {
            root = new TreeNode(50);
            root.left = new TreeNode(30);
            root.right = new TreeNode(70);

            RefreshTree();
            treeCreated = true;

            Debug.Log("Binary Tree created successfully!");
        }
    }

    public void InsertNode()
    {

        isTraversing = false;
        if (!treeCreated)
        {
            ShowMessage("Place BinaryTreeBase first!");
            return;
        }

        if (valueInput == null ||
            !int.TryParse(valueInput.text, out int value))
        {
            ShowMessage("Enter a valid number.");
            return;
        }

        // Keep the first version within three levels.
        if (!CanInsert(root, value, 0))
        {
            ShowMessage("Duplicate value or tree depth limit reached.");
            return;
        }

        root = Insert(root, value);
        RefreshTree();

        ShowMessage("Node " + value + " inserted!");
        valueInput.text = "";
    }

    bool CanInsert(TreeNode node, int value, int depth)
    {
        if (node == null)
            return depth <= 2;

        if (node.value == value)
            return false;

        if (value < node.value)
            return CanInsert(node.left, value, depth + 1);

        return CanInsert(node.right, value, depth + 1);
    }

    TreeNode Insert(TreeNode node, int value)
    {
        if (node == null)
            return new TreeNode(value);

        if (value < node.value)
            node.left = Insert(node.left, value);
        else if (value > node.value)
            node.right = Insert(node.right, value);

        return node;
    }

    void RefreshTree()
    {
        foreach (GameObject obj in visualObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        visualObjects.Clear();

        if (root != null)
            DrawTree(root, 0f, 0, 0.8f, null);
    }

    void DrawTree(
        TreeNode node,
        float x,
        int depth,
        float horizontalOffset,
        GameObject parentNode)
    {
        if (node == null)
            return;

        Transform baseTransform =
            placementManager.placedBase.transform;

        Vector3 position =
            new Vector3(x, 0.95f - depth * 0.32f, 0f);

        GameObject visual =
            Instantiate(nodePrefab, baseTransform);

        visual.name = "Node_" + node.value;
        visual.transform.localPosition = position;
        visual.transform.localRotation = Quaternion.identity;
        visual.transform.localScale =
            new Vector3(0.25f, 0.25f, 0.25f);

        Renderer renderer = visual.GetComponent<Renderer>();

        if (renderer != null)
            renderer.material =
                depth == 0 ? rootMaterial : normalMaterial;

        TMP_Text label =
            visual.GetComponentInChildren<TMP_Text>();

        if (label != null)
            label.text = node.value.ToString();

        visualObjects.Add(visual);

        if (parentNode != null)
            CreateConnection(parentNode, visual, baseTransform);

        DrawTree(
            node.left,
            x - horizontalOffset,
            depth + 1,
            horizontalOffset / 2f,
            visual
        );

        DrawTree(
            node.right,
            x + horizontalOffset,
            depth + 1,
            horizontalOffset / 2f,
            visual
        );
    }

    void CreateConnection(
        GameObject startNode,
        GameObject endNode,
        Transform parent)
    {
        GameObject line =
            GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        line.name = "TreeConnection";
        line.transform.SetParent(parent, false);

        Vector3 start = startNode.transform.localPosition;
        Vector3 end = endNode.transform.localPosition;
        Vector3 direction = end - start;

        line.transform.localPosition = (start + end) / 2f;
        line.transform.localScale =
            new Vector3(0.02f, direction.magnitude / 2f, 0.02f);

        line.transform.localRotation =
            Quaternion.FromToRotation(Vector3.up, direction.normalized);

        visualObjects.Add(line);
    }

    void ShowMessage(string message)
    {
        if (explanationText != null)
            explanationText.text = message;

        Debug.Log(message);
    }

    
    public void DeleteNode()
    {

        isTraversing = false;
    if (!treeCreated)
    {
        ShowMessage("Place BinaryTreeBase first!");
        return;
    }

    if (valueInput == null ||
        !int.TryParse(valueInput.text, out int value))
    {
        ShowMessage("Enter a valid number.");
        return;
    }

    if (!Contains(root, value))
    {
        ShowMessage("Node " + value + " not found!");
        return;
    }

    root = Delete(root, value);

    RefreshTree();

    ShowMessage("Node " + value + " deleted!");
    valueInput.text = "";
    }

    
    bool Contains(TreeNode node, int value)
    {
    if (node == null)
        return false;

    if (node.value == value)
        return true;

    if (value < node.value)
        return Contains(node.left, value);

    return Contains(node.right, value);
    }

    
    TreeNode Delete(TreeNode node, int value)
    {
    if (node == null)
        return null;

    if (value < node.value)
    {
        node.left = Delete(node.left, value);
    }
    else if (value > node.value)
    {
        node.right = Delete(node.right, value);
    }
    else
    {
        // Case 1 and 2: No child or one child
        if (node.left == null)
            return node.right;

        if (node.right == null)
            return node.left;

        // Case 3: Two children
        TreeNode successor = FindMin(node.right);

        node.value = successor.value;

        node.right = Delete(node.right, successor.value);
    }

    return node;
    }

    TreeNode FindMin(TreeNode node)
    {
    while (node.left != null)
        node = node.left;

    return node;
    }

    
    public void SearchNode()
    {

        isTraversing = false;
    if (!treeCreated)
    {
        ShowMessage("Place BinaryTreeBase first!");
        return;
    }

    if (valueInput == null ||
        !int.TryParse(valueInput.text, out int value))
    {
        ShowMessage("Enter a valid number.");
        return;
    }

    // Restore normal colors before searching
    ResetNodeColors();

    TreeNode foundNode = Search(root, value);

    if (foundNode != null)
    {
        HighlightNode(value);
        ShowMessage("Node " + value + " found!");
    }
    else
    {
        ShowMessage("Node " + value + " not found!");
    }
    }
      
    TreeNode Search(TreeNode node, int value)
    {
    if (node == null || node.value == value)
        return node;

    if (value < node.value)
        return Search(node.left, value);

    return Search(node.right, value);
    }

    
    void HighlightNode(int value)
    {
    foreach (GameObject obj in visualObjects)
    {
        if (obj != null &&
            obj.name == "Node_" + value)
        {
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null && searchMaterial != null)
                renderer.material = searchMaterial;

            break;
        }
    }
    }

    
    void ResetNodeColors()
    {
    foreach (GameObject obj in visualObjects)
    {
        if (obj == null || !obj.name.StartsWith("Node_"))
            continue;

        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer == null)
            continue;

        if (obj.name == "Node_" + root.value)
            renderer.material = rootMaterial;
        else
            renderer.material = normalMaterial;
    }
    }

    
    public void StartInorder()
    {
    StartTraversal("Inorder");
    }   

    public void StartPreorder()
    {
    StartTraversal("Preorder");
    }

    public void StartPostorder()
    {
    StartTraversal("Postorder");
    }

    void StartTraversal(string type)
    {
    if (!treeCreated || root == null)
    {
        ShowMessage("Place BinaryTreeBase first!");
        return;
    }

    traversalValues.Clear();
    traversalType = type;
    traversalIndex = 0;

    if (type == "Inorder")
        Inorder(root);
    else if (type == "Preorder")
        Preorder(root);
    else
        Postorder(root);

    isTraversing = true;

    ShowTraversalStep();
    }

    
    void Inorder(TreeNode node)
    {
    if (node == null) return;

    Inorder(node.left);
    traversalValues.Add(node.value);
    Inorder(node.right);
    }

    void Preorder(TreeNode node)
    {
    if (node == null) return;

    traversalValues.Add(node.value);
    Preorder(node.left);
    Preorder(node.right);
    }

    void Postorder(TreeNode node)
    {
    if (node == null) return;

    Postorder(node.left);
    Postorder(node.right);
    traversalValues.Add(node.value);
    }

    
    public void NextTraversalStep()
    {
    if (!isTraversing)
        return;

    traversalIndex++;

    if (traversalIndex >= traversalValues.Count)
    {
        isTraversing = false;
        ResetNodeColors();

        ShowMessage(traversalType + " traversal completed!");

        if (codeText != null)
            codeText.text = "Traversal completed.";

        return;
    }

    ShowTraversalStep();
    }

    
    void ShowTraversalStep()
    {
    if (traversalIndex >= traversalValues.Count)
        return;

    ResetNodeColors();

    int currentValue = traversalValues[traversalIndex];

    HighlightNode(currentValue);

    ShowMessage(
        traversalType + " - Step " +
        (traversalIndex + 1) +
        ": Visiting node " + currentValue
    );

    if (codeText != null)
    {
        string code = "";

        if (traversalType == "Inorder")
        {
            code =
                "Inorder(node.left);\n" +
                "<color=yellow>Visit(node);</color>\n" +
                "Inorder(node.right);";
        }
        else if (traversalType == "Preorder")
        {
            code =
                "<color=yellow>Visit(node);</color>\n" +
                "Preorder(node.left);\n" +
                "Preorder(node.right);";
        }
        else
        {
            code =
                "Postorder(node.left);\n" +
                "Postorder(node.right);\n" +
                "<color=yellow>Visit(node);</color>";
        }

        codeText.text = code;
    }
    }

    
    public void ResetTraversal()
    {
    isTraversing = false;
    traversalIndex = 0;
    traversalValues.Clear();

    if (!treeCreated || root == null)
        return;

    ResetNodeColors();

    ShowMessage("Traversal reset.");

    if (codeText != null)
        codeText.text = "Select a traversal method.";
    }

    
public bool ChallengeInsert(int value)
{
    if (!treeCreated || root == null)
        return false;

    if (!CanInsert(root, value, 0))
        return false;

    isTraversing = false;

    root = Insert(root, value);
    RefreshTree();

    return true;
}

    public bool ChallengeDelete(int value)
    {
    if (!treeCreated || root == null)
        return false;

    if (!Contains(root, value))
        return false;

    isTraversing = false;

    root = Delete(root, value);
    RefreshTree();

    return true;    
    }   

    public bool IsTreeReady()
    {
    return treeCreated && root != null;
    }

    public bool TreeContains(int value)
    {
    return root != null && Contains(root, value);
    }


}
