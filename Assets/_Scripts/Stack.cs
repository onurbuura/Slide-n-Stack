using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Stack : MonoBehaviour
{
    public int Value;
    [SerializeField] GameObject ValIndicatorPrefab;
    GameObject valIndicator;
    public List<GameObject> plates = new List<GameObject>();
    int TotalRow, TotalColumn;
    public Board board;
    public bool isEngaged;
    int Row, Column;

    private void Start()
    {
        EventManager.Instance.OnLevelEnded += ToggleUIElements;

        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        valIndicator = Instantiate(ValIndicatorPrefab, canvas);
        

        UpdateValue();
        UpdateStacks();

        TotalColumn = Board.Grid.GetLength(1);
        TotalRow = Board.Grid.GetLength(0);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnLevelEnded -= ToggleUIElements;
    }

    private void Update()
    {
        if (valIndicator != null)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            valIndicator.transform.position = screenPoint;
        }
    }

    public void SetCoordinate(int row, int col)
    {
        Row = row;
        Column = col;
    }

    public void UpdateColors()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (var e in meshes)
        {
            e.material.color = UIManager.Instance.ReturnColor(Value);
        }
    }

    void UpdateStacks()
    {
        List<MeshRenderer> meshes = GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var e in meshes)
        {
            plates.Add(e.gameObject);
        }
    }

    void ToggleUIElements(GameManager.GameState state)
    {
        if (state != GameManager.GameState.Play)
        {
            Destroy(valIndicator);
        }
    }

    void UpdateValue()
    {
        valIndicator.GetComponent<TMP_Text>().text = Value.ToString();
    }

    public void CheckDirection(InputManager.DirectionType direction, int row, int column)
    {
        
        FloorElement current = Board.Grid[row, column];
        FloorElement target = current;
        int availableTiles = 0;

        if (direction == InputManager.DirectionType.Left)
        { 
                for (int i = column; i >= 0; i--)
                {
                    if (Board.Grid[row, i].Type == FloorElement.ElementType.Block)
                    {
                        
                    break;
                    }

                if (Board.Grid[row, i].OccupantStack == null)
                    availableTiles++;

                   else if(Board.Grid[row, i].OccupantStack == null && Board.Grid[row, i].OccupantStack != this)      
                        continue;

                }

            target = Board.Grid[row, column - availableTiles];

        }

        else if (direction == InputManager.DirectionType.Right)
        { 
                for (int i = column; i < TotalColumn; i++)
                {
                    if (Board.Grid[row, i].Type == FloorElement.ElementType.Block)
                    {
                        
                    break;
                    }

                    if(Board.Grid[row, i].OccupantStack == null)
                    availableTiles++;

                    else if (Board.Grid[row, i].OccupantStack != null && Board.Grid[row, i].OccupantStack != this)
                        continue;
                }

                target = Board.Grid[row, column+availableTiles];

        }

        else if (direction == InputManager.DirectionType.Upwards)
        { 
               for (int i = row; i >= 0; i--)
               {
                    if (Board.Grid[i, column].Type == FloorElement.ElementType.Block)
                    {   
                        break;
                    }

                    if(Board.Grid[i, column].OccupantStack == null)
                        availableTiles++;

                    else if (Board.Grid[i, column].OccupantStack != null && Board.Grid[i, column].OccupantStack != this)
                        continue;
                }

               target = Board.Grid[row - availableTiles, column];

        }

       else if (direction == InputManager.DirectionType.Downwards)
        { 
        
                for (int i = row; i < TotalRow; i++)
                {
                     if (Board.Grid[i, column].Type == FloorElement.ElementType.Block)
                    {
                    
                        break;
                    }

                    if (Board.Grid[i, column].OccupantStack == null)
                        availableTiles++;
                

                    else if (Board.Grid[i, column].OccupantStack != null && Board.Grid[i, column].OccupantStack != this)
                        continue;
            }

                target = Board.Grid[row+availableTiles, column];
        }


        Move(current, target);
       
    }

    private void Move(FloorElement current, FloorElement target)
    {
        Slide(target.transform.position, current, target);
    }

    void Slide(Vector3 pos, FloorElement current, FloorElement target)
    {
        
        transform.DOMove(pos, 0.2f).SetEase(Ease.OutSine).OnComplete(() => 
        {
        });

            current.OccupantStack = null;
            Board.StackMap[current.Row, current.Column] = null;
            target.OccupantStack = this;
            Board.StackMap[target.Row, target.Column] = this;
            this.Row = target.Row;
            this.Column = target.Column;
            CheckJoin(target.Row, target.Column);   
       
    }

    void CheckJoin(int row, int column)
    {
        switch (InputManager.Direction)
        {
            case InputManager.DirectionType.Left:
                if (column > 0)
                {
                    if (Board.Grid[row, column - 1].Type != FloorElement.ElementType.Flat)
                        return;

                    Stack neighbour = Board.Grid[row, column - 1].OccupantStack;

                    if (neighbour.Value == Value)
                    {
                        Join(neighbour, row, column);
                    }
                }
                break;
            case InputManager.DirectionType.Right:
                if (column < TotalColumn - 1)
                {
                    if (Board.Grid[row, column + 1].Type != FloorElement.ElementType.Flat)
                        return;

                    Stack neighbour = Board.Grid[row, column + 1].OccupantStack;
                    if (neighbour.Value == Value)
                    {
                        Join(neighbour, row, column);
                    }
                }

                break;
            case InputManager.DirectionType.Upwards:
                if (row > 0)
                {
                    if (Board.Grid[row - 1, column].Type != FloorElement.ElementType.Flat)
                        return;


                    Stack neighbour = Board.Grid[row-1, column].OccupantStack;
                    if (neighbour.Value == Value)
                    {
                        Join(neighbour, row, column);
                    }
                }
                break;
            case InputManager.DirectionType.Downwards:
                if (row < TotalRow - 1)
                {
                    if (Board.Grid[row + 1, column].Type != FloorElement.ElementType.Flat)
                        return;


                    Stack neighbour = Board.Grid[row + 1, column].OccupantStack;
                    if (neighbour.Value == Value)
                    {
                        Join(neighbour, row, column);
                    }
                }
                break;

        }
  
    }

    void Join(Stack neigbour, int row, int col)
    {
        if (!neigbour.isEngaged)
        {
            isEngaged = true;
            neigbour.Merge(plates);
            Board.Grid[row, col].OccupantStack = null;
            Board.StackMap[row, col] = null;
            Destroy(valIndicator);
            Destroy(gameObject);
        }
    }

    void Merge(List<GameObject> _plates)
    {
        Value *= 2;
        foreach (var e in _plates)
        {
            plates.Insert(0,e);
            e.transform.SetParent(this.transform);
            e.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 0.2f);
        }

        foreach (var e in plates)
        {
            e.transform.DOMoveY(plates.IndexOf(e) * 0.15f, 1f).SetEase(Ease.InOutElastic);
        }

        UpdateValue();
        UpdateColors();

        CheckDirection(InputManager.Direction, Row, Column);
        EventManager.Instance.StacksMerged(Value, new Vector3(transform.position.x, plates.Count*0.15f, transform.position.z));

        

    }
}
