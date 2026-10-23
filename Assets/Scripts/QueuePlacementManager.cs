
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class QueuePlacementManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject queuePrefab;
    public QueueManager queueManager;

    private GameObject placedQueue;

    private List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    void Update()
    {
        if (placedQueue != null)
            return;

        if (!TryGetTap(out Vector2 tapPosition))
            return;

        if (raycastManager.Raycast(
            tapPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            placedQueue = Instantiate(
                queuePrefab,
                hitPose.position,
                hitPose.rotation
            );

            queueManager.queueBase =
                placedQueue.transform;

            Debug.Log("QueueBase placed successfully!");
        }
        else
        {
            Debug.Log("Tap detected, but no AR plane hit.");
        }
    }

    bool TryGetTap(out Vector2 tapPosition)
    {
        tapPosition = Vector2.zero;

        // Phone touch
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                tapPosition = touch.position.ReadValue();

                return !IsPointerOverUI(tapPosition);
            }
        }

        // Laptop mouse
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            tapPosition = Mouse.current.position.ReadValue();

            return !IsPointerOverUI(tapPosition);
        }

        return false;
    }

    bool IsPointerOverUI(Vector2 position)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData data =
            new PointerEventData(EventSystem.current);

        data.position = position;

        List<RaycastResult> results =
            new List<RaycastResult>();

        EventSystem.current.RaycastAll(data, results);

        return results.Count > 0;
    }
}
