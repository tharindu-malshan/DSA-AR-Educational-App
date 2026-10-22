
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class StackPlacementManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject stackPrefab;
    public StackManager stackManager;

    private GameObject placedStack;

    private static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    void Update()
  {
    if (placedStack != null)
        return;

    Vector2 screenPosition;

    if (!TryGetTapPosition(out screenPosition))
        return;

    Debug.Log("Click detected at: " + screenPosition);

    if (raycastManager == null)
    {
        Debug.LogError("Raycast Manager is NOT assigned!");
        return;
    }

    if (stackPrefab == null)
    {
        Debug.LogError("Stack Prefab is NOT assigned!");
        return;
    }

    bool hitPlane = raycastManager.Raycast(
        screenPosition,
        hits,
        TrackableType.PlaneWithinPolygon
    );

    if (hitPlane && hits.Count > 0)
    {
        Pose hitPose = hits[0].pose;

        placedStack = Instantiate(
            stackPrefab,
            hitPose.position,
            hitPose.rotation
        );

        if (stackManager != null)
       {
             stackManager.stackBase = placedStack.transform;
         }

        Debug.Log("StackBase placed successfully!");
    }
    else
    {
        Debug.LogWarning("Click detected, but NO plane was hit.");
    }
   }

    private bool TryGetTapPosition(out Vector2 position)
    {
        position = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            position =
                Touchscreen.current.primaryTouch.position.ReadValue();
            return true;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            position = Mouse.current.position.ReadValue();
            return true;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            position = Input.GetTouch(0).position;
            return true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            position = Input.mousePosition;
            return true;
        }
#endif

        return false;
    }

    public void RemovePlacedStack()
    {
        if (placedStack != null)
        {
            Destroy(placedStack);
            placedStack = null;
        }
    }
}
