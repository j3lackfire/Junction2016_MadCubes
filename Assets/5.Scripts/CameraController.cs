using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Vector3 defaultCameraRotation;
    private Vector3 currentCameraRotation;

    protected void Awake()
    {        
    }

    public void Init()
    {
        defaultCameraRotation = transform.localEulerAngles;
        currentCameraRotation = defaultCameraRotation;
    }

    public void DoUpdate()
    {
        //CheckForPlayerInput();
    }

    public void ScreenShake(ScreenShakeMagnitude magnitude)
    {
        switch (magnitude)
        {
            case ScreenShakeMagnitude.Small:
                break;
        }
    }

    public void FollowCargo(Vector3 delta)
    {
        transform.position += delta;
    }
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
    Unassigned
}

public enum ScreenShakeMagnitude
{
    Small,
    Medium,
    Big,
    Gigantic
}