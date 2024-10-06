//using UnityEngine;
//using UXF;

//public class RedFrameCameraBorder : MonoBehaviour
//{
//    public Color frameColor = Color.red;
//    public float frameThickness = 6f;
//    public bool showFrame = true; // Set this to false if you don't want to show the frame

//    private Texture2D redCircleTexture;
//    private string text = "REC";

//    public Session session;

//    private CursorClass cursorClass;
//    private void Awake()
//    {
//        // Create a simple white circular texture
//        int textureSize = 64;
//        redCircleTexture = new Texture2D(textureSize, textureSize);
//        for (int y = 0; y < textureSize; y++)
//        {
//            for (int x = 0; x < textureSize; x++)
//            {
//                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(textureSize / 2f, textureSize / 2f));
//                if (dist < textureSize / 2f)
//                    redCircleTexture.SetPixel(x, y, Color.white);
//                else
//                    redCircleTexture.SetPixel(x, y, Color.clear);
//            }
//        }
//        redCircleTexture.Apply();
//        cursorClass = GameObject.Find("Cursor").GetComponent<CursorClass>();
//    }

//    private void OnGUI()
//    {
//        //Debug.Log("session active and enabled: " + session.blocks.Count);
//        if (!cursorClass.displayingInstructions && cursorClass.firstTrialStarted)
//        {
//            showFrame = cursorClass.interventionPhase ? true : false;
//            if (showFrame)
//            {
//                // Get the camera's viewport rect
//                Rect cameraRect = Camera.main.pixelRect;

//                // Draw the top frame
//                GUI.color = frameColor;
//                GUI.DrawTexture(new Rect(cameraRect.xMin, cameraRect.yMin, cameraRect.width, frameThickness), Texture2D.whiteTexture);

//                // Draw the bottom frame
//                GUI.DrawTexture(new Rect(cameraRect.xMin, cameraRect.yMax - frameThickness, cameraRect.width, frameThickness), Texture2D.whiteTexture);

//                // Draw the left frame
//                GUI.DrawTexture(new Rect(cameraRect.xMin, cameraRect.yMin, frameThickness, cameraRect.height), Texture2D.whiteTexture);

//                // Draw the right frame
//                GUI.DrawTexture(new Rect(cameraRect.xMax - frameThickness, cameraRect.yMin, frameThickness, cameraRect.height), Texture2D.whiteTexture);

//                 // Draw REC symbol and dot for monitoring pressure group
//                if (session.participantDetails["treatment"].ToString() == "2")
//                {
//                    // Draw the red circle
//                    GUI.color = Color.red;
//                    float dotSize = 32f;
//                    GUI.DrawTexture(new Rect(cameraRect.xMin + frameThickness + 30, cameraRect.yMin + frameThickness + 30, dotSize, dotSize), redCircleTexture);


//                    GUIStyle style = new GUIStyle(GUI.skin.label);
//                    style.normal.textColor = Color.white;
//                    style.fontSize = 32;
//                    float textOffsetX = 50f;
//                    float textOffsetY = 25f;
//                    GUI.Label(new Rect(cameraRect.xMin + frameThickness + dotSize + textOffsetX, cameraRect.yMin + frameThickness + textOffsetY, 100f, 50f), text, style);

//                }
//                else if (session.participantDetails["treatment"].ToString() == "1")
//                {
//                    // Draw the red circle
//                    GUI.color = Color.red;
//                    float dotSize = 0f;
//                    GUI.DrawTexture(new Rect(cameraRect.xMin + frameThickness + 30, cameraRect.yMin + frameThickness + 30, dotSize, dotSize), redCircleTexture);

//                    text = "";
//                    GUIStyle style = new GUIStyle(GUI.skin.label);
//                    style.normal.textColor = Color.white;
//                    style.fontSize = 0;
//                    float textOffsetX = 50f;
//                    float textOffsetY = 25f;
//                    GUI.Label(new Rect(cameraRect.xMin + frameThickness + dotSize + textOffsetX, cameraRect.yMin + frameThickness + textOffsetY, 100f, 50f), text, style);


//                }
//            }
//        }
//    }
//}

using UnityEngine;
using UXF;
using UnityEngine.UI;
using TMPro;

public class RedFrameCameraBorder : MonoBehaviour
{

    public Image frameImageTop;
    public Image frameImageBottom;
    public Image frameImageLeft;
    public Image frameImageRight;
    public Image recSymbolImage;
    public TextMeshProUGUI recText;

    public Session session;

    private CursorClass cursorClass;

    private Color displayColor = new Color(255, 0, 0, 0);

    private void Awake()
    {
        cursorClass = GameObject.Find("Cursor").GetComponent<CursorClass>();
    }

    private void Update()
    {
        if (!cursorClass.displayingInstructions && cursorClass.firstTrialStarted)
        {
            bool showFrame = cursorClass.interventionPhase;

            //frameImageTop.enabled = showFrame;
            //frameImageBottom.enabled = showFrame;
            //frameImageLeft.enabled = showFrame;
            //frameImageRight.enabled = showFrame;
            
            if (showFrame)
            {
                displayColor = new Color(255, 0, 0, 255);
            }
            else
            {
                displayColor = new Color(255, 0, 0, 0);
            }

            frameImageTop.color = displayColor;
            frameImageBottom.color = displayColor;
            frameImageLeft.color = displayColor;
            frameImageRight.color = displayColor;

            //recSymbolImage.enabled = (session.participantDetails["treatment"].ToString() == "2" && showFrame);
            //recText.enabled = (session.participantDetails["treatment"].ToString() == "2" && showFrame);



            recSymbolImage.color = (session.participantDetails["treatment"].ToString() == "2" && showFrame) ? new Color(255, 0, 0, 255) : new Color(255, 0, 0, 0);
            recText.color = (session.participantDetails["treatment"].ToString() == "2" && showFrame) ? new Color(255, 0, 0, 255) : new Color(255, 0, 0, 0); ;
        }
    }
}

