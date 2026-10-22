using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDebugger : MonoBehaviour
{
    public ARPlaneManager planeManager;

    void Update()
    {
        if (planeManager == null)
            return;

        int count = 0;

        foreach (ARPlane plane in planeManager.trackables)
        {
            count++;
        }

        if (Time.frameCount % 120 == 0)
        {
            Debug.Log("Detected AR Planes: " + count);
        }
    }
}