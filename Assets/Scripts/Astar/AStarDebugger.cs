using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{

    private TileScript start, goal;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private GameObject debugTilePrefab;

    public static AStarDebugger self;

    private void Awake()
    {
        if (self == null)
        {
            self = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

     private void Update()
     {
         ClickTile();
         if (Input.GetKeyDown(KeyCode.Space))
         {
             Debug.Log("start path");
             AStar.GetPath(start.GridPosition, goal.GridPosition);
         }
     }

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();
                if (tmp != null)
                {
                    if (start == null)
                    {
                        start = tmp;
                        start.Debugging = true;
                        CreateDebugTile(start.WorldPosition, new Color32(255, 135, 0, 255));
                    }
                    else if (goal == null)
                    {
                        goal = tmp;
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));
                        goal.Debugging = true;
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
    {
        foreach (Node node in openList)
        {
            if (node.TileRef != start && node.TileRef != goal)
                CreateDebugTile(node.TileRef.WorldPosition, Color.cyan, node);

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in path)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);
            }

        }
    }

    private void PointToParent(Node node, Vector2 position)
    {

        if (node.parent != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;

            //Right
            if ((node.GridPosition.X < node.parent.GridPosition.X) && (node.GridPosition.Y == node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            //top right
            else if ((node.GridPosition.X < node.parent.GridPosition.X) && (node.GridPosition.Y > node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            //UP
            else if ((node.GridPosition.X == node.parent.GridPosition.X) && (node.GridPosition.Y > node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            //top left
            else if ((node.GridPosition.X > node.parent.GridPosition.X) && (node.GridPosition.Y > node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            //left
            else if ((node.GridPosition.X > node.parent.GridPosition.X) && (node.GridPosition.Y == node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            //bottom left
            else if ((node.GridPosition.X > node.parent.GridPosition.X) && (node.GridPosition.Y < node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            //bottom 
            else if ((node.GridPosition.X == node.parent.GridPosition.X) && (node.GridPosition.Y < node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            //bottom rigth
            else if ((node.GridPosition.X < node.parent.GridPosition.X) && (node.GridPosition.Y < node.parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }

        }
    }

    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos, Quaternion.identity);
        if (node != null)
        {
            DebugTile tmp = debugTile.GetComponent<DebugTile>();

            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;

        }
        debugTile.GetComponent<SpriteRenderer>().color = color;

    }
}
