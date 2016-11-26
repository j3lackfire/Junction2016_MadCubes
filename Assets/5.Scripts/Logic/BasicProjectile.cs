using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour {
    [SerializeField]
    public float projectileSpeed;

    public bool isEnemyTeam;
    public int damage;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public BaseElementObject targetObject;
    public GameElement bulletElement;

    public void Init(bool _isEnemy, int _damage, Vector3 _startPos, Vector3 _targetPos, BaseElementObject _targetObject, GameElement _bulletElement)
    {
        isEnemyTeam = _isEnemy;
        damage = _damage;
        startPosition = _startPos;
        targetPosition = _targetPos;
        targetObject = _targetObject;
        bulletElement = _bulletElement;
        transform.position = startPosition;
        transform.LookAt(targetPosition);
    }

    public void DoUpdate()
    {
        CheckReachTarget();
    }

    void CheckReachTarget()
    {
        if ((transform.position -targetPosition).magnitude >= 0.5f)
        {
            if (targetObject != null)
            {
                targetPosition = targetObject.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * projectileSpeed);
        } else
        {
            if (targetObject != null)
            {
                targetObject.ReceiveDamage(damage, bulletElement);
            }
            Directors.projectileManager.RemoveProjectile(this);
            Destroy(gameObject);
        }
    }
    
}

public enum ProjectileType
{
    Water_Hero,
    Fire_Hero
}
