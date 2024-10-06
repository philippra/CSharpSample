using System.Linq;
using UnityEngine;
using UXF;

public class CursorClass : MonoBehaviour
{
    public Session session;
    public GameObject cursor;

    public int score;
    public int blockScore;
    public int trainingScore;

    public bool touchesLane;

    public string nameTag;
    public string currentLane;
    public string errorPrevious;

    public bool cc;
    public bool jumped;

    public float jumpTime;

    public bool rewardCollected;
    public bool obstacleCollision;
    public bool fellFromLane;

    public float distanceFromCenterNow;
    public float distanceFromCenterPrev;
    public float stillStart;
    public float errorTrialDuration;

    public bool experimentActive;
    public bool displayingInstructions;
    public bool displayingConditionsInstructions; 
   
    public bool newBlock;
    public bool preBlockThumbs = true;
    public int currentBlock;
    public bool postExperiment;

    public bool newTrial = true;

    public bool firstTrialStarted = false;

    public bool interventionPhase = false;

    public int currentPertDirect;

    public float[] gyroHistory;
    public int gyroHistoryCounter;

    public float forwardVelocity;

    public int thumbREMapping = 1;

    private void Awake()
    {
        cursor = GameObject.Find("Cursor");
        nameTag = "Cursor";
        currentLane = "MiddleLane";
        errorPrevious = "None";
        errorTrialDuration = 5.0f;
        cc = GetComponent<CharacterController>();
        score = 0;
        blockScore = 0;
        trainingScore = 0;
        touchesLane = true;
        jumped = false;
        jumpTime = 1000000.0f;
        rewardCollected = false;
        obstacleCollision = false;
        fellFromLane = false;
        postExperiment = false;
        displayingInstructions = true;
        currentBlock = 1;

        currentPertDirect = 0;

        newBlock = true;

        distanceFromCenterNow = 0.0f;
        distanceFromCenterPrev = 0.0f;

        experimentActive = false;
        stillStart = 99999.0f;

        gyroHistory = new float[6];
        gyroHistoryCounter = 0;

    }
       
    public bool trialEndChecker(float trialTime, CursorClass cursorClass, Transform cursorTransform, Transform currentLaneTransform, Session session, CursorControls cursorControls)
    {
        if (trialTime >= 3.25f && cursorClass.currentLane == "MiddleLane")
        {
            cursorClass.currentBlock = session.CurrentBlock.number;
            Debug.Log("Collided with Obstacle.");
            Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
            cursorClass.obstacleCollision = true;
         
            
            cursorClass.errorPrevious = "<b>Gefressen!</b>\n0 Punkte";
            cursorClass.session.CurrentTrial.result["switchDirection"] = "collision_" + cursorClass.currentLane;
            triggerError(cursorClass, trialTime, cursorControls);
            return true;
        }

        else if (trialTime >= 4.00f && cursorClass.jumped)
        {
            cursorClass.currentBlock = session.CurrentBlock.number;
            Debug.Log("Reward on " + cursorClass.currentLane + " collected!");
            Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
            cursorClass.rewardCollected = true;
            errorPrevious = "None";

            switch (cursorClass.currentLane)
            {
                case "RightLane":

                    if ((session.CurrentTrial.settings.GetInt("training") == 1))
                        cursorClass.trainingScore += GameObject.FindGameObjectWithTag("RightReward").GetComponent<RewardSize>().rewardSize;
                    else
                    {
                        cursorClass.score += GameObject.FindGameObjectWithTag("RightReward").GetComponent<RewardSize>().rewardSize;
                        cursorClass.blockScore += GameObject.FindGameObjectWithTag("RightReward").GetComponent<RewardSize>().rewardSize;
                    }


                    session.CurrentTrial.result["switchDirection"] = "rightLane";
                    break;

                case "LeftLane":

                    if ((session.CurrentTrial.settings.GetInt("training") == 1))
                        cursorClass.trainingScore += GameObject.FindGameObjectWithTag("LeftReward").GetComponent<RewardSize>().rewardSize;
                    else
                    {
                        cursorClass.blockScore += GameObject.FindGameObjectWithTag("LeftReward").GetComponent<RewardSize>().rewardSize;
                        cursorClass.score += GameObject.FindGameObjectWithTag("LeftReward").GetComponent<RewardSize>().rewardSize;
                    }
                    session.CurrentTrial.result["switchDirection"] = "leftLane";
                    break;
            }
            cursorClass.distanceFromCenterNow = 0.0f;
            cursorClass.distanceFromCenterPrev = 0.0f;
            GameObject.FindGameObjectWithTag("RightReward").transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("LeftReward").transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            if (session.CurrentTrial.numberInBlock == session.CurrentBlock.trials.Count())
            {
                cursorClass.currentBlock++;
                cursorClass.errorPrevious = "None";
                cursorClass.newBlock = true;
                cursorClass.preBlockThumbs = true;
                cursorClass.displayingInstructions = true;

                Debug.Log("cursorClass.currentBlock: " + cursorClass.currentBlock);

                int[] BlocksBeforeConditionsChanges = { 5 };

                cursorClass.displayingConditionsInstructions = BlocksBeforeConditionsChanges.Contains(cursorClass.currentBlock);

                cursorClass.experimentActive = false;

                cursorClass.blockScore = 0;
                cursorClass.trainingScore = 0;

            }
            else if (session.CurrentTrial.numberInBlock == 1)
                cursor.GetComponent<CursorClass>().newBlock = false;

            if (session.currentTrialNum == session.Trials.Count())
                cursorClass.postExperiment = true;
            session.EndCurrentTrial();
            cursorClass.experimentActive = false;
            errorTrialDuration = 4.0f;
            return true;
        }

        else if (cursorTransform.position.x < (currentLaneTransform.position.x - 5.0f) || cursorTransform.position.x > (currentLaneTransform.position.x + 5.0f))
        {
            Debug.Log("Fell from lane!");
            Debug.Log(Time.time);
            cursorClass.fellFromLane = true;
            cursorClass.errorPrevious = "<b>An Land\ngeschwemmt!</b>\n";
            cursorClass.session.CurrentTrial.result["switchDirection"] = "fall_" + cursorClass.currentLane;
            cursorClass.jumped = true;
            cursorClass.errorTrialDuration = Time.time - (float)cursorClass.session.CurrentTrial.result["start_time"];
            cursorClass.triggerError(cursorClass, trialTime, cursorControls);
            return true;
        }
        else
            return false;
    }

    public void Controller_Moved(CursorClass cursorClass, float gyroState, float decisionStickNow, Transform cursorTransform, float trialTime, CursorControls cursorControls)
    {
        var tiltThreshold = 30;
        var decisionStickThreshold = 0.2;

        bool controllerTiltedLeftward = gyroState > tiltThreshold;
        bool controllerTiltedRightward = gyroState < -tiltThreshold;

        bool decisionStickMovedLeftward = decisionStickNow > decisionStickThreshold;
        bool decisionStickMovedRightward = decisionStickNow < -decisionStickThreshold;

        if (controllerTiltedLeftward || decisionStickMovedLeftward)
        {
            cursorClass.gyroHistory[cursorClass.gyroHistoryCounter] = gyroState;

            if (cursorClass.gyroHistory.Average() > tiltThreshold)
            {
                Debug.Log("Controller moved prematurely!");
                Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
                cursorClass.errorPrevious = "<b>Controller zu \nstark gekippt!</b>\n";
                session.CurrentTrial.result["switchDirection"] = "earlyControllerMovement_" + cursorClass.currentLane;

                triggerError(cursorClass, trialTime, cursorControls);
            }

            else if (decisionStickMovedLeftward)
            {
                 Debug.Log("Decision Joystick moved prematurely!");
                Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
                cursorClass.errorPrevious = "<b>Entscheidungs-Joystick\nzu früh bewegt!</b>\n";
                session.CurrentTrial.result["switchDirection"] = "earlyDecisionMovement_" + cursorClass.currentLane;

                triggerError(cursorClass, trialTime, cursorControls);
            }

        }
        else if (controllerTiltedRightward || decisionStickMovedRightward)
        {
            cursorClass.gyroHistory[cursorClass.gyroHistoryCounter] = gyroState;

            if (cursorClass.gyroHistory.Average() < -tiltThreshold )
            {
                Debug.Log("Controller moved prematurely!");
                Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
                cursorClass.errorPrevious = "<b>Controller zu \nstark gekippt!</b>\n"; 
                session.CurrentTrial.result["switchDirection"] = "earlyControllerMovement_" + cursorClass.currentLane;

                triggerError(cursorClass, trialTime, cursorControls);
                //Debug.Log("JUMPED");
            }
            else if (decisionStickMovedRightward)
            {
                 Debug.Log("Decision Joystick moved prematurely!");
                Debug.Log((Time.time - (float)session.CurrentTrial.result["start_time"]));
                cursorClass.errorPrevious = "<b>Entscheidungs-Joystick\nzu früh bewegt!</b>\n";
                session.CurrentTrial.result["switchDirection"] = "earlyDecisionMovement_" + cursorClass.currentLane;

                triggerError(cursorClass, trialTime, cursorControls);
            }

        }

        if (cursorClass.gyroHistoryCounter >= (cursorClass.gyroHistory.Length - 1))
            cursorClass.gyroHistoryCounter = 0;
        else
            cursorClass.gyroHistoryCounter++;
        
    }

    public bool triggerError(CursorClass cursorClass, float trialTime, CursorControls cursorControls)
    {
        Debug.Log("Error triggered");

        session.CurrentTrial.result["errorTime"] = Time.time;

        cursorClass.currentBlock = session.CurrentBlock.number;
        if ((session.CurrentTrial.settings.GetInt("training") == 1))
            cursorClass.trainingScore -= 0;
        else
        {
            cursorClass.score -= 0;
            cursorClass.blockScore -= 0;
        }

        cursorClass.distanceFromCenterNow = 0.0f;
        cursorClass.distanceFromCenterPrev = 0.0f;

        GameObject.FindGameObjectWithTag("RightReward").transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        GameObject.FindGameObjectWithTag("LeftReward").transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        
        if (session.CurrentTrial.numberInBlock == session.CurrentBlock.trials.Count())
        {
            cursorClass.currentBlock++;
            cursorClass.errorPrevious = "None";
            errorTrialDuration = 4.0f;
            cursorClass.newBlock = true;
            cursorClass.displayingInstructions = true;
            cursorClass.errorTrialDuration = errorTrialDuration;

            Debug.Log("cursorClass.currentBlock: " + cursorClass.currentBlock);

            if ((session.CurrentTrial.settings.GetInt("training") == 1))
                cursorClass.blockScore = 0;

            int[] BlocksBeforeConditionsChanges = { 5 };

            cursorClass.displayingConditionsInstructions = BlocksBeforeConditionsChanges.Contains(cursorClass.currentBlock);
        }
        else if (session.CurrentTrial.numberInBlock == 1)
        {
            cursor.GetComponent<CursorClass>().newBlock = false;
            cursorClass.errorTrialDuration = errorTrialDuration;
        }
        else
            cursorClass.errorTrialDuration = Time.time - (float)session.CurrentTrial.result["start_time"];

        if (session.currentTrialNum == session.Trials.Count())
            cursorClass.postExperiment = true;

        session.EndCurrentTrial();
        cursorClass.experimentActive = false;
        return true;
    }
}
