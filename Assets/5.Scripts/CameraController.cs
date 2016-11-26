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
        CheckForPlayerInput();
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(Direction.Left);
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveCamera(Direction.Left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveCamera(Direction.Right);
        }

        if (Input.GetKey(KeyCode.W))
        {
            MoveCamera(Direction.Up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveCamera(Direction.Down);
        }
    }

    public void RotateCamera(Direction direction)
    {
        if (direction == Direction.Left)
        {
            currentCameraRotation += new Vector3(0f, -GameConstant.cameraRotateAngle, 0f);
            if (currentCameraRotation.y < 0f)
            {
                currentCameraRotation += new Vector3(0f, 360f, 0f);
            }
        } else
        {
            currentCameraRotation += new Vector3(0f, GameConstant.cameraRotateAngle, 0f);
            if (currentCameraRotation.y >= 360f)
            {
                currentCameraRotation += new Vector3(0f, -360f, 0f);
            }
        }
        transform.localRotation = Quaternion.Euler(currentCameraRotation);
    }

    public void MoveCamera(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                transform.position += new Vector3(transform.forward.x, 0f, transform.forward.z) * GameConstant.cameraMoveSpeed * Time.deltaTime;
                break;
            case Direction.Down:
                transform.position += new Vector3(-transform.forward.x, 0f, -transform.forward.z) * GameConstant.cameraMoveSpeed * Time.deltaTime;
                break;
            case Direction.Left:
                transform.Translate(Vector3.left * GameConstant.cameraMoveSpeed * Time.deltaTime);
                break;
            case Direction.Right:
                transform.Translate(Vector3.right * GameConstant.cameraMoveSpeed * Time.deltaTime);
                break;
        }
    }

    public void ScreenShake(ScreenShakeMagnitude magnitude)
    {
        switch (magnitude)
        {
            case ScreenShakeMagnitude.Small:
                break;
        }
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