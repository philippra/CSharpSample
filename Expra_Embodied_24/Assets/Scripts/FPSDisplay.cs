using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshPro FpsText;
    private float pollingTime = 0.25f;
    private float time;
    private int frameCount;

    void LateUpdate()
    {
        time += Time.deltaTime;

        frameCount++;

        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS" + "\n";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
