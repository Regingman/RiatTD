using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.self.tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();

        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();

        Node curretnNode = nodes[start];

        openList.Add(curretnNode);

        while (openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                    Point neighbourPos = new Point(curretnNode.GridPosition.X - x, curretnNode.GridPosition.Y - y);

                    if (LevelManager.self.InBounds(neighbourPos) && neighbourPos != curretnNode.GridPosition && LevelManager.self.tiles[neighbourPos].Walkable)
                    {
                        int gCost = 0;

                        if (Mathf.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        {
                            if (!ConnectedDiagonally(curretnNode, nodes[neighbourPos]))
                            {
                                continue;
                            }
                            gCost = 14;
                        }



                        Node neighbour = nodes[neighbourPos];
                        if (openList.Contains(neighbour))
                        {
                            if (curretnNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalcValues(curretnNode, nodes[goal], gCost);
                            }
                        }
                        else
                        if (!closedList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.CalcValues(curretnNode, nodes[goal], gCost);
                        }
                    }
                }
            }

            openList.Remove(curretnNode);
            closedList.Add(curretnNode);

            if (openList.Count > 0)
            {
                curretnNode = openList.OrderBy(n => n.F).First();

            }

            if (curretnNode == nodes[goal])
            {
                while (curretnNode.GridPosition != start)
                {
                    finalPath.Push(curretnNode);
                    curretnNode = curretnNode.parent;
                }
                break;
            }
        }

        //debug
        //AStarDebugger.self.DebugPath(openList, closedList, finalPath);
        return finalPath;
    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPosition - currentNode.GridPosition;
        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y + direction.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        if(LevelManager.self.InBounds(first) && !LevelManager.self.tiles[first].Walkable)
        {
            return false;
        }
        if(LevelManager.self.InBounds(second)&& !LevelManager.self.tiles[second].Walkable)
        {
            return false;
        }
        return true;
    }
}
