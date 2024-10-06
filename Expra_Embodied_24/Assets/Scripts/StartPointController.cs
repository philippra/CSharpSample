
using UnityEngine;

using UXF;


public class StartPointController : MonoBehaviour
{
    public Session session;
    public GameObject leftReward;
    public GameObject rightReward;
    public GameObject middleLane;
    public GameObject cursor;
    public CursorClass cursorClass;
    public GameObject centralObst;

    //private Vector3 cursorStart;
    //private Vector3 centralObstStart;

    public void Awake()
    {

        cursor = GameObject.Find("Cursor");

        cursorClass = cursor.GetComponent<CursorClass>();

        centralObst = GameObject.Find("CentralObstacle");

        middleLane = GameObject.Find("MiddleLane");

        centralObst = GameObject.Find("CentralObstacle");

        leftReward = GameObject.Find("LeftReward");
        rightReward = GameObject.Find("RightReward");
        leftReward.AddComponent<RewardSize>();
        rightReward.AddComponent<RewardSize>();
    }

    public void Set_Trial_Parameters()
    {

        rightReward.GetComponent<RewardSize>().rewardSize = session.CurrentTrial.settings.GetInt("reward_right");
        leftReward.GetComponent<RewardSize>().rewardSize = 100 - session.CurrentTrial.settings.GetInt("reward_right");

        rightReward.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        leftReward.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        float factorRight = 2.50f * (float)rightReward.GetComponent<RewardSize>().rewardSize / 100;
        float factorLeft = 2.50f * (float)leftReward.GetComponent<RewardSize>().rewardSize / 100;

        rightReward.transform.GetChild(0).transform.localScale = new Vector3(0.1f * factorRight, 5f * factorRight, 0.4f * factorRight);
        leftReward.transform.GetChild(0).transform.localScale = new Vector3(0.1f * factorLeft, 5f * factorLeft, 0.4f * factorLeft);

        cursorClass.stillStart = Time.time;

        cursorClass.thumbREMapping = (session.CurrentTrial.settings.GetString("response_mapping") == "compatible") ? 1 : -1;

    }


    public void Begin_Trial()
    {

                switch (cursorClass.experimentActive)
                {
                    case true:
                        session.BeginNextTrial();
                        cursorClass.newTrial = true;
                        cursorClass.currentPertDirect = session.CurrentTrial.settings.GetInt("pert_direct");
                        break;
                    case false:
                        break;
                }

       
    }

    public void LateUpdate()
    {
        if (!session.InTrial && cursorClass.experimentActive)
            Begin_Trial();
    }

}
