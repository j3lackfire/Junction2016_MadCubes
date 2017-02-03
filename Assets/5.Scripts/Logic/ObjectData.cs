using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectData {
    //[HideInInspector]
    public int level;
    //defined value (set in the editor)
    public float moveSpeed;

    public int baseMaxHealth;
    public int baseDamage;

    public float attackRange;

    public float respawnTime; //only use for hero

    //This is the length of the attack animation
    public float attackDuration;
    public float dealDamageTime;

    public float specialCoolDown;

    //calculated value - code calculate value
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int damange;

    [HideInInspector]
    public int health; //current health

    [HideInInspector]
    public float sight;

    //[HideInInspector]
    public float currentSpecialCountDown;
}
