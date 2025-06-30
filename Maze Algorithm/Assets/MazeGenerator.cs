using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    public int mazeWidth;
    public int mazeHeight;
    public float waitTime;

    private MazeCell[,] mazeGrid;
    private Coroutine mazeCoroutine;

    [Header("Player Spawning")]
    public GameObject playerPrefab; // Assign your player prefab in the Inspector
    private GameObject playerInstance;

    void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // If a maze is already being generated, stop it
        if (mazeCoroutine != null)
        {
            StopCoroutine(mazeCoroutine);
        }

        // Destroy old maze cells if they exist
        if (mazeGrid != null)
        {
            foreach (var cell in mazeGrid)
            {
                if (cell != null)
                    Destroy(cell.gameObject);
            }
        }

        // Destroy old player if it exists
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        mazeGrid = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeGrid[x, y] = Instantiate(_mazeCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        mazeCoroutine = StartCoroutine(GenerateMazeIterative(mazeGrid[0, 0]));
    }

    private IEnumerator GenerateMazeIterative(MazeCell startCell)
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell = stack.Peek();
            if (!currentCell.IsVisited)
            {
                currentCell.Visit();
            }

            var unvisited = GetUnvisitedCells(currentCell).OrderBy(_ => Random.value).ToList();
            if (unvisited.Count > 0)
            {
                MazeCell nextCell = unvisited.First();
                ClearWalls(currentCell, nextCell);
                stack.Push(nextCell);
            }
            else
            {
                stack.Pop();
            }

            yield return new WaitForSeconds(waitTime);
        }

        // Open two random sides after maze generation
        List<int> sides = new List<int> { 0, 1, 2, 3 }; // 0=left, 1=right, 2=bottom, 3=top
        int firstSide = sides[Random.Range(0, sides.Count)];
        sides.Remove(firstSide);
        int secondSide = sides[Random.Range(0, sides.Count)];

        int entranceX = 0, entranceY = 0; // Defaults in case needed

        void OpenRandomSide(int side, out int cellX, out int cellY)
        {
            cellX = 0;
            cellY = 0;
            switch (side)
            {
                case 0: // Left
                    cellY = Random.Range(0, mazeHeight);
                    cellX = 0;
                    mazeGrid[cellX, cellY].ClearLeftWall();
                    break;
                case 1: // Right
                    cellY = Random.Range(0, mazeHeight);
                    cellX = mazeWidth - 1;
                    mazeGrid[cellX, cellY].ClearRightWall();
                    break;
                case 2: // Bottom
                    cellX = Random.Range(0, mazeWidth);
                    cellY = 0;
                    mazeGrid[cellX, cellY].ClearBottomWall();
                    break;
                case 3: // Top
                    cellX = Random.Range(0, mazeWidth);
                    cellY = mazeHeight - 1;
                    mazeGrid[cellX, cellY].ClearTopWall();
                    break;
            }
        }

        // Open entrance and store its position
        OpenRandomSide(firstSide, out entranceX, out entranceY);
        // Open exit (no need to store position)
        int dummyX, dummyY;
        OpenRandomSide(secondSide, out dummyX, out dummyY);

        // Spawn player at entrance
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, new Vector3(entranceX, entranceY, 0), Quaternion.identity);
        }
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int y = (int)currentCell.transform.position.y;

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, y];
            if (!cellToRight.IsVisited)
            {
                yield return cellToRight;
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, y];
            if (!cellToLeft.IsVisited)
            {
                yield return cellToLeft;
            }
        }
        if (y + 1 < mazeHeight)
        {
            var cellToTop = mazeGrid[x, y + 1];
            if (!cellToTop.IsVisited)
            {
                yield return cellToTop;
            }
        }
        if (y - 1 >= 0)
        {
            var cellToBottom = mazeGrid[x, y - 1];
            if (!cellToBottom.IsVisited)
            {
                yield return cellToBottom;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (previousCell.transform.position.y < currentCell.transform.position.y)
        {
            previousCell.ClearTopWall();
            currentCell.ClearBottomWall();
            return;
        }
        if (previousCell.transform.position.y > currentCell.transform.position.y)
        {
            previousCell.ClearBottomWall();
            currentCell.ClearTopWall();
            return;
        }
    }
}