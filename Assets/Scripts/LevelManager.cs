using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Префаб создаваемого тайла
    /// </summary>
    [Header("Шапка")]
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    public Portal bluePortal { get; set; }

    public Point blueSpawn, redSpawn;

    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private GameObject portalRed;

    public Dictionary<Point, TileScript> tiles { get; set; }

    public static LevelManager self;


    private void Initialize()
    {

        if (self == null)
        {
            Debug.Log("init");
            self = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    ///  Свойства дял возврата размера тайла
    /// </summary>
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Start()
    {
        Initialize();
        CreateLevel();
    }

    private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(path));
        }

    }

    public void SwapInt(int a, int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }


    /// <summary>
    /// Создание уровня
    /// </summary>
    void CreateLevel()
    {

        tiles = new Dictionary<Point, TileScript>();


        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        //Вычисление длины карты по координате x 
        int mapX = mapData[0].ToCharArray().Length;

        //Вычисление длины карты по координате y
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //вычисление стартовой позиции относительно левой границы экрана
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) // y position
        {

            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++) //x position
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    /// <summary>
    /// Генераций нового тайла на карте
    /// </summary>
    /// <param name="tileTypes"> Тип тайла который будет расположен на карте</param>
    /// <param name="worldStart">Стартовая позиция карты</param>
    /// <param name="x">Позиция тайла по координате x</param>
    /// <param name="y">Позиция тайла по координате y</param>
    private void PlaceTile(string tileTypes, int x, int y, Vector3 worldStart)
    {
        //Преобразование tileTypes в int, для того чтобы использовать при генерации нового тайла
        int tileIndex = int.Parse(tileTypes);

        //Создание нового тайла 
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //изменение положения тайла при помощи переменных x,y и worldStart
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);


    }

    private string[] ReadLevelText()
    {
        int mapNumber = PlayerPrefs.GetInt("mapNumber");
        TextAsset bindData = Resources.Load("Level" + mapNumber) as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(0, 0);

        GameObject tmp = Instantiate(portal, tiles[blueSpawn].transform.position, Quaternion.identity);
        bluePortal = tmp.GetComponent<Portal>();
        bluePortal.name = "BluePortal";

        redSpawn = new Point(17, 5);

        Instantiate(portalRed, tiles[redSpawn].transform.position, Quaternion.identity);


    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(blueSpawn, redSpawn);
    }
}
