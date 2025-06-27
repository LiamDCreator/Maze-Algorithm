using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _mazeCellPrefab;

    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    private GameObject[,] mazeGrid; // Changed from MazeCell to GameObject

    void Start()
    {
        mazeGrid = new GameObject[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeGrid[x, y] = Instantiate(_mazeCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {

    }
}