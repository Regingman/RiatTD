using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public bool IsEmpty { get; private set; }

    private Color32 fullcColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    public bool Walkable { get; set; }

    public bool Debugging { get; set; }

    private Tower myTower;


    private Color vector;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));

        }

    }


    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {

        Walkable = true;
        IsEmpty = true;

        this.GridPosition = gridPos;

        transform.position = worldPos;

        transform.SetParent(parent);

        LevelManager.self.tiles.Add(gridPos, this);

    }

    private void OnMouseOver()
    {
        //        ColorTile(fullcColor);
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.self.clickedBtn != null)
        {
            if (IsEmpty && !Debugging)
            {

                ColorTile(emptyColor);
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            else if (!IsEmpty && !Debugging)
            {
                ColorTile(fullcColor);
            }


        }
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.self.clickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (transform.GetChild(0) != null)
            {
                myTower = transform.GetChild(0).GetComponent<Tower>();
            }

            if (myTower != null)
            {
                GameManager.self.SelectTower(myTower);
            }
            else
            {
                GameManager.self.DeseletTower();
            }
        }
    }

    private void OnMouseExit()
    {
        if (!Debugging)
            ColorTile(Color.white);
    }

    private void PlaceTower()
    {
        //Debug.Log("Place tower in " + GridPosition.X + "," + GridPosition.Y);

        GameObject tower = Instantiate(GameManager.self.clickedBtn.TowerPrefab, transform.position, Quaternion.identity);

        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);

        myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        IsEmpty = false;
        // 
        GameManager.self.BuyTower();
        Walkable = false;

    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
