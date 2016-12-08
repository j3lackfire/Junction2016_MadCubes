using UnityEngine;
using System.Collections;

public class BaseProjectile : PooledObject {
    //Manager
    protected ProjectileManager projectileManager;
    //Move speed of the projectile
    [SerializeField]
    protected float projectileSpeed;
    //is this really neccesary ?
    //[SerializeField]
    protected bool isEnemyTeam;

    //[SerializeField]
    protected ProjectileType projectileType;

    //[SerializeField]
    protected int damage;
    //[SerializeField]
    protected Vector3 startPosition;
    //[SerializeField]
    protected BaseObject attackerObject;
    //[SerializeField]
    protected Vector3 targetPosition;
    //[SerializeField]
    protected BaseObject targetObject;
    protected long targetID;

    //is this bullet chase the target
    protected bool isChaseBullet;

    protected override void OnFirstInit()
    {
        base.OnFirstInit();
        projectileManager = Directors.projectileManager;
    }

    public virtual void Init(ProjectileType _type, bool _isEnemyProjectile, int _damage)
    {
        OnFirstInit();
        projectileType = _type;
        isEnemyTeam = _isEnemyProjectile;
        damage = _damage;
        isChaseBullet = true;
    }

    public virtual void InitPosition(Vector3 _startPos, Vector3 _targetPos)
    {
        startPosition = _startPos;
        targetPosition = _targetPos;
        transform.position = startPosition;
        transform.LookAt(targetPosition);
    } 

    public virtual void InitObjects(BaseObject _attacker, BaseObject _target)
    {
        attackerObject = _attacker;
        targetObject = _target;
        targetID = _target.id;
    }

    public virtual void SetChaseBullet(bool _boo) { isChaseBullet = _boo; }

    public virtual void DoUpdate()
    {
        MoveBullet();
        if (isChaseBullet)
        {
            ChaseBulletFixedTargetPosition();
        }
        if (IsBulletReachFinalPosition())
        {
            DealDamageToTarget();
            BulletEffect();
            KillBullet();
        }
    }

    protected void MoveBullet()
    {
        //move the bullet to the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * projectileSpeed);
    }

    //chase bullet correct position count down
    int correctPositionCountDown = 10;
    //only use for chase bullet because it need to follow the target
    protected void ChaseBulletFixedTargetPosition()
    {
        correctPositionCountDown--;
        if (correctPositionCountDown <= 0)
        {
            if (IsTargetChange())
            {
                //so it wouldn't check too much
                correctPositionCountDown = 9999;
                targetObject = null;
            } else
            {
                targetPosition = targetObject.transform.position;
                correctPositionCountDown = 10;
            }
        }
        
    }

    protected bool IsTargetChange() //because of pooling
    {
        if (targetObject == null || targetObject.objectState == ObjectState.Die || targetObject.id != targetID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    protected virtual bool IsBulletReachFinalPosition()
    {
        if ((transform.position - targetPosition).magnitude >= 0.5f)
        {
            return false;
        } else
        {
            return true;
        }
    }

    protected virtual void DealDamageToTarget()
    {
        if (IsTargetChange()) { }
        else
        {
            targetObject.ReceiveDamage(damage, attackerObject);
        }
    }

    protected virtual void BulletEffect() { }

    protected virtual void KillBullet()
    {
        projectileManager.RemoveProjectile(this);
        ReturnToPool();
    }
}

public enum ProjectileType
{
    Invalid, //err ?
    Water_Hero_Laser,//for Water hero
    Fire_Hero_Laser,
    Fire_Creep_Projectile,
    Water_Creep_Projectile
}
