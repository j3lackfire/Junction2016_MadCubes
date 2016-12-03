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
    public bool isDead;

    [SerializeField]
    private float cargoSpeed;

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
        isDead = false;
        UpdateStatsByLevel(1);
    }	

    public override void DoUpdate () {
        if (!isGameWin && !isDead)
        {
            MoveToTargetPosition();
        }
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
            transform.position = Vector3.MoveTowards(transform.position, currentTargetNode.transform.position, Time.deltaTime * cargoSpeed);
            Directors.cameraController.FollowCargo(transform.position - oldPos);
        }
    }

    protected override void ReduceHealth(int damage)
    {
        //Directors.cameraController.ScreenShake(ScreenShakeMagnitude.Small);
        objectData.health -= damage;
        if (objectData.health <= 0)
        {
            //Destroy(gameObject);
            Directors.uiMaster.GameOver();
            isDead = true;
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
        Directors.enemyManager.spawnPointParent.transform.position = currentTargetNode.transform.position;
        transform.LookAt(currentTargetNode.transform.position);
    }
}
