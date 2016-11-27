using UnityEngine;
using System.Collections;

public class BaseElementObject : MonoBehaviour {

    //Components
    protected NavMeshAgent navMeshAgent;
    protected BaseObjectRenderer objectRenderer;
    [SerializeField]
    public ObjectData objectData = new ObjectData();
    [SerializeField]
    protected AnimatorWrapper animatorWrapper;
    protected Animator childAnimator;
    [SerializeField]
    protected ObjectManager objectManager;
    protected ProjectileManager projectileManager;

    public bool isEnemy;
    
    public ObjectState objectState;
    protected bool isDamageDeal;
    protected float attackCountDown;

    private Vector3 targetPosition;
    [SerializeField]
    protected BaseElementObject targetObject;

    public virtual void Init(ObjectManager _objectManager, bool _isEnemy, int objectLevel)
    {
        isEnemy = _isEnemy;
        objectManager = _objectManager;
        projectileManager = Directors.projectileManager;
        //components
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        navMeshAgent.speed = objectData.objectMoveSpeed;
        //components
        childAnimator = GetComponentInChildren<Animator>();
        animatorWrapper = new AnimatorWrapper(childAnimator);

        UpdateStatsByLevel(objectLevel);
        objectState = ObjectState.Idle;
        isDamageDeal = false;
    }

    public virtual void UpdateStatsByLevel (int level)
    {
        objectData.objectLevel = level;
        objectData.objectMaxHealth = objectData.objectMaxHealth + (int)(objectData.objectMaxHealth * level * 0.15f);
        objectData.objectDamange = objectData.objectDamange + (int)(objectData.objectDamange * level * 0.15f);
        objectData.objectHealth = objectData.objectMaxHealth;
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
            case ObjectState.Die:
                //when hero die, should countdown or do something
                WhileHeroDie();
                break;
        }

        animatorWrapper.DoUpdate();
    }

    //call every frame when a hero is die
    public virtual void WhileHeroDie() { }

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
        if (targetObject == null)
        {
            animatorWrapper.AddTriggerToQueue("EnterIdleAnimation");
            objectState = ObjectState.Idle;
            return;
        }
        
        if (IsTargetInRange())
        {
            StartAttackTarget();
        }
        else
        {
            countdown--;
            if (countdown <= 0)
            {
                if (isEnemy && targetObject == PlayerManager.cargoKart &&
                    objectManager.RequestTarget(this) != PlayerManager.cargoKart)
                {
                    ChargeAtObject(objectManager.RequestTarget(this));
                } else
                {
                    countdown = 5;
                    targetPosition = targetObject.transform.position;
                    navMeshAgent.SetDestination(targetPosition);
                }
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
        if (targetObject != null) {
            transform.LookAt(targetObject.transform.position);
        }
        animatorWrapper.AddTriggerToQueue("EnterAttackAnimation");
        navMeshAgent.Stop();
        isDamageDeal = false;
        attackCountDown = 0;

    }

    protected virtual void DealDamageToTarget()
    {
        if (targetObject != null)
        {
            projectileManager.CreateProjectile(ProjectileType.Fire_Creep, isEnemy, objectData.objectDamange, transform.position, targetObject, GetObjectElement());
        }
    }

    protected void FinnishAttackTarget()
    {
        isDamageDeal = false;
        if (targetObject != null && IsTargetInRange() && !isEnemy)
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
        //switch (GetObjectElement())
        //{
        //    case GameElement.Invalid:
        //    default:
        //        Debug.Log("<color=red>Invalid element, please check again.!!!! </color>");
        //        break;
        //    case GameElement.Fire:
        //        break;
        //    case GameElement.Water:
        //        break;
        //    case GameElement.Rock:
        //        break;
        //    case GameElement.Tree:
        //        break;
        //    case GameElement.Electricity:
        //        break;
        //    case GameElement.Cargo:
        //        break;
        //}
        ReduceHealth((int)(damageMultiplier * damage));
    }

    public virtual void ReduceHealth(int damage)
    {
        objectData.objectHealth -= damage;
        if (objectData.objectHealth <= 0)
        {
            //dead effect.
            for (int i = 0; i < 15; i++)
            {
                GameObject corpse = GameObject.CreatePrimitive(PrimitiveType.Cube);
                corpse.transform.position = gameObject.transform.position + (new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)));
                corpse.transform.localScale = new Vector3(0.3333f, 0.3333f, 0.3333f);
                corpse.GetComponent<Renderer>().material = PrefabsManager.GetMaterialColor(GetObjectElement(), !isEnemy);
                corpse.AddComponent<Rigidbody>();
                Destroy(corpse, Random.Range(2.5f,3.5f));
            }

            //end dead effect
            if (objectManager == null)
            {
                if (isEnemy)
                {
                    objectManager = Directors.enemyManager;
                } else
                {
                    objectManager = Directors.playerManager;
                }
            }
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
    Stun,
    Die
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