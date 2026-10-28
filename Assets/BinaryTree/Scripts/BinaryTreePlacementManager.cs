
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BinaryTreePlacementManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject binaryTreeBasePrefab;

    public GameObject placedBase;

    private List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    void Update()
    {
        if (placedBase != null)
            return;

        if (!TryGetTap(out Vector2 tapPosition))
            return;

        if (IsPointerOverUI(tapPosition))
            return;

        if (raycastManager == null ||
            binaryTreeBasePrefab == null)
            return;

        if (raycastManager.Raycast(
            tapPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            placedBase = Instantiate(
                binaryTreeBasePrefab,
                hitPose.position,
                hitPose.rotation
            );

            Debug.Log("Binary Tree Base placed successfully!");
        }
    }

    bool TryGetTap(out Vector2 position)
    {
        position = Vector2.zero;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                position = touch.position.ReadValue();
                return true;
            }
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            position = Mouse.current.position.ReadValue();
            return true;
        }

        return false;
    }

    bool IsPointerOverUI(Vector2 position)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData =
            new PointerEventData(EventSystem.current);

        eventData.position = position;

        List<RaycastResult> results =
            new List<RaycastResult>();

        EventSystem.current.RaycastAll(
            eventData, results
        );

        return results.Count > 0;
    }
}
