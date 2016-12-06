using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectData {
    //defined value (set in the editor)
    public float moveSpeed;

    public int maxHealth;

    public int damange;
    public float attackRange;

    public float respawnTime; //only use for hero

    public float attackDuration;
    public float dealDamageTime;

    //calculated value - code calculate value
    [HideInInspector]
    public int health;

    [HideInInspector]
    public int level;
    [HideInInspector]
    public float sight;
}
