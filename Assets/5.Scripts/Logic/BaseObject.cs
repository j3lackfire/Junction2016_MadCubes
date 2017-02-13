using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseObject : PooledObject
{
    //COMPONENTS
    
    protected ObjectRenderer objectRenderer;
    [SerializeField] //maybe read data from JSON later for easier configuration ?
    public ObjectData objectData = new ObjectData();
    //[SerializeField] //serialize field for easier debuggin, don't need now
    protected AnimatorWrapper animatorWrapper;
    protected Animator childAnimator;
    //MANAGERS
    protected ObjectManager objectManager; //Object's manager, for team specific action
    protected PlayerManager playerManager; //For player's team function, like get nearest hero
    protected EnemyManager enemyManager; //For enemy's team function, 
    protected ProjectileManager projectileManager;
    protected CameraController cameraController; //need for screen shake
    protected MouseController mouseController; //need for hero only, might change later though

    //public variables (for other unit to check)
    public bool isEnemy;
    protected ObjectState objectState;
    
    //Private variables, mostly for flagging and checking 
    protected float idleCountDown; //The amount of frame to check for new target when idle
    protected bool isDamageDeal;
    protected float attackCountUp;

    //[SerializeField]
    protected long targetID;
    protected BaseObject targetObject;

    public virtual ObjectType GetObjectType() { return ObjectType.Invalid; }

    public virtual ProjectileType GetProjectileType() { return ProjectileType.Invalid; }

    public virtual CorpseType GetCorpseType() { return CorpseType.Invalid; }

    public ObjectState GetObjectState() { return objectState;}

    public virtual void Init(ObjectManager _objectManager, bool isEnemyTeam, int objectLevel)
    {
        //for testing and easier debuging.
        gameObject.name = GetObjectType().ToString() + "_" + id.ToString();
        isEnemy = isEnemyTeam;
        objectManager = _objectManager;
        OnFirstInit();
        PrepareComponent();
        
        UpdateStatsByLevel(objectLevel);
        SetState(ObjectState.Idle);
        isDamageDeal = false;
        idleCountDown = GameConstant.idleCheckFrequency;
        healthRegenCountUp = 0f;
    }

    protected override void OnFirstInit()
    {
        base.OnFirstInit();
        projectileManager = Directors.instance.projectileManager;
        cameraController = Directors.instance.cameraController;
        mouseController = Directors.instance.mouseController;
        playerManager = Directors.instance.playerManager;
        enemyManager = Directors.instance.enemyManager;
    }

    protected virtual void PrepareComponent()
    {
        childAnimator = GetComponentInChildren<Animator>();
        animatorWrapper = new AnimatorWrapper(childAnimator);

        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<ObjectRenderer>();
            objectRenderer.InitRenderer(this);
        }
        objectRenderer.UpdateHealthBar(1f);
        objectRenderer.OnParentObjectRespawn();
    }

    //TODO: Seriously need to consider this thing again
    public virtual void UpdateStatsByLevel(int level = 1)
    {
        objectData.level = level;
        objectData.sight = objectData.attackRange + 3f;
        objectData.maxHealth = objectData.baseMaxHealth+ (int)(objectData.baseMaxHealth * (level -1) * GameConstant.normalCreepDamageIncreasePerLevel);
        objectData.damange = objectData.baseDamage+ (int)(objectData.baseDamage * (level - 1) * GameConstant.normalCreepDamageIncreasePerLevel);
        objectData.health = objectData.maxHealth;
        objectData.currentSpecialCoolDown = objectData.specialCoolDown;
    }

    //Update is called every frame by this object manager.
    //DON'T OVERRIDE THIS UNLESS CARGO KART!!!.
    public virtual void DoUpdate()
    {
        switch (objectState)
        {
            case ObjectState.Idle:
                ObjectIdle();
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
                ObjectStun();
                break;
            case ObjectState.Special:
                ObjectSpecial();
                break;
            case ObjectState.Die:
                WhileObjectDie();
                break;
        }
        //Main core logic is different between Building and units.
        AdditionalUpdateFunction();
    }

    //What should this object do when a state is changed ????
    protected virtual void SetState(ObjectState state)
    {
        objectState = state;
    }

    protected virtual void AdditionalUpdateFunction()
    {
        //Make the special skill countdown correctly
        UpdateSpecialCountDown();
        //Regen the object's health.
        RegenHealth();
        //update animator wrapper to make animation run correctly
        animatorWrapper.DoUpdate();
        //update the renderer because of things....
        objectRenderer.DoUpdateRenderer();
    }

    protected virtual void ObjectIdle() {}

    //Only unit should run. This method shouldn't be in base object. But it just here so both Unit and Building share the same update loop
    protected virtual void ObjectRunning() {}
    //Similary normal object can't run, only units.
    protected virtual void ObjectCharging() {}

    protected virtual void ObjectAttack()
    {
        //it should be attack count up, shouldn't it ? LOL
        attackCountUp += Time.deltaTime;
        if (!isDamageDeal)
        {
            if (attackCountUp >= objectData.dealDamageTime)
            {
                DealDamageToTarget();
                isDamageDeal = true;
            }
        }
        else
        {
            if (attackCountUp >= objectData.attackDuration)
            {
                FinnishAttackTarget();
            }
        }
    }

    protected virtual void ObjectStun() { }

    protected virtual void ObjectSpecial() { }

    //overide for hero
    protected virtual void WhileObjectDie() { }

    //manager call - this function is called by manager or by player control
    //is local call: is this function called locally or externally
    //This used to be the just MOVE function, but I realize maybe building can be controlled and target to
    //Like set rally poit for building.
    //This will be the default function for mouse control interaction or manager interaction
    public virtual void SetObjectTargetPosition(Vector3 _targetPosition, bool isLocalCall) {}

    //If not, the order must be queued up.
    protected virtual bool CanExecuteOrder()
    {
        return GetObjectState() == ObjectState.Run
            || GetObjectState() == ObjectState.Charge
            || GetObjectState() == ObjectState.Idle;
    }

    //manager call - this function is called by manager or by player control
    public virtual void SetObjectTarget(BaseObject target, bool isLocalCall) {}

    protected void SetTargetObject(BaseObject _target)
    {
        targetObject = _target;
        targetID = _target.id;
    }

    protected bool IsTargetChanged()
    {
        return targetID != targetObject.id;    
    }

    protected bool IsTargetInRange()
    {
        if (Ultilities.GetDistanceBetween(gameObject, targetObject.gameObject) <= objectData.attackRange)
        //if ((transform.position - targetPosition).magnitude <= objectData.attackRange)
        {
            return true;
        }
        return false;
    }

    protected virtual void StartAttackTarget()
    {
        if (targetObject != null && !IsTargetChanged())
        {
            transform.LookAt(targetObject.transform.position);
            SetState(ObjectState.Attack);
        }
        else
        {
            SetState(ObjectState.Idle);
            return;
        }
        isDamageDeal = false;
        attackCountUp = 0;
    }

    //TODO recheck this function !!!!
    protected virtual void DealDamageToTarget()
    {
        if (targetObject != null && !IsTargetChanged())
        {
            projectileManager.CreateProjectile(GetProjectileType(), isEnemy, objectData.damange, transform.position, this, targetObject.transform.position, targetObject);
            //deal direct damage, but I don't know if this is needed :/
            //targetObject.ReceiveDamage(10, this);
        }
    }

    protected virtual void FinnishAttackTarget()
    {
        isDamageDeal = false;

        //The last check -> if enemy is attacking the cargo, while the hero is close, he should focus on the
        //hero instead. -> Might change later
        if (targetObject != null && CanTargetObject() && !IsTargetChanged() && IsTargetInRange())
        {
            StartAttackTarget();
        }
        else
        {
            BaseObject requestedTarget = objectManager.RequestTarget(this);
            if (requestedTarget == null || !IsTargetInRange() || !CanTargetObject() )
            {
                SetState(ObjectState.Idle);
            }
            else
            {
                SetTargetObject(requestedTarget);
                StartAttackTarget();
            }
        }
    }

    //Special skill of object. Usually use for hero but I might think of something clever later
    //Implement this to make sure if you CAN NOT activate the special, you WON'T
    public virtual bool ActiveSpecial()
    {
        if (CanActiveSpecial())
        {
            objectData.currentSpecialCoolDown = 0;
            return true;
        }
        return false;
    }

    protected virtual bool CanActiveSpecial()
    {
        return objectData.currentSpecialCoolDown >= objectData.specialCoolDown;
    }

    protected virtual void UpdateSpecialCountDown()
    {
        if (objectData.currentSpecialCoolDown < objectData.specialCoolDown)
        {
            objectData.currentSpecialCoolDown += Time.deltaTime;
        }
    }

    //Passing the attacker in to check detail
    public virtual void ReceiveDamage(int damage, BaseObject attacker = null)
    {
        //If the object is attacked, stop regen health for 3 seconds.
        if (GetHealthRegenRate() > 0f)
        {
            healthRegenCountUp = - GameConstant.attackStopRegenTime * GetHealthRegenRate() * objectData.maxHealth / 100f;
        }
        //Complex calculation here later if I want the game to get complex
        ReduceHealth(damage);
    }

    protected virtual void ReduceHealth(int damage)
    {
        objectData.health -= damage;
        objectRenderer.UpdateHealthBar();
        if (objectData.health <= 0)
        {
            OnObjectDie();
        } 
    }

    public virtual void OnObjectDie()
    {
        SetState(ObjectState.Die);

        DeadEffect();
        objectManager.RemoveObject(this);
        KillObject();
    }

    protected virtual void DeadEffect()
    {
        for (int i = 0; i < 9; i++)
        {
            Corpse corpse = PrefabsManager.SpawnCorpse(GetCorpseType());
            corpse.Init(transform.position);
        }
    }

    protected virtual void KillObject()
    {
        //Destroy(gameObject);
        ReturnToPool();
    }

    //temp variable
    private float healthRegenCountUp = 0f;

    protected virtual void RegenHealth()
    {
        if (GetHealthRegenRate() <= 0 || objectState == ObjectState.Die || objectData.health >= objectData.maxHealth)
        {
            return;
        }
        healthRegenCountUp += Time.deltaTime * GetHealthRegenRate() * objectData.maxHealth / 100f;
        if (healthRegenCountUp >= 1)
        {
            objectData.health = Mathf.Min((int)healthRegenCountUp + objectData.health, objectData.maxHealth);
            healthRegenCountUp -= (int)healthRegenCountUp;
            objectRenderer.UpdateHealthBar();
        }
    }

    //percent of max health regen per second
    //1 mean 1% of health every second
    protected virtual float GetHealthRegenRate() {
        return 0f;
    }

    //tobe called by the renderer function. Hero will have this function off.
    public virtual bool AutoHideHealthBar() { return true; }

    public virtual bool CanTargetObject()
    {
        return GetObjectState() != ObjectState.Die;
    }
}

public enum ObjectState
{
    Idle, //standing in one place and not doing anything
    Run, //runnnnnn
    Charge, //running to attack a target
    Attack, //attack target
    Stun, //object is stunned and can't do anything
    Special, //is doing special spell
    Die //die 
}