using UnityEngine;
using System.Collections;

//Default, base object = enemy
public class BaseUnit : BaseObject {
    //COMPONENTS
    protected UnityEngine.AI.NavMeshAgent navMeshAgent;
    
    //private variables
    protected Vector3 targetPosition;
    protected bool isHavingTargetPosition;

    protected bool isFollowingAlly;
    protected BaseObject followingAlly;
    //The position of the target object is always changing. It would hurt performance 
    //if we update the target position every frame.
    protected int objectChargeCountdown;


    public override void Init(ObjectManager _objectManager, bool isEnemyTeam, int objectLevel)
    {
        base.Init(_objectManager, isEnemyTeam, objectLevel);

        isHavingTargetPosition = false;
        isFollowingAlly = false;
        objectChargeCountdown = GameConstant.objectChargeCountdownValue;
    }

    protected override void PrepareComponent()
    {
        base.PrepareComponent();
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        navMeshAgent.speed = objectData.moveSpeed;

    }

    protected override void SetState(ObjectState state)
    {
        switch (state)
        {
            case ObjectState.Idle:
                //Hot fix, so that when the hero respawn, he will not move to his previous position.
                //Is this one a hot fix or a neccessary logic step ????
                navMeshAgent.Stop();
                navMeshAgent.SetDestination(transform.position);

                //Another hot fix, to fix the wrong rotation of object when finishing an animation
                //There is still a weird rotation sometime but I can not catch it all the time :/
                //Debug.Log("Local Rotation " + objectRenderer.gameObject.transform.localRotation.ToString());
                objectRenderer.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //Debug.Log("Local Rotation after set - " + objectRenderer.gameObject.transform.localRotation.ToString());

                animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
                break;
            case ObjectState.Run:
            case ObjectState.Charge:
                //need this one so the object will not just repeat the run animation while running
                if (objectState != ObjectState.Run && objectState != ObjectState.Charge)
                {
                    animatorWrapper.AddTriggerToQueue("EnterMovementAnimation");
                }
                break;
            case ObjectState.Attack:
                navMeshAgent.Stop();
                animatorWrapper.AddTriggerToQueue("EnterAttackAnimation");
                break;
            case ObjectState.Stun:
                break;
            case ObjectState.Special:
                navMeshAgent.Stop();
                animatorWrapper.AddTriggerToQueue("EnterAttackAnimation");
                break;
            case ObjectState.Die:
                //Maybe special case here ? to add the dead effect here and other stuffs.

                //Object die while moving, when respawned will have a weird position offset.
                //This is to fix that bug
                objectRenderer.OnParentObjectDie();
                break;
        }
        base.SetState(state);
    }

    protected override void ObjectIdle()
    {
        if (isHavingTargetPosition)
        {
            MoveToTargetPosition();
            return;
        }
        idleCountDown -= Time.deltaTime;
        if (idleCountDown <= 0f)
        {
            idleCountDown = GameConstant.idleCheckFrequency;

            BaseObject requestedTarget = objectManager.RequestTarget(this);
            if (requestedTarget != null)
            {
                ChargeAtObject(requestedTarget);
            }
            else
            {
                if (isFollowingAlly)
                {
                    if (followingAlly != null && followingAlly.CanTargetObject())
                    {
                        if ((followingAlly.transform.position - transform.position).magnitude >= GameConstant.runningReachingDistance * 6)
                        {
                            SetTargetMovePosition(followingAlly.transform.position);
                        }
                    }
                    else
                    {
                        isFollowingAlly = false;
                    }
                }
            }
        }
    }

    //[HideInInspector] //this to make sure the object follow other smoothly
    //private int followAllyCountDown = 20; 
    protected override void ObjectRunning()
    {
        base.ObjectRunning();
        //TODO: I'm not very sure about this. This could hurt performance
        //It's does not follow very smoothly with this function, but I guess this is still OK
        //if (isFollowingAlly)
        //{
        //    followAllyCountDown--;
        //    if (followAllyCountDown <= 0)
        //    {
        //        followAllyCountDown = 20;
        //        if ((followingAlly.transform.position - targetPosition).magnitude >= GameConstant.runningReachingDistance * 2) {
        //            SetTargetMovePosition(followingAlly.transform.position, true);
        //        }
        //    }
        //}
        //Old function. Working pretty well :/
        if ((transform.position - targetPosition).magnitude <= GameConstant.runningReachingDistance)
        {
            SetState(ObjectState.Idle);
        }

    }

    //is enemy = true ??????
    protected override void ObjectCharging()
    {
        if (targetObject == null || IsTargetChanged())
        {
            SetState(ObjectState.Idle);
            return;
        }
        if (IsTargetInRange())
        {
            StartAttackTarget();
            return;
        }

        objectChargeCountdown--;
        if (objectChargeCountdown <= 0)
        {
            objectChargeCountdown = GameConstant.objectChargeCountdownValue;
            //set the target position again.
            targetPosition = targetObject.transform.position;
            navMeshAgent.SetDestination(targetPosition);
        }

        //This make basic unit object does not lock to one target.
        objectChargeCountdown--;
        if (objectChargeCountdown <= 0)
        {
            if (isEnemy && targetObject.GetObjectType() == ObjectType.CargoKart &&
                objectManager.RequestTarget(this).GetObjectType() != ObjectType.CargoKart)
            {
                ChargeAtObject(objectManager.RequestTarget(this));
            }
            else
            {
                objectChargeCountdown = GameConstant.objectChargeCountdownValue;
                targetPosition = targetObject.transform.position;
                navMeshAgent.SetDestination(targetPosition);
            }
        }
        //end of region
    }

    public override void SetObjectTargetPosition(Vector3 _targetPosition, bool isLocalCall)
    {
        base.SetObjectTargetPosition(_targetPosition, isLocalCall);
        if (!isLocalCall)
        {
            isFollowingAlly = false;
        }
        SetTargetMovePosition(_targetPosition);
    }

    //when hero is busy (like being stun, or attacking) it can not move to the target position instantly
    //instead, it just add the action to the queue
    protected void SetTargetMovePosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        if (CanExecuteOrder())
        {
            MoveToTargetPosition();
        }
        else
        {
            isHavingTargetPosition = true;
        }
    }

    protected void MoveToTargetPosition()
    {
        navMeshAgent.Resume();
        navMeshAgent.SetDestination(targetPosition);
        isHavingTargetPosition = false;
        SetState(ObjectState.Run);
    }

    public override void SetObjectTarget(BaseObject target, bool isLocalCall)
    {
        base.SetObjectTarget(target, isLocalCall);
        if (!isLocalCall)
        {
            isFollowingAlly = false;
        }
        ChargeAtObject(target);
    }

    protected void ChargeAtObject(BaseObject target)
    {
        if (target == null)
        {
            //just let it be idle
            SetState(ObjectState.Idle);
        }
        else
        {
            SetTargetObject(target);
            targetPosition = targetObject.transform.position;
            navMeshAgent.Resume();
            navMeshAgent.SetDestination(targetPosition);
            SetState(ObjectState.Charge);
        }
    }

    //Make object run after an ally.
    public void SetFollowAlly(BaseObject allyObject)
    {
        followingAlly = allyObject;
        SetTargetMovePosition(followingAlly.transform.position);
        isFollowingAlly = true;
    }

    protected override void FinnishAttackTarget()
    {
        isDamageDeal = false;
        //action cancle. If hero is attacking and the player want to cancel it.
        if (isHavingTargetPosition)
        {
            MoveToTargetPosition();
            return;
        }

        //The last check -> if enemy is attacking the cargo, while the hero is close, he should focus on the
        //hero instead. -> Might change later
        if (targetObject != null && CanTargetObject() && !IsTargetChanged() && IsTargetInRange())
        {
            StartAttackTarget();
        }
        else
        {
            BaseObject requestedTarget = objectManager.RequestTarget(this);
            if (requestedTarget == null)
            {
                SetState(ObjectState.Idle);
            }
            else
            {
                ChargeAtObject(requestedTarget);
            }
        }
    }

    public override bool ActiveSpecial()
    {
        isFollowingAlly = false;
        return base.ActiveSpecial();
    }


}
