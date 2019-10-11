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

    private Point blueSpawn, redSpawn;

    [SerializeField]
    private GameObject portal;

    public Dictionary<Point, TileScript> tiles { get; set; }


   
   

    /// <summary>
    ///  Свойства дял возврата размера тайла
    /// </summary>
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Start()
    {
        CreateLevel();
    }



    /// <summary>
    /// Создание уровня
    /// </summary>
    void CreateLevel()
    {

        tiles = new Dictionary<Point, TileScript>();


        string[] mapData = ReadLevelText();

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
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0));

        tiles.Add(new Point(x, y), newTile);

        SpawnPortals();
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(0, 0);

        Instantiate(portal, tiles[blueSpawn].transform.position, Quaternion.identity);

    }
}
