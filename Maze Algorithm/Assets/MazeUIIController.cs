using UnityEngine;
using UnityEngine.UI;

public class MazeUIController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public InputField widthInput;
    public InputField heightInput;
    public InputField waitTimeInput;

    private int tempWidth;
    private int tempHeight;
    private float tempWaitTime;

    void Start()
    {
        tempWidth = mazeGenerator.mazeWidth;
        tempHeight = mazeGenerator.mazeHeight;
        tempWaitTime = mazeGenerator.waitTime;

        widthInput.text = tempWidth.ToString();
        heightInput.text = tempHeight.ToString();
        waitTimeInput.text = tempWaitTime.ToString("F2");

        widthInput.onEndEdit.AddListener(OnWidthChanged);
        heightInput.onEndEdit.AddListener(OnHeightChanged);
        waitTimeInput.onEndEdit.AddListener(OnWaitTimeChanged);
    }

    public void OnWidthChanged(string value)
    {
        if (int.TryParse(value, out int width) && width > 0)
            tempWidth = width;
    }

    public void OnHeightChanged(string value)
    {
        if (int.TryParse(value, out int height) && height > 0)
            tempHeight = height;
    }

    public void OnWaitTimeChanged(string value)
    {
        if (float.TryParse(value, out float waitTime) && waitTime >= 0f)
            tempWaitTime = waitTime;
    }


    public void ApplyMazeSettings() // apply new maze settings when Generate button is pressed
    {
        mazeGenerator.mazeWidth = tempWidth;
        mazeGenerator.mazeHeight = tempHeight;
        mazeGenerator.waitTime = tempWaitTime;
        mazeGenerator.GenerateMaze();
    }
}