using UnityEngine;
using System.Collections;

//Hero = player team
public class BaseHero : BaseObject {
    //Is enemy = false ???
    [Header("Hero fields")]
    protected float deadCountDown;
    protected float maxDistantToCargo = 45;

    //cached object for calculating
    private CargoKart cargoKart;

    public HighLightCircle manaIndicationCircle;
    private float manaCircleStartSize;


    public override void Init(ObjectManager _objectManager, bool isEnemyTeam, int objectLevel)
    {
        base.Init(_objectManager, isEnemyTeam, objectLevel);
        cargoKart = Directors.instance.playerManager.GetCargoKart();
        manaIndicationCircle.Init();
        manaIndicationCircle.SetTargetGameObject(this.gameObject);
        manaCircleStartSize = manaIndicationCircle.transform.localScale.x;
        objectData.currentSpecialCoolDown = objectData.specialCoolDown;
    }

    protected override void AdditionalUpdateFunction()
    {
        base.AdditionalUpdateFunction();
        ValidateHeroDistanceToCargo();
        manaIndicationCircle.DoUpdate();
    }

    //private cached value, only used for the function beloew.
    private int validateHeroDistanceCheckCount = 20;
    //Only call this functinon every 10 or something frame to save performance.
    //Check if the hero is too far away from the cargo or not. If so, move him back
    private void ValidateHeroDistanceToCargo()
    {
        if (Directors.instance.GetBattleState() != BattleState.Battling)
        {
            return;
        }

        validateHeroDistanceCheckCount--;
        if (validateHeroDistanceCheckCount <= 0 
            && objectState != ObjectState.Die 
            && GetDistanceToCargo() >= maxDistantToCargo)
        {
            OnHeroVeryFarFromCargo();
            validateHeroDistanceCheckCount = 20;
        }
    }

    private float GetDistanceToCargo()
    {
        return (transform.position - cargoKart.transform.position).magnitude;
    }

    public override bool ActiveSpecial()
    {
        if (CanActiveSpecial())
        {
            SetManaIndicationCircleSize(0.001f);
        }
        return base.ActiveSpecial();
    }

    private void SetManaIndicationCircleSize(float percent) //range from 0 to 1
    {
        //1.25 = base value, set in the editor
        manaIndicationCircle.transform.localScale = Vector3.one * percent * manaCircleStartSize;
    }

    protected override void UpdateSpecialCountDown()
    {
        base.UpdateSpecialCountDown();
        if (objectData.currentSpecialCoolDown < objectData.specialCoolDown)
        {
            SetManaIndicationCircleSize(objectData.currentSpecialCoolDown / objectData.specialCoolDown);
        }
    }

    //Need a better function name.
    protected void OnHeroVeryFarFromCargo()
    {
        SetMovePosition(cargoKart.transform.position);
    }

    public override void OnObjectDie()
    {
        //Make mouse controller don't select this unit anymore
        //TODO make this less messy
        mouseController.DeselectObject(this);
        cameraController.ScreenShake(ScreenShakeMagnitude.Big);
        childAnimator.gameObject.SetActive(false);
        deadCountDown = objectData.respawnTime;
        manaIndicationCircle.gameObject.SetActive(false);
        SetState(ObjectState.Die);
    }

    protected override void WhileObjectDie()
    {
        base.WhileObjectDie();
        deadCountDown -= Time.deltaTime;
        if (deadCountDown <= 0)
        {
            //hero is alive.
            //this is very buggy, I don't know why
            //navMeshAgent.Move(PlayerManager.cargoKart.transform.position);
            transform.position = Directors.instance.playerManager.GetCargoKart().transform.position + new Vector3(Random.Range(-2f,2f), 0f, Random.Range(-2f, 2f));
            childAnimator.gameObject.SetActive(true);
            OnHeroRessurect();
        }
    }

    public virtual void OnHeroRessurect() {
        SetState(ObjectState.Idle);
        childAnimator.gameObject.SetActive(true);
        manaIndicationCircle.gameObject.SetActive(true);
        
        //I want to make that each time hero level up, he might gains a new skill instead of just raw data.
        //Make it way better.
        UpdateStatsByLevel(++objectData.level);
        objectRenderer.UpdateHealthBar(1f);
    }

    public override void UpdateStatsByLevel(int level)
    {
        objectData.level = level;
        objectData.maxHealth = objectData.maxHealth + (int)(objectData.maxHealth * (level - 1) * 0.22f);
        objectData.damange = objectData.damange + (int)(objectData.damange * (level - 1) * 0.22f);
        objectData.health = objectData.maxHealth;
    }

    protected override float GetHealthRegenRate()
    {
        return 1.25f;
    }

    public override bool AutoHideHealthBar() { return false; }
}
