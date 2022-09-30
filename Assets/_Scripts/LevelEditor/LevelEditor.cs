using TMPro;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class LevelEditor : MonoBehaviour
{
    public static LevelEditor Instance;
    [SerializeField] GameObject defaultFloor;
    [SerializeField] TMP_InputField levelInput;
    [SerializeField] TMP_InputField targetValueInput;
    public Material defaultMaterial;
    public Material blockedMaterial;
    const int RowCount = 7;
    const int ColumnCount = 7;
    FloorPiece[,] grid;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    void Start()
    {
        CreateLevel();

        Cam cam = Camera.main.transform.gameObject.GetComponent<Cam>(); 
        cam.SetCamera(RowCount, ColumnCount);
    }

    void CreateLevel()
    {
        grid = new FloorPiece[ColumnCount, RowCount];
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                GameObject obj = Instantiate(defaultFloor, transform);
                obj.transform.position = new Vector3(j, 0, -i);
                grid[i, j] = obj.GetComponent<FloorPiece>();

            }
        }

        levelInput.text = 1.ToString();
        targetValueInput.text = 32.ToString();
    }

    public void SaveLevel()
    {
        List<FloorTile> tiles = new List<FloorTile>();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                tiles.Add(new FloorTile(grid[i, j].isBlock, j,i));
               
            }
        }

        int target = System.Convert.ToInt32(targetValueInput.text);
        Level level = new Level(tiles,ColumnCount, RowCount, target);
        int levelIndex = System.Convert.ToInt32(levelInput.text);

        string lvl = JsonUtility.ToJson(level);
        string path = $"{Application.dataPath}/StreamingAssets/Levels/{levelIndex.ToString()}.json";
         File.WriteAllText(path, lvl);

        AssetDatabase.Refresh();
    }
}
