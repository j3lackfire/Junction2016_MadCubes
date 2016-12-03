using UnityEngine;
using System.Collections;

//Default, base object = enemy
public class BaseUnit : BaseObject {
    //is enemy = true ??????
    protected override void ObjectCharging()
    {
        if (targetObject == null)
        {
            SetState(ObjectState.Idle);
            return;
        }

        if (IsTargetInRange())
        {
            StartAttackTarget();
            return;
        }

        //This make basic unit object does not lock to one target.
        objectChargeCountdown--;
        if (objectChargeCountdown <= 0)
        {
            if (isEnemy && targetObject == PlayerManager.cargoKart &&
                objectManager.RequestTarget(this) != PlayerManager.cargoKart)
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
}
