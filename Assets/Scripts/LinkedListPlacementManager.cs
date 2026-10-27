
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class LinkedListPlacementManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject linkedListBasePrefab;

    public GameObject placedBase;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (placedBase != null)
            return;

        Vector2 screenPosition;

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            screenPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null &&
                 Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        else
        {
            return;
        }

        if (IsPointerOverUI(screenPosition))
            return;

        if (raycastManager == null || linkedListBasePrefab == null)
            return;

        if (raycastManager.Raycast(
            screenPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            placedBase = Instantiate(
                linkedListBasePrefab,
                pose.position,
                pose.rotation
            );

            Debug.Log("LinkedListBase placed successfully!");
        }
    }

    bool IsPointerOverUI(Vector2 position)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData =
            new PointerEventData(EventSystem.current);

        pointerData.position = position;

        List<RaycastResult> results =
            new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }
}
