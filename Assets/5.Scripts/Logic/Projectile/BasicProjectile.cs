using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour {
    //Manager
    protected ProjectileManager projectileManager;
    //Move speed of the projectile
    [SerializeField]
    protected float projectileSpeed;
    //is this really neccesary ?
    [SerializeField]
    protected bool isEnemyTeam;

    [SerializeField]
    protected ProjectileType projectileType;

    [SerializeField]
    protected int damage;
    [SerializeField]
    protected Vector3 startPosition;
    [SerializeField]
    protected BaseObject attackerObject;
    [SerializeField]
    protected Vector3 targetPosition;
    [SerializeField]
    protected BaseObject targetObject;

    //is this bullet chase the target
    protected bool isChaseBullet;

    public virtual void Init(ProjectileType _type, bool _isEnemyProjectile, int _damage)
    {
        projectileManager = Directors.projectileManager;
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
    }

    public virtual void SetChaseBullet(bool _boo) { isChaseBullet = _boo; }

    public virtual void DoUpdate()
    {
        CheckReachTarget();
    }

    //chase bullet correct position count down
    int correctPositionCountDown = 10;

    protected virtual void CheckReachTarget()
    {
        if ((transform.position - targetPosition).magnitude >= 0.5f)
        {
            if (targetObject != null && isChaseBullet)
            {
                correctPositionCountDown--;
                if (correctPositionCountDown <= 0)
                {
                    targetPosition = targetObject.transform.position;
                    correctPositionCountDown = 10;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * projectileSpeed);
        } else
        {
            if (targetObject != null)
            {
                targetObject.ReceiveDamage(damage, attackerObject);
            }
            projectileManager.RemoveProjectile(this);
            //Pools, pools, pools
            Destroy(gameObject);
        }
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
