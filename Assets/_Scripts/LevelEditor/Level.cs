using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public List<FloorTile> grid;
    public int ColumnCount;
    public int RowCount;
    public int Target;

    public Level(List<FloorTile> grid, int ColumnCount, int RowCount, int Target)
    {
        this.grid = grid;
        this.ColumnCount = ColumnCount;
        this.RowCount = RowCount;
        this.Target = Target;
    }
}
