using System.Linq;
using UnityEngine;
using UXF;

public class MainScript : MonoBehaviour
{

    private GameObject cursor;

    private Transform cursorTransform;

    private Transform leftLaneTransform;
    private Transform rightLaneTransform;

    public Session session;

    private GameObject middleLane;
    private GameObject rightLane;
    private GameObject leftLane;
    private GameObject tempCurrentLane;

    private Transform tempCurrentLaneTransform;

    private MeshRenderer leftRewardRenderer;
    private MeshRenderer rightRewardRenderer;

    private MoveObjects objectMover;

    private CursorClass cursorClass;

    private CursorControls CursorControls;

    private float pert_strength = 0.070f;

    private DisplayInstructions instruction;

      private void Awake()
    {

        CursorControls = GameObject.Find("Controller").GetComponent<CursorControls>();
        
        cursor = GameObject.Find("Cursor");

        cursorTransform = cursor.transform;

        cursorClass = cursor.GetComponent<CursorClass>();

        middleLane = GameObject.Find("MiddleLane");

        rightLane = GameObject.Find("RightLane");

        leftLane = GameObject.Find("LeftLane");

        tempCurrentLane = GameObject.Find("MiddleLane");

        tempCurrentLaneTransform = tempCurrentLane.transform;

        leftLaneTransform = leftLane.transform;

        rightLaneTransform = rightLane.transform;

        leftRewardRenderer = leftLane.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();

        rightRewardRenderer = rightLane.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();

        objectMover = middleLane.GetComponent<MoveObjects>();

        instruction = GameObject.Find("Instruction").GetComponent<DisplayInstructions>();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

        float _time = Time.time;
        var _inputStateController = CursorControls.dsc.GetInputState();
        
        float _gyroState = (float)CursorControls.get_gyro_state(_inputStateController);

        float _decisionStickNow = (float)CursorControls.decisionStickNow;

        if (!cursorClass.postExperiment && !cursorClass.displayingInstructions)
        {
            

            Cursor.visible = false;
            switch (session.InTrial)
            {
                case true:

                    if (!CursorControls.paramsSet)
                        CursorControls.Set_Start_Parameters();

                    var _timeSinceTrialStart = _time - (float)session.CurrentTrial.result["start_time"];

                    cursorClass.firstTrialStarted = true;
                    
                    if (!leftRewardRenderer.enabled && _timeSinceTrialStart >= 2.00f)
                    {
                        leftRewardRenderer.enabled = true;
                        rightRewardRenderer.enabled = true;
                        session.CurrentTrial.result["rewardTiming"] = Time.time;

                    }
                    //tempCurrentLane = GameObject.Find(cursorClass.currentLane);

                    switch (cursorClass.currentLane)
                    {
                        case "MiddleLane":
                            tempCurrentLane = middleLane;
                            break;
                        case "LeftLane":
                            tempCurrentLane = leftLane;
                            break;
                        case "RightLane":
                            tempCurrentLane = rightLane;
                            break;
                    }
                    tempCurrentLaneTransform = tempCurrentLane.transform;

                    if (cursorClass.trialEndChecker(_timeSinceTrialStart, cursorClass, tempCurrentLaneTransform, cursorTransform, session, CursorControls))
                        break;
      

                    cursorClass.distanceFromCenterNow = (cursorTransform.position.x - tempCurrentLaneTransform.position.x);
                    
                    //objectMover.Move_and_Perturb(cursorTransform, cursorClass, pert_strength, tempCurrentLaneTransform);

                    objectMover.MoveForward(cursorTransform, cursorClass);

                    pert_strength = session.CurrentTrial.settings.GetInt("training") == 1 ? 0.060f : 0.070f;

                    objectMover.PerturbCursor(cursorTransform, cursorClass, pert_strength);

                    float trackingDiff = -CursorControls.ComputeTrackingDiff(cursorClass.currentPertDirect, _inputStateController, cursorClass);

                    objectMover.TrackingMovement(trackingDiff, cursorTransform, cursorClass);

                    if (!cursorClass.jumped && _timeSinceTrialStart > 2.25f)
                    {
                        objectMover.Jump(cursorTransform, cursorClass, leftLaneTransform, rightLaneTransform, cursorClass.distanceFromCenterNow, CursorControls, _timeSinceTrialStart);

                    }
                    else if (!cursorClass.jumped && _timeSinceTrialStart <= 2.25f && _timeSinceTrialStart >= 0.75)
                    {
                        //Debug.Log("Premature Movement tracked");
                        cursorClass.Controller_Moved(cursorClass, _gyroState, _decisionStickNow, cursorTransform, _timeSinceTrialStart, CursorControls);
                        break;
                    }

                    cursorClass.distanceFromCenterPrev = cursorTransform.position.x - tempCurrentLaneTransform.position.x;
                    break;
                case false:

                    //Debug.Log(cursorClass.stillStart);

                    if (!(cursorClass.displayingInstructions) && (cursorClass.stillStart == 99999.0f || Mathf.Abs(CursorControls.get_gyro_state(_inputStateController)) > 15))
                    {
                        cursorClass.stillStart = _time;
                        if (Mathf.Abs(CursorControls.get_gyro_state(_inputStateController)) > 15)
                            CursorControls.controllerSteady = false;
                        CursorControls.RefreshControllers();
                        CursorControls.paramsSet = false;

                    }

                    else if (!(Mathf.Abs(CursorControls.get_gyro_state(_inputStateController)) > 15))
                        CursorControls.controllerSteady = true;


                    if (CursorControls.HeldStill(cursorClass.stillStart, cursorClass.errorTrialDuration, cursorClass) && !(Mathf.Abs(_gyroState) > 15))
                    {
                        cursorClass.experimentActive = true;
                        CursorControls.controllerSteady = true;
                        cursorClass.stillStart = 99999.0f;
                    }

                    leftRewardRenderer.enabled = false;
                    rightRewardRenderer.enabled = false;

                    CursorControls.leftStickPrev = (float)CursorControls.dsc.GetInputState().LeftStick.XAxis;
                    CursorControls.rightStickPrev = (float)CursorControls.dsc.GetInputState().RightStick.XAxis;
                    break;



            }
        }

        else if (cursorClass.postExperiment)
        {
            var _postExperimentQuest = GameObject.Find("PostExperimentQuestionnaire").GetComponent<PostExperimentQuestionnaire>();
            _postExperimentQuest.runQuestionnaire();
            if (_postExperimentQuest.questionCounter > _postExperimentQuest.questionnaire.Length - 1)
            {
                session.End();
                Application.Quit();
            }

        }
        else
        {
            instruction.Display_Instructions();
        }

       
    }

}

