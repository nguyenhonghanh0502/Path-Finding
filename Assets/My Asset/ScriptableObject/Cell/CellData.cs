using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellData", menuName = "Data/CellData")]
public class CellData : AData
{
    public GameObject Prefab;
    public float CellSize = 1f;

    private static List<GameObject> cells = new List<GameObject>();

    public Cell Create((int width, int height) sizeVal, (int x, int y) index, Transform parent, CellState cellState)
    {
        Vector3 offset = new Vector3((sizeVal.width - 1) * CellSize / 2, (sizeVal.height - 1) * CellSize / 2, 0);
        var pos = new Vector3(index.x * CellSize, index.y * CellSize) - offset;
        GameObject cell = Instantiate(Prefab, pos, Quaternion.identity, parent);
        cell.transform.localScale = new Vector3(CellSize, CellSize);
        cells.Add(cell);

        Cell c = cell.GetComponent<Cell>();
        if (c == null) c = cell.AddComponent<Cell>();
        c.UpdateState(cellState);
        return c;
    }
}

public enum CellState
{
    Empty = 0,
    Obstacles = 1,
    Start = 2,
    Goal = 3,
    Path = 4,
}
