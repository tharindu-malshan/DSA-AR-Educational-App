
using UnityEngine;
using TMPro;

public class QueueBlockLabel : MonoBehaviour
{
    public void SetLabel(int value)
    {
        GameObject labelObject = new GameObject("NumberLabel");

        labelObject.transform.SetParent(
            transform, false
        );

        labelObject.transform.localPosition =
            new Vector3(0f, 0f, -0.51f);

        labelObject.transform.localRotation =
            Quaternion.identity;

        labelObject.transform.localScale =
            Vector3.one * 0.1f;

        TextMeshPro text =
            labelObject.AddComponent<TextMeshPro>();

        text.text = value.ToString();
        text.fontSize = 5;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        RectTransform rect =
            text.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(10, 5);
    }
}
