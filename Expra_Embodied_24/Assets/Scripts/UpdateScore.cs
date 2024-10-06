
using DualSenseUnity;
using TMPro;
using UnityEngine;
using UXF;
public class UpdateScore : MonoBehaviour
{
    public GameObject cursor;
    public CursorControls cursorControls;
    public DualSenseController dsc;
    public CursorClass cursorClass;
    public Session session;
    public TextMeshPro scoreTMP;
    public int scorePrev;
    public int currentScoring;
    public int trainingTrial;
    public int tempScore;
    public string controllerSteadyMessage;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        cursorClass = cursor.GetComponent<CursorClass>();
        cursorControls = GameObject.Find("Controller").GetComponent<CursorControls>();
        scorePrev = 0;
        currentScoring = 0;
        scoreTMP = gameObject.GetComponent<TextMeshPro>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (session.hasInitialised){
            if (session.InTrial)
            {
                trainingTrial = session.CurrentTrial.settings.GetInt("training");
                if ((trainingTrial == 1))
                {
                    scorePrev = cursorClass.trainingScore;
                    scoreTMP.alpha = 0;
                }
                else
                {
                    scorePrev = cursorClass.blockScore;
                    scoreTMP.alpha = 0;
                }
            }

            else
            {
                switch (cursorControls.controllerSteady)
                {
                    case true:
                        controllerSteadyMessage = "<color=green>Controller gerade</color>";
                        break;
                    case false:
                        controllerSteadyMessage = "<color=red>Controller schief</color>";
                        break;
                }
                if ((trainingTrial == 1))
                {
                    currentScoring = cursorClass.trainingScore - scorePrev;
                    tempScore = cursorClass.trainingScore;
                }

                
                else
                {
                    //Debug.Log(cursorClass.blockScore);
                    currentScoring = cursorClass.blockScore - scorePrev;
                    tempScore = cursorClass.blockScore;
                }



                if (cursorClass.errorPrevious == "None" & !cursorClass.newBlock)
                {
                    scoreTMP.text = "<color=green>" + currentScoring + " Punkte\neingesammelt!" + "\n\nScore:\n" + tempScore + "</color>\n" + controllerSteadyMessage;
                    scoreTMP.alpha = 1;
                }

                else if (cursorClass.errorPrevious != "None" & !cursorClass.newBlock)
                {
                    scoreTMP.text = "<color=red>" + cursorClass.errorPrevious + "</color>" + "\nScore:\n" + tempScore + "\n\n" + controllerSteadyMessage;
                    scoreTMP.alpha = 1;

                }

                else if (!cursorClass.displayingInstructions)
                {
                    scoreTMP.text = "Score:\n" + tempScore + "\n\n" + controllerSteadyMessage;
                    scoreTMP.alpha = 1;
                    
                }



            }
        }
    }
}
