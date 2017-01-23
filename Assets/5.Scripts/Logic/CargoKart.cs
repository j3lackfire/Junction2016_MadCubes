using UnityEngine;
using System.Collections.Generic;

public class CargoKart : BaseObject {
    //for use in the editor. Later will change to a randomly generated path.
    public List<GameObject> targetPosList = new List<GameObject>();
    //for easy view/debug in the editor. Remove later
    [SerializeField]
    public GameObject currentTargetNode;

    private int currentTargetNodeIndex;

    public bool isGameWin;
    
    public override ObjectType GetObjectType()
    {
        return ObjectType.CargoKart;
    }

    //Maybe I should recode this thing to follow the base object standard. It will make it easier
    public override void Init(ObjectManager _objectManager, bool _isEnemy, int level)
    {
        currentTargetNodeIndex = 0;
        currentTargetNode = targetPosList[currentTargetNodeIndex];
        isGameWin = false;
        PrepareComponent();
        UpdateStatsByLevel(1);
        objectState = ObjectState.Run;
    }

    protected override void PrepareComponent()
    {
        cameraController = Directors.instance.cameraController;
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<ObjectRenderer>();
            objectRenderer.InitRenderer(this);
        }
        objectRenderer.UpdateHealthBar(1f);
    }

    public override void DoUpdate () {
        if (!isGameWin && objectState != ObjectState.Die)
        {
            MoveToTargetPosition();
        }
        RegenHealth();
    }

    private void MoveToTargetPosition()
    {
        if (IsTargetNodeReached())
        {
            if (IsFinalNodeReached())
            {
                //stop moving do nothing
                isGameWin = true;
            }
            else
            {
                SetNextTargetNode();
            }
        }
        else
        {
            Vector3 oldPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, currentTargetNode.transform.position, Time.deltaTime * objectData.moveSpeed);
            //remove later.
            cameraController.FollowCargo(transform.position - oldPos);
        }
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
        SetState(ObjectState.Die);
        objectRenderer.gameObject.SetActive(false);
        cameraController.ScreenShake(ScreenShakeMagnitude.Big);
        DeadEffect();
        Directors.instance.EndBattle();
        Debug.Log("<color=red>Battle state  </color>" + Directors.instance.GetBattleState());
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
