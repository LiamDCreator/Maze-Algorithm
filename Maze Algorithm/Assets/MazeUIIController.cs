using UnityEngine;
using UnityEngine.UI;

public class MazeUIController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public InputField widthInput;
    public InputField heightInput;
    public InputField waitTimeInput;

    void Start()
    {
        // Initialize UI with current values
        widthInput.text = mazeGenerator.mazeWidth.ToString();
        heightInput.text = mazeGenerator.mazeHeight.ToString();
        waitTimeInput.text = mazeGenerator.waitTime.ToString("F2");

        // Add listeners
        widthInput.onEndEdit.AddListener(OnWidthChanged);
        heightInput.onEndEdit.AddListener(OnHeightChanged);
        waitTimeInput.onEndEdit.AddListener(OnWaitTimeChanged);
    }

    public void OnWidthChanged(string value)
    {
        if (int.TryParse(value, out int width) && width > 0)
            mazeGenerator.mazeWidth = width;
    }

    public void OnHeightChanged(string value)
    {
        if (int.TryParse(value, out int height) && height > 0)
            mazeGenerator.mazeHeight = height;
    }

    public void OnWaitTimeChanged(string value)
    {
        if (float.TryParse(value, out float waitTime) && waitTime >= 0f)
        {
            mazeGenerator.waitTime = waitTime;
        }
    }
}