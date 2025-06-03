
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid Data", menuName = "Data/GridData")]
public class GridData : AData
{
    [SerializeField]
    private Vector2Int size = new Vector2Int(-1, -1);

    [SerializeField]
    private Vector2Int start = new Vector2Int(-1, -1);

    [SerializeField]
    private Vector2Int goal = new Vector2Int(-1, -1);

    public CellData CellData;
    public (int min, int max) WidthLimits = (5, 10);
    public (int min, int max) HeighthLimits = (5, 10);
    public float ObstacleProbability = 0.2f;

    #region Init
    public GridData (CellData cellData, (int width, int height) sizeVal)
    {
        CellData = cellData;
        int[,] geid = GenerateGrid(sizeVal);
    }

    public void WithSize(Vector2Int val)
    {
        size = val;
    }

    public void WithStart(Vector2Int val)
    {
        start = val;
    }

    public void WithGoal(Vector2Int val)
    {
        goal = val;
    }
    #endregion

    #region Load Data & Spawn
    public Vector2Int LoadSize()
    {
        Vector2Int finalSize = size;
        if (finalSize == new Vector2Int(-1, -1))
        {
            int width = Random.Range(WidthLimits.min, WidthLimits.max);
            int height = Random.Range(HeighthLimits.min, HeighthLimits.max);
            finalSize = new Vector2Int(width, height);
        }
        return finalSize;
    }

    public Vector2Int GetStart(int[,] grid)
    {
        var start = this.start;

        if (start == new Vector2Int(-1, -1))
        {
            do
            {
                start = new(Random.Range(0, grid.GetLength(0)), Random.Range(0, grid.GetLength(1)));
            } while (grid[start.x, start.y] == 1);

            SetStart(start);
        }
        grid[start.x, start.y] = 2;
        return start;
    }

    public Vector2Int GetGoal(int[,] grid, Vector2Int start)
    {
        var goal = this.goal;
        if (goal == new Vector2Int(-1, -1))
        {
            do
            {
                goal = new(Random.Range(0, grid.GetLength(0)), Random.Range(0, grid.GetLength(1)));
            } while (grid[goal.x, goal.y] == 1 || goal == start);

            SetGoal(goal);
        }
        grid[goal.x, goal.y] = 3;
        return goal;
    }

    public Vector2Int SetStart(Vector2Int val)
    {
        start = val;
        return start;
    }

    public Vector2Int SetGoal(Vector2Int val)
    {
        goal = val;
        return goal;
    }

    public int[,] GenerateGrid((int width, int height) sizeVal)
    {
        int[ , ] grid = new int[sizeVal.width, sizeVal.height];
        for (int i = 0; i < sizeVal.width; i++)
        {
            for (int j = 0; j < sizeVal.height; j++)
            {
                grid[i, j] = Random.value < ObstacleProbability ? 1 : 0;
            }
        }
        return grid;
    }

    public Dictionary<Vector2Int, Cell> FactoryGridMap(int[,] grid, Transform parent)
    {
        Dictionary<Vector2Int, Cell> result = new ();

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = CellData.Create((width, height), (i, j), parent, GetCellState(grid[i, j]));
                result.Add(new Vector2Int(i, j), cell);
            }
        }
        return result;
    }

    private CellState GetCellState(int val)
    {
        return val switch
        {
            0 => CellState.Empty,
            1 => CellState.Obstacles,
            2 => CellState.Start,
            3 => CellState.Goal,
            4 => CellState.Path,
            _ => CellState.Empty
        };
    }
    #endregion
}
