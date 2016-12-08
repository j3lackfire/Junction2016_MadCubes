using UnityEngine;
using System.Collections;

//Hero = player team
public class BaseHero : BaseObject {
    //Is enemy = false ???
    protected float deadCountDown;

    //hero should have skill, shouldn't he ???
    public virtual void ActiveSkill() { }

    public override void OnObjectDie()
    {
        //Make mouse controller don't select this unit anymore
        //TODO make this less messy
        if (mouseController.currentlySelectedHero == this)
        {
            mouseController.currentlySelectedHero = null;
            mouseController.hightLightCircle.transform.gameObject.SetActive(false);
        }
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
            transform.position = Directors.playerManager.cargoKart.transform.position + new Vector3(Random.Range(-2f,2f), 0f, Random.Range(-2f, 2f));
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
        objectData.maxHealth = objectData.maxHealth + (int)(objectData.maxHealth * level * 0.22f);
        objectData.damange = objectData.damange + (int)(objectData.damange * level * 0.22f);
        objectData.health = objectData.maxHealth;
    }
}
