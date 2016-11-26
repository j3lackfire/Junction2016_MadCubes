using UnityEngine;
using System.Collections.Generic;

public class CargoKart : MonoBehaviour {

    public List<GameObject> targetPosList = new List<GameObject>();
    [SerializeField]
    GameObject currentTargetNode;

    private int currentTargetNodeIndex;

    public bool isGameWin;

    [SerializeField]
    private float cargoSpeed;

    public void Init()
    {
        currentTargetNodeIndex = 0;
        currentTargetNode = targetPosList[currentTargetNodeIndex];
        isGameWin = false;
    }	

    public void DoUpdate () {
        if (!isGameWin)
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
            transform.position = Vector3.MoveTowards(transform.position, currentTargetNode.transform.position, Time.deltaTime * cargoSpeed);
            //cameraController.FollowCargoKart();
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
}
