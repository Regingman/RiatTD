using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private bool flag = false;
    private Monster target;

    public Monster Target
    {
        get { return target; }
    }

    [SerializeField]
    private string projectileType;

    public float projectileSpeed = 3f;

    public int level = 1;

    private Queue<Monster> monsters = new Queue<Monster>();
    private bool canAttack = true;
    public float attackTime;
    public float damage = 10;
    private int towerUpdatePrice = 10;

    public int TowerUpdatePrice
    {
        get { return towerUpdatePrice * level; }
    }

    public float attackCooldown = 1f;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Shoot()
    {
        Projectile projectile = GameManager.self.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    private void Attack()
    {
        if (!canAttack)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackCooldown)
            {
                canAttack = true;
                attackTime = 0;
            }
        }

        if (target == null && monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }

        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Debug.Log("Выстрел");
                Shoot();
                canAttack = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Monster"))
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            target = null;
        }
    }

    public void Select()
    {
        if (flag)
        {
            mySpriteRenderer.enabled = flag;
            flag = false;
        }
        else
        {
            mySpriteRenderer.enabled = flag;
            flag = true;
        }
    }

    void Update()
    {
        Attack();
    }
}
