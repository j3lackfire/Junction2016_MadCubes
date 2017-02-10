using UnityEngine;
using System.Collections.Generic;

public class CameraController : BaseManager {
    [Header("Camera configuration")]
    [SerializeField]
    private float cameraSpeed = 5f;

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

    //[SerializeField] //test variable just to see.
    //float d;

    //Object components
    private Animator camAnimator;

    //cached object
    private GameObject cargo;
    private PlayerManager playerManager;
    private Vector3 targetPosition; //the position the camera want to go to
    [SerializeField]
    private float screenAspectRatio;

    public override void Init()
    {
        base.Init();
        playerManager = director.playerManager;
        camAnimator = gameObject.GetComponentInChildren<Animator>();
        screenAspectRatio = Screen.width / (float)Screen.height;
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        MoveCameraToTargetPosition();
        AutoAdjustCameraSize();
        switch (director.GetBattleState())
        {
            case BattleState.Prepare:
                WhileBattlePrepare();
                break;
            case BattleState.Battling:
                AutomaticFollowCargo();
                break;
            case BattleState.Finish:
                WhileBattlePrepare();
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
        targetPosition = _position;
    }

    private void MoveCameraToTargetPosition()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), cameraSpeed * Time.deltaTime);
    }

    //need to consider in the screen ratio which is not SQUARE screen
    private float GetFurthestScreenDistanceToCamera()
    {
        List<BaseHero> heroList = playerManager.heroList;
        float furthestDistance = -1f;
        Vector3 cameraPos = new Vector3(transform.position.x, 0, transform.position.z);
        for (int i = 0; i < heroList.Count; i++)
        {
            Vector3 heroPos = heroList[i].transform.position;
            float distanceX = heroPos.x - cameraPos.x;
            distanceX = distanceX < 0 ? -distanceX : distanceX; //make sure the value is not negative

            float distanceZ = heroPos.z - cameraPos.z;
            distanceZ = distanceZ < 0 ? -distanceZ : distanceZ; //make sure the value is not negativedistanceZ *= screenAspectRatio;
            distanceZ *= screenAspectRatio;
            float distance = distanceX > distanceZ ? distanceX : distanceZ;
            //float distance = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
            }
        }

        return furthestDistance;
    }

    private void AutoAdjustCameraSize()
    {
        float d = GetFurthestScreenDistanceToCamera();
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
            if (heroList[i].CanTargetObject())
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
        //SetCameraPosition(cargo.transform.position + cargoOffsetPosition);
        SetCameraPosition(GetHeroesAndCargoMiddlePosition());
    }

    private Vector3 GetHeroesAndCargoMiddlePosition()
    {
        List<BaseHero> heroList = playerManager.heroList;
        //calculate the middle point of all the heroes
        Vector3 cameraPosition = Vector3.zero;
        int aliveHeroCount = 0;
        for (int i = 0; i < heroList.Count; i++)
        {
            if (heroList[i].CanTargetObject())
            {
                cameraPosition += heroList[i].transform.position;
                aliveHeroCount++;
            }
        }
        cameraPosition += cargo.transform.position + cargoOffsetPosition;
        aliveHeroCount++;
        cameraPosition /= aliveHeroCount;
        return cameraPosition;
    }
}

public enum ScreenShakeMagnitude
{
    Small,
    Medium,
    Big,
    Gigantic
}