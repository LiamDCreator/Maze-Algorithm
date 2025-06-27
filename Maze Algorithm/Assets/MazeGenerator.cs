using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    private MazeCell[,] mazeGrid;

    IEnumerator Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeGrid[x, y] = Instantiate(_mazeCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
        yield return GenerateMaze(null, mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);
        yield return new WaitForSeconds(0.05f);
        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int y = (int)currentCell.transform.position.y;

        if (x + 1 < mazeWidth)
        {
            var celltoRight = mazeGrid[x + 1, y];
            if (celltoRight.IsVisited == false)
            {
                yield return celltoRight;
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, y];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }
        if (y + 1 < mazeHeight)
        {
            var cellToTop = mazeGrid[x, y + 1];
            if (cellToTop.IsVisited == false)
            {
                yield return cellToTop;
            }
        }
        if (y - 1 >= 0)
        {
            var cellToBottom = mazeGrid[x, y - 1];
            if (cellToBottom.IsVisited == false)
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