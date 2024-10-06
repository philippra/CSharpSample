using UnityEngine;
using UXF;

public class ResetObjects : MonoBehaviour
{
    public GameObject leftLane;
    public GameObject rightLane;
    public GameObject cursor;
    public GameObject middleLane;

    public CursorClass cursorClass;

    public GameObject leftReward;
    public GameObject rightReward;
    public MeshRenderer leftRewardRenderer;
    public MeshRenderer rightRewardRenderer;

    public void Start()
    {
        leftLane = GameObject.Find("LeftLane");
        rightLane = GameObject.Find("RightLane");
        cursor = GameObject.Find("Cursor");
        middleLane = GameObject.Find("MiddleLane");
        cursorClass = cursor.GetComponent<CursorClass>();
        leftRewardRenderer = leftLane.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
        rightRewardRenderer = rightLane.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();

        leftReward = GameObject.Find("LeftReward");
        rightReward = GameObject.Find("RightReward");

    }

    public void Reset(Session session)
    {

        cursorClass.cc = false;

        middleLane.transform.position = new Vector3(0.0f, 0.0f, 33.0f);

        leftRewardRenderer.enabled = false; // left reward
        rightRewardRenderer.enabled = false; // right reward

        rightReward.transform.GetChild(0).transform.localScale = new Vector3(0.1f, 5f, 0.4f);
        rightReward.transform.GetChild(0).transform.localScale = new Vector3(0.1f, 5f, 0.4f);

        cursor.transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);

        var scale = cursor.transform.localScale;

        scale.Set(1.0f, 1.0f, 1.0f);

        cursor.transform.localScale = scale;

        cursor.transform.position = new Vector3(0.0f, 1.375f, -0.6f);
        
        cursorClass.distanceFromCenterNow = 0.0f;
        cursorClass.distanceFromCenterPrev = 0.0f;

        cursorClass.touchesLane = false;
        cursorClass.jumped = false;
        cursorClass.jumpTime = 1000000;
        cursorClass.rewardCollected = false;
        cursorClass.obstacleCollision = false;
        cursorClass.fellFromLane = false;
        cursorClass.cc = true;
        cursorClass.experimentActive = false;
        cursorClass.stillStart = Time.time;
        cursorClass.currentLane = "MiddleLane";

    }
}
