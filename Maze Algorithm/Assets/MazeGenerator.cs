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