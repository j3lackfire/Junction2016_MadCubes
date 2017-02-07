using UnityEngine;
using System.Collections;

public class CameraController : BaseManager {

    private Animator camAnimator;

    private GameObject cargo;
    private Vector3 oldCargoPosition;

    private PlayerManager playerManager;

    public override void Init()
    {
        base.Init();
        playerManager = director.playerManager;
        camAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        switch (director.GetBattleState())
        {
            case BattleState.Prepare:
                break;
            case BattleState.Battling:
                AutomaticFollowCargo();
                break;
            case BattleState.Finish:
                break;
            default:
                Debug.Log("<color=red>CAMERA MANANGER - battle state not defined !!!!</color>" + director.GetBattleState());
                break;
        }
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

    private void AutomaticFollowCargo()
    {
        if (cargo == null)
        {
            cargo = playerManager.GetCargoKart().gameObject;
            oldCargoPosition = cargo.transform.position;
        }
        Vector3 deltaPos = cargo.transform.position - oldCargoPosition;
        transform.position += deltaPos;
        oldCargoPosition = cargo.transform.position;
    }
}

public enum ScreenShakeMagnitude
{
    Small,
    Medium,
    Big,
    Gigantic
}