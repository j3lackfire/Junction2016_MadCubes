using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : BaseManager
{ 
    List<BasicProjectile> projectileList = new List<BasicProjectile>();

    public override void Init() { }
    
    public override void DoUpdate()
    {
        for (int i = 0; i < projectileList.Count; i++)
        {
            projectileList[i].DoUpdate();
        }
    }

    public BasicProjectile CreateProjectile(ProjectileType type, bool _isEnemy, int _damage, Vector3 _startPos, BaseObject _attacker, Vector3 _endPos, BaseObject _target)
    {
        BasicProjectile projectile = PrefabsManager.SpawnProjectile(type);
        projectile = Instantiate(projectile);
        projectile.Init(type, _isEnemy, _damage);
        projectile.InitPosition(_startPos, _endPos);
        projectile.InitObjects(_attacker, _target);

        projectileList.Add(projectile);
        return projectile;
    }

    public void RemoveProjectile(BasicProjectile p)
    {
        projectileList.Remove(p);
    }

}
