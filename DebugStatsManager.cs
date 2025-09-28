using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStatsManager : MonoBehaviour
{
    private int framesPerSecond;

    [SerializeField]
    private Text fpsDisplay, cpuDisplay, gpuDisplay;

    // Start is called before the first frame update
    void Start()
    {
        framesPerSecond = 0;
        cpuDisplay.text = "CPU: " + SystemInfo.processorType;
        gpuDisplay.text = "GPU: " + SystemInfo.graphicsDeviceName;
    }

    // Update is called once per frame
    void Update()
    {
        framesPerSecond = (int)(1 / Time.unscaledDeltaTime);
        fpsDisplay.text = "FPS: " + framesPerSecond;
    }
}
