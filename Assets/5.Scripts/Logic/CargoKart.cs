using UnityEngine;
using System.Collections.Generic;

public class CargoKart : BaseObject {
    //for use in the editor. Later will change to a randomly generated path.
    public List<GameObject> targetPosList = new List<GameObject>();
    
    //for easy view/debug in the editor. Remove later
    [SerializeField]
    private GameObject currentTargetNode;

    private int currentTargetNodeIndex;

    private CargoState cargoState;

    //for cargo preparation. Change to other type of circle when I have time to implement
    [SerializeField]
    private HighLightCircle activeCircle;

    private float startCircleSize = 6f;
    private float finishCircleSize = 2.5f;

    [SerializeField]
    private float cargoActiveRange = 3f;

    [SerializeField]
    private float cargoActiveTime = 3f;

    [SerializeField]
    private float currentActiveTime;

    [SerializeField]
    private bool isBeingActivated = false; //boolean to save performance.

    public override ObjectType GetObjectType()
    {
        return ObjectType.CargoKart;
    }

    //Maybe I should recode this thing to follow the base object standard. It will make it easier
    public override void Init(ObjectManager _objectManager, bool _isEnemy, int level)
    {
        objectManager = _objectManager;

        currentTargetNodeIndex = 0;
        currentTargetNode = targetPosList[currentTargetNodeIndex];
        PrepareComponent();
        UpdateStatsByLevel(1);

        activeCircle.Init();
        SetCargoState(CargoState.Prepare);
    }

    protected override void PrepareComponent()
    {
        cameraController = Directors.instance.cameraController;
        playerManager = Directors.instance.playerManager;
        enemyManager = Directors.instance.enemyManager;

        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<ObjectRenderer>();
            objectRenderer.InitRenderer(this);
        }
        objectRenderer.UpdateHealthBar(1f);
    }

    protected void SetCargoState(CargoState _cargoState)
    {
        cargoState = _cargoState;
        switch (_cargoState)
        {
            case CargoState.Invalid:
                objectState = ObjectState.Idle;
                break;
            case CargoState.Idle:
                objectState = ObjectState.Idle;
                break;
            case CargoState.Prepare:
                //the hero needs to stay inside the cargo's range for 3 seconds for it to move.
                currentActiveTime = cargoActiveTime;
                activeCircle.SetTargetGameObject(this.gameObject);
                SetActivationCircleSize(startCircleSize);
                objectState = ObjectState.Idle;
                break;
            case CargoState.Run:
                objectState = ObjectState.Run;
                break;
            case CargoState.Finnished:
                objectState = ObjectState.Idle;
                Directors.instance.EndBattle();
                break;
            case CargoState.Die:
                objectState = ObjectState.Die;
                break;
            default:
                Debug.Log("<color=red> DEFAULT STATE IS NOT SET !!! PLEASE CHECK !!!!</color>");
                break;
        }
    }

    public override void DoUpdate () {
        switch(cargoState)
        {
            case CargoState.Idle:
                WhileCargoIdle();
                break;
            case CargoState.Prepare:
                WhileCargoPrepare();
                break;
            case CargoState.Run:
                WhileCargoRun();
                break;
            case CargoState.Finnished:
                WhileCargoFinish();
                break;
            case CargoState.Die:
                //What to do ?
                break;
            case CargoState.Invalid:
            default:
                Debug.Log("<color=red> DEFAULT STATE IS NOT SET !!! PLEASE CHECK !!!!</color> " + cargoState.ToString());
                break;
        }
        //update animator wrapper to make animation run correctly
        RegenHealth();
        //animatorWrapper.DoUpdate();
        //objectRenderer.DoUpdateRenderer();
    }

    private void WhileCargoIdle() {}

    private void WhileCargoPrepare()
    {
        //The cargo should have a sort of area Range. 
        //If the hero stay in the range for 3 seconds
        //The cargo will start moving.
        if (!isBeingActivated)
        {
            //Should only check around every 10 or 20 frames or so to save performance.
            if (IsHeroInActivationRange())
            {
                isBeingActivated = true;
            }
        } else
        {
            //Reset the timer if the hero move out of the active range.
            if (IsHeroInActivationRange())
            {
                currentActiveTime -= Time.deltaTime;
                float activeCircleSize = finishCircleSize + (currentActiveTime / cargoActiveTime) * (startCircleSize - 2.5f);
                SetActivationCircleSize(activeCircleSize);
                if (currentActiveTime <= 0f)
                {
                    SetCargoState(CargoState.Run);
                    activeCircle.DeactiveCircle();
                    Directors.instance.StartBattle();
                }
            } else
            {
                currentActiveTime = cargoActiveTime;
                isBeingActivated = false;
                SetActivationCircleSize(startCircleSize);
            }
        }
        activeCircle.DoUpdate();
    }

    //for the activation circle.
    private void SetActivationCircleSize(float _size)
    {
        activeCircle.transform.localScale = Vector3.one * _size;
    }

    //move to target position
    private void WhileCargoRun()
    {
        if (IsTargetNodeReached())
        {
            if (IsFinalNodeReached())
            {
                //stop moving do nothing
                SetCargoState(CargoState.Finnished);
            }
            else
            {
                SetNextTargetNode();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetNode.transform.position, Time.deltaTime * objectData.moveSpeed);
        }

    }

    private void WhileCargoFinish()
    {

    }

    private void WhileCargoDie() { }

    //It do some complex calculating, maybe only called every few frame.
    private bool IsHeroInActivationRange()
    {
        if ((playerManager.GetNearestHero(this).transform.position - transform.position).magnitude < cargoActiveRange)
        {
            return true;
        }
        return false;
    }

    private bool IsFinalNodeReached()
    {
        if (currentTargetNodeIndex >= targetPosList.Count - 1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private bool IsTargetNodeReached()
    {
        if ((transform.position - currentTargetNode.transform.position).magnitude <= 0.5f)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void SetNextTargetNode()
    {
        currentTargetNodeIndex++;
        currentTargetNode = targetPosList[currentTargetNodeIndex];
        transform.LookAt(currentTargetNode.transform.position);
    }

    public override void OnObjectDie()
    {
        SetCargoState(CargoState.Die);
        objectRenderer.gameObject.SetActive(false);
        cameraController.ScreenShake(ScreenShakeMagnitude.Big);
        DeadEffect();
        //TODO: rework this
        Directors.instance.EndBattle();
        Debug.Log("<color=red>Battle state </color>" + Directors.instance.GetBattleState());
    }

    public override CorpseType GetCorpseType()
    {
        return CorpseType.Water_Creep_Corpse;
    }

    protected override float GetHealthRegenRate()
    {
        return 0.75f;
    }

    public override bool AutoHideHealthBar() { return false; }
}

public enum CargoState
{
    Invalid = -1,
    Prepare = 0, //Stay at the base, doing nothing
    Run = 1, //
    Idle, //Not yet decide what this will do :/
    Finnished, //Reach the final target.
    Die
}