using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform GameArea;
    [SerializeField] Transform StackRoot;

    [Header("Prefabs")]
    [SerializeField] GameObject FlatSurface;
    [SerializeField] GameObject Block;
    [SerializeField] GameObject StackPrefab;

    public static FloorElement[,] Grid;
    public static Stack[,] StackMap;

    int moveCount = 0;

    IEnumerator Start()
    {
        EventManager.Instance.OnDirectionSent += AssessStacks;
        EventManager.Instance.OnDirectionSent += UpdateMoveCount;


        yield return new WaitForEndOfFrame();
        BuildGameArea(GameManager.Instance.FetchLevel());
    }

    void OnDestroy()
    {
        EventManager.Instance.OnDirectionSent -= AssessStacks;
        EventManager.Instance.OnDirectionSent -= UpdateMoveCount;
    }


    void BuildGameArea(Level level)
    {
        Grid = new FloorElement[level.RowCount, level.ColumnCount];
        StackMap = new Stack[level.RowCount, level.ColumnCount];
        foreach (var e in level.grid)
        {
            GameObject obj;

            if (e.isBlock)
            {
                obj = Instantiate(Block, GameArea);
                obj.GetComponent<FloorElement>().Type = FloorElement.ElementType.Block;

            }
            else
            { 
                obj = Instantiate(FlatSurface, GameArea);
                obj.GetComponent<FloorElement>().Type = FloorElement.ElementType.Flat;

            }

            FloorElement element = obj.GetComponent<FloorElement>();
            element.Column = e.col;
            element.Row = e.row;
            if(element.Type == FloorElement.ElementType.Flat)
                obj.transform.position = new Vector3(e.col, 0, -e.row);
            else if (element.Type == FloorElement.ElementType.Block)
                obj.transform.position = new Vector3(e.col, 1, -e.row);
            Grid[e.row, e.col] = element;

        }

        for (int i = 0; i < GameManager.StartingTileCount; i++)
        {
            SpawnNewStack();
        }

        UIManager.Instance.PrintTarget(level.Target);
        GameObject cam = Camera.main.transform.gameObject;
        cam.GetComponent<Cam>().SetCamera(level);
    }

    void SpawnNewStack()
    {
        List<FloorElement> availableTiles = new List<FloorElement>();

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j].Type == FloorElement.ElementType.Flat)
                {
                    if (Grid[i, j].OccupantStack == null)
                    {
                        availableTiles.Add(Grid[i, j]);
                    }
                }
            }
        }

        if (availableTiles.Count == 0)
        {
            EventManager.Instance.LevelEnded(GameManager.GameState.Lose);
            return;
        }

        int indx = Random.Range(0, availableTiles.Count);
        FloorElement selectedFlat = availableTiles[indx];
        GameObject stackObj = Instantiate(StackPrefab, StackRoot);
        stackObj.transform.position = new Vector3(selectedFlat.Column, 0, -selectedFlat.Row);
        Stack stack = stackObj.GetComponent<Stack>();
        stack.SetCoordinate(selectedFlat.Row, selectedFlat.Column);
        selectedFlat.OccupantStack = stack;
        StackMap[selectedFlat.Row, selectedFlat.Column] = stack;
        stack.UpdateColors();
        stack.board = this;

    }



    public void AssessStacks(InputManager.DirectionType direction)
    {

        int col = Grid.GetLength(1);
        int row = Grid.GetLength(0);
        switch (direction)
        {
            case InputManager.DirectionType.Left:

                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (StackMap[i, j] == null)
                            continue;

                        StackMap[i, j].CheckDirection(direction, i, j);

                    }
                }

                break;

            case InputManager.DirectionType.Right:

                for (int i = col-1; i >= 0; i--)
                {
                    for (int j = row-1; j >= 0; j--)
                    {
                        if (StackMap[i, j] == null)
                            continue;
                        StackMap[i, j].CheckDirection(direction, i,j);
                    }
                }

                break;

            case InputManager.DirectionType.Upwards:
               
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (StackMap[i, j] == null)
                            continue;

                        StackMap[i, j].CheckDirection(direction, i, j);
                    }
                }
                break;

            case InputManager.DirectionType.Downwards:

                for (int i = row-1; i>=0; i--)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (StackMap[i, j] == null)
                            continue;

                        StackMap[i, j].CheckDirection(direction, i, j);
                    }
                }
                break;
        }
        
    }

    void UpdateMoveCount(InputManager.DirectionType direction)
    {
        moveCount++;

        if (moveCount % GameManager.Instance.SpawnRatio == 0)
            StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(0.5f);

        SpawnNewStack();
    }
}
