using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPiece : MonoBehaviour
{
   public bool isBlock;

    private void OnMouseDown()
    {
        isBlock = !isBlock;
        SwapMaterial();
    }

    void SwapMaterial()
    {
        if (isBlock)
        {
            GetComponent<MeshRenderer>().material = LevelEditor.Instance.blockedMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = LevelEditor.Instance.defaultMaterial;
        }
    }
}
