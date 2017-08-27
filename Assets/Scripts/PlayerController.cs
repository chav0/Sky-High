using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Point a, Point b)
    {
        return (a.X == b.X && a.Y == b.Y);
    }
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }
    public override bool Equals(System.Object obj)
    {
        // If parameter cannot be cast to ThreeDPoint return false:
        Point p = obj as Point;
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return base.Equals(obj) && X == p.X && Y == p.Y;
    }
}

public class PathNode
{
    // Координаты точки на карте.
    public Point Position { get; set; }
    // Длина пути от старта (G).
    public int PathLengthFromStart { get; set; }
    // Точка, из которой пришли в эту точку.
    public PathNode CameFrom { get; set; }
    // Примерное расстояние до цели (H).
    public int HeuristicEstimatePathLength { get; set; }
    // Ожидаемое полное расстояние до цели (F).
    public int EstimateFullPathLength;
}

public class Astar
{
    public static List<Point> FindPath(int[,] field, Point start, Point goal)
    {
        // Шаг 1.
        Debug.Log("0");
        var closedSet = new Collection<PathNode>();
        var openSet = new Collection<PathNode>();
        Debug.Log("1");
        // Шаг 2.
        PathNode startNode = new PathNode()
        {
            Position = start,
            CameFrom = null,
            PathLengthFromStart = 0,
            HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal),
            EstimateFullPathLength = GetHeuristicPathLength(start, goal)
        };
        openSet.Add(startNode);
        //Debug.Log("2");
        while (openSet.Count > 0)
        {
            // Шаг 3.
            var currentNode = openSet.OrderBy(node => node.EstimateFullPathLength).First();
            // Шаг 4.
            if (currentNode.Position == goal)
                return GetPathForNode(currentNode);
            // Шаг 5.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            // Шаг 6.
            foreach (var neighbourNode in GetNeighbours(currentNode, goal, field))
            {
                // Шаг 7.
                if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                    continue;
                var openNode = openSet.FirstOrDefault(node =>
                  node.Position == neighbourNode.Position);
                // Шаг 8.
                if (openNode == null)
                {
                    openSet.Add(neighbourNode);
                }
                else
                  if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                {
                    // Шаг 9.
                    openNode.CameFrom = currentNode;
                    openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                }
            }
        }
        // Шаг 10.
        return null;
    }

    private static int GetDistanceBetweenNeighbours()
    {
        return 1;
    }

    private static int GetHeuristicPathLength(Point from, Point to)
    {
        return Mathf.Abs(from.X - to.X) + Mathf.Abs(from.Y - to.Y);
    }

    private static Collection<PathNode> GetNeighbours(PathNode pathNode,
  Point goal, int[,] field)
    {
        var result = new Collection<PathNode>();

        // Соседними точками являются соседние по стороне клетки.
        Point[] neighbourPoints = new Point[4];
        neighbourPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
        neighbourPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
        neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
        neighbourPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

        foreach (var point in neighbourPoints)
        {
            // Проверяем, что не вышли за границы карты.
            if (point.X < 0 || point.X >= field.GetLength(0))
                continue;
            if (point.Y < 0 || point.Y >= field.GetLength(1))
                continue;
            // Проверяем, что по клетке можно ходить.
            if ((field[point.X, point.Y] != 0))
                continue;
            // Заполняем данные для точки маршрута.
            var neighbourNode = new PathNode()
            {
                Position = point,
                CameFrom = pathNode,
                PathLengthFromStart = pathNode.PathLengthFromStart +
                GetDistanceBetweenNeighbours(),
                HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal),
                EstimateFullPathLength = pathNode.PathLengthFromStart +
                GetDistanceBetweenNeighbours() + GetHeuristicPathLength(point, goal)
            };
            result.Add(neighbourNode);
        }
        return result;
    }

    private static List<Point> GetPathForNode(PathNode pathNode)
    {
        var result = new List<Point>();
        var currentNode = pathNode;
        while (currentNode != null)
        {
            result.Add(currentNode.Position);
            currentNode = currentNode.CameFrom;
        }
        result.Reverse();
        return result;
    }
}

public class PlayerController : MonoBehaviour
{
    private List<Point> path = null;
    private int current_position_in_path = 1;
    public static int player_hp; 

    void Start()
    {
        player_hp = Settings._player_hp; 
    }

    int normalize(int v)
    {
        if (v == 0) return 0;
        if (v < 0) return -1;
        return 1; 
    }

    void FixedUpdate()
    {
        if (Target._new)
        {
            Target._new = false;
            int[,] field = new int[CellGenerator.levelGrid.Length, CellGenerator.levelGrid[0].Length];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (CellGenerator.levelGrid[i][j].GetComponent<Cube>()._color == 0) field[i, j] = 0;
                    else field[i, j] = 1;
                }
            }

            Point current_position = new Point((int)Mathf.Round(transform.position.x / 10.0f), (int)Mathf.Round(transform.position.z / 10.0f));
            Debug.Log(Target._target.X); 
            path = Astar.FindPath(field, current_position, Target._target);
            Target._new = false;
            current_position_in_path = 1;
        }
        if (path != null && Target._target != new Point(-1, -1) && current_position_in_path <= path.Count - 1)
        {
            var heading = new Vector2((int)Mathf.Round(path[current_position_in_path].X * 10.0f - transform.position.x), (int)Mathf.Round(path[current_position_in_path].Y * 10.0f - transform.position.z)); 
            if (heading.magnitude >= 0.01f)
            {
                transform.position = transform.position + new Vector3(normalize((int)Mathf.Round(heading.x)), 0, normalize((int)Mathf.Round(heading.y))) * Time.deltaTime*40;
                if (path != null && current_position_in_path == path.Count - 1 && heading.magnitude <= 0.1f)
                {
                    path = null;
                    current_position_in_path = 1;
                }
            }
            else
            {
                    transform.position = new Vector3(path[current_position_in_path].X * 10.0f, 0, path[current_position_in_path].Y * 10.0f);
                    current_position_in_path++;
            }

            //transform.position = Vector3.Lerp(path[current_position_in_path-1], path[current_position_in_path], Time.deltaTime * 40.0f);
        }
    }
}
