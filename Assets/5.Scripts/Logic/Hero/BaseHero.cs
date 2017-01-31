using UnityEngine;
using System.Collections;

//Hero = player team
public class BaseHero : BaseObject {
    //Is enemy = false ???
    protected float deadCountDown;
    protected float maxDistantToCargo =25;

    //private object for calculating
    private CargoKart cargoKart;

    public override void Init(ObjectManager _objectManager, bool isEnemyTeam, int objectLevel)
    {
        base.Init(_objectManager, isEnemyTeam, objectLevel);
        Debug.Log("<color=red> Update hero stats by level </color>" + objectLevel);
        cargoKart = Directors.instance.playerManager.GetCargoKart();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        ValidateHeroDistanceToCargo();
        
    }

    //private cached value, only used for the function beloew.
    private int validateHeroDistanceCheckCount = 20;
    //Only call this functinon every 10 or something frame to save performance.
    //Check if the hero is too far away from the cargo or not. If so, move him back
    private void ValidateHeroDistanceToCargo()
    {
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

    //Need a better function name.
    protected void OnHeroVeryFarFromCargo()
    {
        Debug.Log("<color=#123acb> On hero very far from cargo </color>");
        SetMovePosition(cargoKart.transform.position);
    }

    //hero should have skill, shouldn't he ???
    public virtual void ActiveSkill() { }

    public override void OnObjectDie()
    {
        //Make mouse controller don't select this unit anymore
        //TODO make this less messy
        mouseController.DeselectObject(this);
        cameraController.ScreenShake(ScreenShakeMagnitude.Big);
        childAnimator.gameObject.SetActive(false);
        deadCountDown = objectData.respawnTime;
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
            SetState(ObjectState.Idle);
            OnHeroRessurect();
        }
    }

    public virtual void OnHeroRessurect() {
        SetState(ObjectState.Idle);
        childAnimator.gameObject.SetActive(true);
        //errr ?
        //I want to make that each time hero level up, he might gains a new skill instead of just raw data.
        //Make it way better.
        objectData.level++;
        UpdateStatsByLevel(objectData.level);
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
