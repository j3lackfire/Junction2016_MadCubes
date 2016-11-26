using UnityEngine;
using System.Collections;

public class BaseElementObject : MonoBehaviour {

    //Components
    private NavMeshAgent navMeshAgent;
    protected BaseObjectRenderer objectRenderer;
    [SerializeField]
    public ObjectData objectData = new ObjectData();
    [SerializeField]
    protected AnimatorWrapper animatorWrapper;
    private Animator childAnimator;
    [SerializeField]
    protected ObjectManager objectManager;

    public bool isEnemy;
    
    public ObjectState objectState;
    private bool isDamageDeal;
    private float attackCountDown;

    private Vector3 targetPosition;
    [SerializeField]
    BaseElementObject targetObject;

    public virtual void Init(ObjectManager _objectManager, bool _isEnemy)
    {
        isEnemy = _isEnemy;
        objectManager = _objectManager;
        objectData.objectHealth = objectData.objectMaxHealth;
        //components
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        navMeshAgent.speed = objectData.objectMoveSpeed;
        //components
        childAnimator = GetComponentInChildren<Animator>();
        animatorWrapper = new AnimatorWrapper(childAnimator);

        objectState = ObjectState.Idle;
        isDamageDeal = false;
    }

    int idleCountDown= 10;

    public virtual void DoUpdate()
    {
        switch(objectState)
        {
            case ObjectState.Idle:
                idleCountDown--;
                if (idleCountDown <= 0)
                {
                    idleCountDown = 10;
                    if (objectManager.RequestTarget(this) != null)
                    {
                        ChargeAtObject(objectManager.RequestTarget(this));
                    }
                }
                
                break;
            case ObjectState.Run:
                ObjectRunning();
                break;
            case ObjectState.Charge:
                ObjectCharging();                
                break;
            case ObjectState.Attack:
                ObjectAttack();
                break;
            case ObjectState.Stun:
                break;
        }

        animatorWrapper.DoUpdate();
    }

    private void ObjectRunning()
    {
        if ((transform.position - targetPosition).magnitude <= 0.5f)
        {
            //navMeshAgent.Stop();
            animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
            objectState = ObjectState.Idle;
        }
    }

    int countdown = 5;

    private void ObjectCharging()
    {
        if (IsTargetInRange())
        {
            StartAttackTarget();
        }
        else
        {
            countdown--;
            if (countdown <= 0)
            {
                countdown = 5;
                targetPosition = targetObject.transform.position;
                navMeshAgent.SetDestination(targetPosition);
            }
        }
    }

    protected virtual void ObjectAttack()
    {
        attackCountDown += Time.deltaTime;
        if (!isDamageDeal)
        {
            if (attackCountDown >= objectData.dealDamageTime)
            {
                DealDamageToTarget();
                isDamageDeal = true;
            }
        } else
        {
            if (attackCountDown >= objectData.attackDuration)
            {
                FinnishAttackTarget();
            }
        }
    }

    //manager call
    public void SetMovePosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        navMeshAgent.Resume();
        navMeshAgent.SetDestination(targetPosition);
        if (objectState != ObjectState.Run)
        {
            animatorWrapper.AddTriggerToQueue("EnterMovementAnimation");
        }
        objectState = ObjectState.Run;
    }

    //manager call
    public void ChargeAtObject(BaseElementObject target)
    {
        if (target == null)
        {
            //just let it be idle
        }
        else
        {
            targetObject = target;
            targetPosition = targetObject.transform.position;
            navMeshAgent.Resume();
            navMeshAgent.SetDestination(targetPosition);
            if (objectState != ObjectState.Run)
            {
                animatorWrapper.AddTriggerToQueue("EnterMovementAnimation");
            }

            objectState = ObjectState.Charge;
        }
    }

    private bool IsTargetInRange()
    {
        if ((transform.position - targetPosition).magnitude <= objectData.objectAttackRange)
        {
            return true;
        }
        return false;
    }

    protected virtual void StartAttackTarget()
    {
        objectState = ObjectState.Attack;
        transform.LookAt(targetObject.transform.position);
        animatorWrapper.AddTriggerToQueue("EnterAttackAnimation");
        navMeshAgent.Stop();
        isDamageDeal = false;
        attackCountDown = 0;

    }

    protected virtual void DealDamageToTarget()
    {
        Debug.Log(gameObject.name + " - deal damage to " + targetObject + " - damage " + objectData.objectDamange);
        if (targetObject != null)
        {
            targetObject.ReceiveDamage(objectData.objectDamange, GetObjectElement());

        }
    }

    protected void FinnishAttackTarget()
    {
        isDamageDeal = false;
        if (targetObject != null && IsTargetInRange())
        {
            StartAttackTarget();
        } else
        {
            if (objectManager.RequestTarget(this) == null)
            {
                animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
                objectState = ObjectState.Idle;
            } else
            {
                ChargeAtObject(objectManager.RequestTarget(this));
            }
        }
    }

    public void ReceiveDamage (int damage, GameElement attackedElement)
    {
        float damageMultiplier = 1f;
        switch (GetObjectElement())
        {
            case GameElement.Invalid:
            default:
                Debug.Log("<color=red>Invalid element, please check again.!!!! </color>");
                break;
            case GameElement.Fire:
                break;
            case GameElement.Water:
                break;
            case GameElement.Rock:
                break;
            case GameElement.Tree:
                break;
            case GameElement.Electricity:
                break;
            case GameElement.Cargo:
                break;
        }
        ReduceHealth((int)(damageMultiplier * damage));
    }

    public void ReduceHealth(int damage)
    {
        objectData.objectHealth -= damage;
        if (objectData.objectHealth <= 0)
        {
            //dead effect.
            objectManager.RemoveObject(this);
            Destroy(gameObject);
        }
    }

    public virtual GameElement GetObjectElement () { return GameElement.Invalid; }
}

public enum ObjectState
{
    Idle,
    Run,
    Charge,
    Attack,
    Stun
}

public enum GameElement
{
    Invalid,
    Fire,
    Water,
    Rock,
    Tree,
    Electricity,
    Cargo
}