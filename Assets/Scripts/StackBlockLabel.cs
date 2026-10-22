
using UnityEngine;
using TMPro;

public class StackBlockLabel : MonoBehaviour
{
    public void SetValue(int value)
    {
        GameObject labelObject = new GameObject("ValueLabel");

        labelObject.transform.SetParent(transform, false);

        TextMeshPro label = labelObject.AddComponent<TextMeshPro>();

        label.text = value.ToString();
        label.fontSize = 5;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.black;

        labelObject.transform.localPosition =
            new Vector3(0, 0, -0.51f);

        labelObject.transform.localRotation =
            Quaternion.Euler(0, 180, 0);

        labelObject.transform.localScale =
            Vector3.one * 0.2f;

        label.rectTransform.sizeDelta =
            new Vector2(3, 2);
    }
}
