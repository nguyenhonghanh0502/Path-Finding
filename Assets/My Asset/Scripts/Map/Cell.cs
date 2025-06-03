using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    private CellState state;
    private (int x, int y) pos;

    public void UpdateState(CellState stateVal)
    {
        state = stateVal;
        sr.color = state switch
        {
            CellState.Empty => Color.white,
            CellState.Obstacles => Color.grey,
            CellState.Start => Color.green,
            CellState.Goal => Color.red,
            CellState.Path => Color.yellow,
            _ => Color.white
        };
    }

    public bool IsSamePos((int x, int y) val)
    {
        return pos.x == val.x && pos.y == val.y;
    }
}
