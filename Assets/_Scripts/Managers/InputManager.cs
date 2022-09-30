using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    Vector2 initialMousePos;

    public enum DirectionType
    { 
        Left,
        Right,
        Upwards,
        Downwards
    }

    public static DirectionType Direction;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

     void Update()
    {

        if (GameManager.Instance.State != GameManager.GameState.Play)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            initialMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 lastMousePos = Input.mousePosition;
            GetDirection(lastMousePos);
            initialMousePos = Vector2.zero;
        }
    }

    void GetDirection(Vector2 lastMousePos)
    {
        float DeltaX = initialMousePos.x - lastMousePos.x;
        float DeltaY = initialMousePos.y - lastMousePos.y;

        if (Mathf.Abs(DeltaX) > Mathf.Abs(DeltaY))
        {
            if (DeltaX < 0)
            {
                Direction = DirectionType.Right;
            }
            else
            {
                Direction = DirectionType.Left;
            }
        }
        else
        {
            if (DeltaY < 0)
            {
                Direction = DirectionType.Upwards;
            }
            else
            {
                Direction = DirectionType.Downwards;
            }
        }

        EventManager.Instance.DirectionSent(Direction);
    }
}
