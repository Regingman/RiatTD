using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;

    private float tempSpeed;

    public SpriteRenderer sprite;
    private Color color;
    private Stack<Node> path;
    public Canvas canvas;


    public SimpleHealthBar healthBar;

    public float health = 30;
    public float maxHealth;

    private Point GridPosition { get; set; }

    private Vector3 destination;

    private void Awake()
    {
        maxHealth = health;
        healthBar.UpdateBar(health, maxHealth);
        sprite = gameObject.GetComponent<SpriteRenderer>();
        color = sprite.color;
    }

    private void Update()
    {
        Move();

    }

    public bool IsActive { get; set; }

    public void Spawn()
    {
        transform.position = LevelManager.self.bluePortal.transform.position;
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1f, 1f), false));
        SetPath(LevelManager.self.Path);
        tempSpeed = maxSpeed;
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        // IsActive = false;
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;
        IsActive = true;
        if (remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)
        {
            Vector3 mousePosition = destination; //положение мыши из экранных в мировые координаты
            var angle = Vector2.Angle(Vector2.up, mousePosition - transform.position);//угол между вектором от объекта к мыше и осью х
            transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);//немного магии на последок
            transform.position = Vector2.MoveTowards(transform.position, destination, maxSpeed * Time.deltaTime);


            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }

    }

    private void SetPath(Stack<Node> path)
    {
        if (path != null)
        {
            this.path = path;

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedPortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            GameManager.self.lives--;
            GameManager.self.liveText.text = "Lives: " + GameManager.self.lives;
            if (GameManager.self.lives == 0)
            {
                GameManager.self.waveText.text = "К сожалению вы проиграли!";
                GameManager.self.WaveText.text = "Restart!";
                GameManager.self.Restart(1, GameManager.self.gamePlayLevel);
            }
        }


        if (other.tag == "projectile")
        {
            Debug.Log(other.name);
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            health -= projectile.parent.damage * projectile.parent.level;
            healthBar.UpdateBar(health, maxHealth);
            GameManager.self.Pool.ReleaseObject(projectile.gameObject);
            sprite.color = projectile.parent.color;
            switch (projectile.parent.towerEffect)
            {
                case TowerEffect.doubleDamage:
                    health -= projectile.parent.damage * projectile.parent.level;
                    healthBar.UpdateBar(health, maxHealth);
                    break;
                case TowerEffect.slow:
                    maxSpeed = 0.2f;
                    break;
            }
            Invoke("ColorRe", 0.5f);
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (health <= 0)
        {
            GameManager.self.Currency += 2;
            Release();
            health = maxHealth;
        }
    }

    private void Release()
    {
        IsActive = false;
        GridPosition = LevelManager.self.blueSpawn;
        GameManager.self.Pool.ReleaseObject(gameObject);
        GameManager.self.RemoveMonster(this);
    }

    void ColorRe()
    {
        sprite.color = color;
        maxSpeed = tempSpeed;
    }

}


