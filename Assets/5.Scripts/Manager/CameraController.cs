using UnityEngine;
using System.Collections.Generic;

public class CameraController : BaseManager {
    [Header("Camera configuration")]
    [SerializeField]
    private Vector3 cargoOffsetPosition; //offset position compare to the cargo

    //Dynamic camera. If hero is close, the camera will be close to the ground (zoomed in)
    //If the hero is far, the camera will zoom out

    [SerializeField]
    private float minCameraYPosition;
    [SerializeField]
    private float minHeroDistance;

    [SerializeField]
    private float maxCameraYPosition;
    [SerializeField]
    private float maxHeroDistance;

    //Object components
    private Animator camAnimator;

    [SerializeField]
    float d;

    //cached object
    private GameObject cargo;
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
                d = GetFurthestHeroDistanceToCamera();
                AutoAdjustCameraSize();
                WhileBattlePrepare();
                break;
            case BattleState.Battling:
                AutoAdjustCameraSize();
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

    /* Change the main position of the camera. Right now, it just jump the camera.
    but in the future, it will use some smooth movement so it will look
    much better */
    public void SetCameraPosition(Vector3 _position)
    {
        transform.position = new Vector3(_position.x, transform.position.y, _position.z);
    }

    private float GetFurthestHeroDistanceToCamera()
    {
        //TODO: Can be optimized by cached all of the hero at the start of the prepare phase
        List<BaseHero> heroList = playerManager.heroList;
        float furthestDistance = -1f;
        Vector3 cameraPos = new Vector3(transform.position.x, 0, transform.position.z);
        for (int i = 0; i < heroList.Count; i ++)
        {
            float distance = (heroList[i].transform.position - cameraPos).magnitude;
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
            }
        }
        return furthestDistance;
    }

    private void AutoAdjustCameraSize()
    {
        float d = GetFurthestHeroDistanceToCamera();
        if (d <= minHeroDistance)
        {
            SetCameraSize(0);
        } else
        {
            if (d >= maxHeroDistance)
            {
                SetCameraSize(1);
            } else
            {
                float camSize = (d - minHeroDistance) / (maxHeroDistance - minHeroDistance);
                SetCameraSize(camSize);
            }
        }
    }

    //0 < size < 1. 0 = no zoom. 1 = full zoom
    private void SetCameraSize(float _size)
    {
        if (_size <= 0)
        {
            transform.position = new Vector3(transform.position.x, minCameraYPosition, transform.position.z);
        } else
        {
            if (_size >= 1)
            {
                transform.position = new Vector3(transform.position.x, maxCameraYPosition, transform.position.z);
            } else
            {
                float yPos = minCameraYPosition + (maxCameraYPosition - minCameraYPosition) * _size;
                transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            }
        }
    }

    //Prepare camera
    private void WhileBattlePrepare()
    {
        SetCameraPosition(GetAllHeroesMiddlePosition());
    }

    private Vector3 GetAllHeroesMiddlePosition()
    {
        //TODO: Can be optimized by cached all of the hero at the start of the prepare phase
        List<BaseHero> heroList = playerManager.heroList;
        //calculate the middle point of all the heroes
        Vector3 cameraPosition = Vector3.zero;
        int aliveHeroCount = 0;
        for (int i = 0; i < heroList.Count; i++)
        {
            if (heroList[i].GetObjectState() != ObjectState.Die)
            {
                cameraPosition += heroList[i].transform.position;
                aliveHeroCount++;
            }
        }
        cameraPosition /= aliveHeroCount;
        return cameraPosition;
    }

    //Battle Camera
    private void AutomaticFollowCargo()
    {
        if (cargo == null)
        {
            cargo = playerManager.GetCargoKart().gameObject;
        }
        Vector3 newPos = cargo.transform.position + cargoOffsetPosition;
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
    }
}

public enum ScreenShakeMagnitude
{
    Small,
    Medium,
    Big,
    Gigantic
}