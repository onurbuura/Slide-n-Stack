using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FloorTile
{
    public bool isBlock;
    public int col;
    public int row;

    public FloorTile(bool isBlock, int col, int row)
    {
        this.isBlock = isBlock;
        this.col = col;
        this.row = row;
    }
}
