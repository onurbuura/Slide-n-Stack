using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorElement : MonoBehaviour
{
    public enum ElementType
    { 
      Flat,
      Block
    }

    [SerializeField] GameObject ValIndicatorPrefab;
   // GameObject valIndicator;

    public ElementType Type;
    public Stack OccupantStack;

    public int Row;
    public int Column;

    private void Start()
    {
        //Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        //valIndicator = Instantiate(ValIndicatorPrefab, canvas);

        //Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //valIndicator.transform.position = screenPoint;

        //string tx = $"({Row},{Column})";
        //valIndicator.GetComponent<TMP_Text>().text = tx;
    }
}
