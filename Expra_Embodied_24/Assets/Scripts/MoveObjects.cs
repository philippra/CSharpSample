using System.Linq;
using TMPro;
using UnityEngine;
using DualSenseUnity;
using UXF;

public class MoveObjects : MonoBehaviour
{
    [SerializeField]
    Session session;
    private GameObject cursor;
    private float r;

    private float deltaTime;
    private float perturActual;
    private float intendedDeltaTime = 1.0f / 60.0f;

    private CursorClass cc;

    private ParticleSystem cursorParticleSystem;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    public void Start()
    {

        cursor = GameObject.Find("Cursor");
        cc = cursor.GetComponent<CursorClass>();
        cursorParticleSystem = cursor.GetComponent<ParticleSystem>();

        velocityModule = cursorParticleSystem.velocityOverLifetime;
    }

    public void MoveForward(Transform cursorTransform, CursorClass cursorClass)
    {
        deltaTime = Time.deltaTime;
        cc.forwardVelocity = (0.22f * (deltaTime / intendedDeltaTime));

        cursorTransform.position = new Vector3(cursorTransform.position.x, cursorTransform.position.y, cursorTransform.position.z + cc.forwardVelocity);
    }

    public void PerturbCursor(Transform cursorTransform, CursorClass cursorClass, float pert_strength)
    {
        r = Random.Range(0.25f, 1.25f);
        deltaTime = Time.deltaTime;
        var _tempPertStrength = pert_strength;
        perturActual = r * cursorClass.currentPertDirect * _tempPertStrength * (deltaTime / intendedDeltaTime);

        cursorTransform.position = new Vector3(cursorTransform.position.x + perturActual, cursorTransform.position.y, cursorTransform.position.z);
    }

    public void TrackingMovement(float trackingDiff, Transform cursorTransform, CursorClass cursorClass)
    {
        string responseMapping = session.CurrentTrial.settings.GetString("response_mapping");

        int counterbalance = (int)session.participantDetails["counterbalance"];

        Vector3 adjustVector = new Vector3(-trackingDiff, 0, 0);

        if (responseMapping == "non-overlap" && counterbalance > 4){
            adjustVector = new Vector3(trackingDiff, 0, 0);
        }

        Vector3 finalPos = adjustVector + cursorTransform.position;

        cursorTransform.position = Vector3.Lerp(cursorTransform.position, finalPos, deltaTime / intendedDeltaTime);
    }

    public void Jump(Transform cursorTransform, CursorClass cursorClass, Transform leftLaneTransform, Transform rightLaneTransform, float distanceFromCenterNow, CursorControls cursorControls, float trialTime)
    {
        float decisionStickNow = cursorControls.decisionStickNow;

        if (decisionStickNow < -0.90)
        {
                cursorTransform.position = new Vector3(distanceFromCenterNow + leftLaneTransform.position.x, cursorTransform.position.y, cursorTransform.position.z);
                session.CurrentTrial.result["jumpTime"] = Time.time;
                cursorClass.currentLane = "LeftLane";
                cursorClass.jumped = true;
                cursorClass.jumpTime = trialTime;
        }
        else if (decisionStickNow > 0.90)
        {

                cursorTransform.position = new Vector3(distanceFromCenterNow + rightLaneTransform.position.x, cursorTransform.position.y, cursorTransform.position.z);
                session.CurrentTrial.result["jumpTime"] = Time.time;
                cursorClass.currentLane = "RightLane";
                cursorClass.jumped = true;
                cursorClass.jumpTime = trialTime;
        }
    }

}
