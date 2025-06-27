using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject mazeSquare;

    public bool IsVisited { get; private set; }
    public void Visit()
    {
        IsVisited = true;
        mazeSquare.SetActive(false);
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    public void ClearTopWall()
    {
        topWall.SetActive(false);
    }

    public void ClearBottomWall()
    {
        bottomWall.SetActive(false);
    }
}
