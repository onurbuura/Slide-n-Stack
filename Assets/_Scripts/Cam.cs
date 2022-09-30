using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] bool DynamicCamera;
    public void SetCamera(Level level)
    {
        if (DynamicCamera)
        { 
            transform.position = new Vector3((level.RowCount - 1) / 2, level.ColumnCount * 2, (level.ColumnCount - 1) / -2);
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }
    }

    public void SetCamera(int RowCount, int ColumnCount)
    {
        if (DynamicCamera)
        { 
            transform.position = new Vector3((RowCount - 1) / 2, ColumnCount * 2, (ColumnCount - 1) / -2);
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }
    }
}
