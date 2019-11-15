using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Tower parent;
    private Monster target;
    private void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, parent.projectileSpeed * Time.deltaTime);
            Vector2 dir = target.transform.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if(!target.IsActive)
        {
            GameManager.self.Pool.ReleaseObject(gameObject);
        }
    }

}
