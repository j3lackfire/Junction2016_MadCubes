using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour {

    List<BasicProjectile> projectileList = new List<BasicProjectile>();

    public void Init() { }
    
    public void DoUpdate()
    {
        for (int i = 0; i < projectileList.Count; i++)
        {
            projectileList[i].DoUpdate();
        }
    }

    public BasicProjectile CreateProjectile(ProjectileType type, bool _isEnemy, int damage, Vector3 startPos, BaseElementObject target, GameElement bulletType)
    {
        BasicProjectile bp = PrefabsManager.SpawnProjectile(type);
        bp = GameObject.Instantiate(bp);
        if (type == ProjectileType.Fire_Hero)
        {
            bp.Init(_isEnemy, damage, startPos, target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),Random.Range(-0.5f, 0.5f))
                , target, bulletType);
            bp.isChaseBullet = false;
        }
        else
        {
            if(type == ProjectileType.Water_Hero)
            {
                bp.Init(_isEnemy, damage, startPos, target.transform.position, target, bulletType);
            }
            else
            {
                bp.Init(_isEnemy, damage, startPos, target.transform.position, target, bulletType);
            }
        }
        projectileList.Add(bp);
        return bp;
    }

    public void RemoveProjectile(BasicProjectile p)
    {
        projectileList.Remove(p);
    }

}
