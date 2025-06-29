using UnityEngine;

public class MazeCameraController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public float cellSize = 1f; // Set to your cell prefab's size (default 1)
    public float padding = 1f;  // Extra space around the maze
    [SerializeField] private float zoomOutValue;
    [SerializeField] private float zoomInValue;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCameraToMaze();
    }

    // Call this method after generating the maze
    public void UpdateCameraToMaze()
    {
        if (mazeGenerator == null || cam == null) return;

        // Calculate maze bounds
        float mazeWidthWorld = mazeGenerator.mazeWidth * cellSize;
        float mazeHeightWorld = mazeGenerator.mazeHeight * cellSize;

        // Center position
        float centerX = (mazeGenerator.mazeWidth - 1) * cellSize / 2f;
        float centerY = (mazeGenerator.mazeHeight - 1) * cellSize / 2f;
        Vector3 center = new Vector3(centerX, centerY, -10f); // -10 for default camera z

        // Set camera position
        cam.transform.position = center;

        // Calculate orthographic size
        float aspect = cam.aspect;
        float sizeY = (mazeHeightWorld / 2f) + padding;
        float sizeX = ((mazeWidthWorld / 2f) + padding) / aspect;
        cam.orthographicSize = Mathf.Max(sizeY, sizeX);
    }
    public void ZoomOutCamera()
    {
        if (cam == null) return;
        cam.orthographicSize *= zoomOutValue;
    }
    public void ZoomInCamera()
    {
        if (cam == null) return;
        cam.orthographicSize /= zoomInValue;
    }
}