using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager self;
    public TowerBtn clickedBtn { get; set; }
    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString() + "<color=lime>$</color>";
        }
    }

    [SerializeField]
    private int currency;
    [SerializeField]
    private Text currencyTxt;

    public int gamePlayLevel;

    private int wave = 0;

    public int lives = 5;

    public Text waveText;

    public Text levelText;

    public Text WaveText;

    public Text liveText;

    public int Level = 1;



    public int maxWave = 3;

    [SerializeField]
    private GameObject waveBtn;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private Text upgradeBtnText;


    public Tower tower;

    [SerializeField]
    private List<Monster> activeMonsters = new List<Monster>();

    public bool WaveActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }


    public ObjectPool Pool { get; set; }
    /// <summary>
    /// Initialize Singleton
    /// </summary>
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
        if (self == null)
        {
            self = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        currencyTxt.text = Currency + "$";
    }

    void Update()
    {
        HandleEscape();
    }

    /// <summary>
    /// Pick Tower
    /// </summary>
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && !WaveActive)
        {
            clickedBtn = towerBtn;
            Hover.self.Activate(towerBtn.Sprite);
        }

    }

    public void Restart(int level, int gamePlayLvl)
    {
        maxWave = 1 + (level * 2);
        lives = 5 - gamePlayLvl;
        Currency = 10 - gamePlayLvl;
        wave = 0;
        Level = level;
        levelText.text = "Level: " + Level;
        liveText.text = "Lives: " + lives;
        gamePlayLevel = gamePlayLvl;
    }

    public void BuyTower()
    {
        if (Currency >= clickedBtn.Price)
        {
            Currency -= clickedBtn.Price;
            Hover.self.Deactivate();
            clickedBtn = null;
        }

    }


    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.self.Deactivate();
        }
    }

    public void StartWave()
    {
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave) + "\nОсталось волн: " + (maxWave - wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
        WaveText.text = "Next Wave";
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.self.GeneratePath();
        for (int i = 0; i < wave; i++)
        {
            LevelManager.self.GeneratePath();
            int monsterIndex = Random.Range(0, 2);
            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "Soldier";
                    break;
                case 1:
                    type = "Soldier";
                    break;
                case 2:
                    type = "Soldier";
                    break;
            }
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

            activeMonsters.Add(monster);
            yield return new WaitForSeconds(2.5f);
        }


    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if (!WaveActive)
        {
            if (maxWave - wave == 0)
            {
                Level++;
                waveText.text = "Поздравляю вы победили!";
                Restart(Level, gamePlayLevel);
                WaveText.text = "Next Level";
                waveBtn.SetActive(true);

            }
            else
            {
                waveBtn.SetActive(true);
            }
        }
    }

    public void SelectTower(Tower tower)
    {

        this.tower = tower;
        this.tower.Select();

        if (!upgradePanel.activeSelf)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            upgradePanel.SetActive(false);
        }
    }

    public void DeseletTower()
    {

        if (this.tower != null)
        {
            this.tower.Select();
        }

        this.tower = null;
    }

    public void UpgradeBtn()
    {
        tower.level += 1;
        upgradeBtnText.text = "Уровень башни равен " + tower.level;
        Currency -= tower.TowerUpdatePrice;
    }
}
