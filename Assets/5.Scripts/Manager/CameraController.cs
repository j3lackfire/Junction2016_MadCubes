using UnityEngine;
using System.Collections;

public class CameraController : BaseManager {

    private Animator camAnimator;

    public override void Init()
    {
        camAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    public override void DoUpdate()
    {
    }

    public void ScreenShake(ScreenShakeMagnitude magnitude)
    {
        switch (magnitude)
        {
            case ScreenShakeMagnitude.Small:
                camAnimator.SetTrigger("SmallShake");
                break;
            case ScreenShakeMagnitude.Big:
                camAnimator.SetTrigger("BigShake");
                break;
        }
    }

    public void FollowCargo(Vector3 delta)
    {
        transform.position += delta;
    }
}

public enum ScreenShakeMagnitude
{
    Small,
    Medium,
    Big,
    Gigantic
}