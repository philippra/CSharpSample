
using System.Collections.Generic;
using UnityEngine;
using DualSenseUnity;
using UXF;
using UnityEngine.Windows;
using System.Collections;

public class CursorControls : MonoBehaviour
{
    [SerializeField]
    public Session session;

    ControllerOutputState output = new();
    public uint _controllerCount;
    public List<DualSenseController> _dualSenseControllers;
    public DualSenseController dsc;

    public bool paramsSet = false;

    public bool controllerSteady;

    public float leftStickPrev;
    public float rightStickPrev;

    public float trackingStickPrev;
    public float trackingStickNow;

    public float trackingStickDiff;

    public float decisionStickNow;

    private float gyroState;

    private float intendedDeltaTime = 1.0f / 60.0f;

    //private int buttonPressCounter = 0;

    [SerializeField]
    public CursorClass cc;

    public GameObject cursor;

    public void Awake()
    {
        RefreshControllers();
        controllerSteady = false;
        if (dsc == null)
            Application.Quit();
    }

    public ref ControllerOutputState returnOutput()
    {
        return ref output;
    }

    public void Set_Start_Parameters()
    {
        

        if ((int)session.participantDetails["counterbalance"] % 2 != 0)
        {
            if (session.CurrentTrial.settings.GetString("response_mapping") == "overlap")
                trackingStickPrev = (float)dsc.GetInputState().RightStick.XAxis;
            else if (session.CurrentTrial.settings.GetString("response_mapping") == "non-overlap")
            {
                trackingStickPrev = (float)(dsc.GetInputState().RightStick.PushButton);
            }
        }
        else if ((int)session.participantDetails["counterbalance"] % 2 == 0)
        {
            if (session.CurrentTrial.settings.GetString("response_mapping") == "overlap")
                trackingStickPrev = (float)dsc.GetInputState().LeftStick.XAxis;
            else if (session.CurrentTrial.settings.GetString("response_mapping") == "non-overlap")
            {
                trackingStickPrev = (float)(dsc.GetInputState().LeftStick.PushButton);
            }
        }

        output.LightBarEnabled = false;
        output.LightBarIntensity = 0.0d;

        gyroState = (float)dsc.GetInputState().Gyroscope.XAxis / 90;

        output.LeftTriggerEffect.InitializeContinuousResistanceEffect(0.0f, 0.0f);
        output.RightTriggerEffect.InitializeContinuousResistanceEffect(0.0f, 0.0f);


        dsc.SetOutputState(output);

        paramsSet = true;

    }

    public float get_gyro_state(ControllerInputState _inputStateController)
    {
        //Debug.Log((float)_inputStateController.Gyroscope.XAxis);
        gyroState = (float)_inputStateController.Gyroscope.XAxis/90;
        transform.localEulerAngles = new Vector3(gyroState, 0.0f, 0.0f);

        return gyroState;
    }

    public float ComputeTrackingDiff(int pert_direct, ControllerInputState inputStateController, CursorClass cursorClass)
    {
        float orthogonalAxis = 0.0f;
        float orthogonalThreshold = 0.40f;

        string responseMapping = session.CurrentTrial.settings.GetString("response_mapping");
        float diffScalar = responseMapping == "overlap" ? 1.1f : 1.1f;

        if (cursorClass.newTrial)
        {
            cursorClass.newTrial = false;
            //buttonPressCounter = 0;
        }

        bool isCounterbalanceOdd = (int)session.participantDetails["counterbalance"] % 2 != 0;

        if (isCounterbalanceOdd)
        {
            ProcessInputState(responseMapping, (float)inputStateController.RightStick.XAxis, (float)inputStateController.RightStick.YAxis,
                              inputStateController.RightStick.PushButton, ref trackingStickNow, ref orthogonalAxis);
        }
        else
        {
            ProcessInputState(responseMapping, (float)inputStateController.LeftStick.XAxis, (float)inputStateController.LeftStick.YAxis,
                              inputStateController.LeftStick.PushButton, ref trackingStickNow, ref orthogonalAxis);
        }

        return CalculateTrackingDiff(pert_direct, responseMapping, orthogonalAxis, orthogonalThreshold, diffScalar);
    }

    private void ProcessInputState(string responseMapping, float xAxis, float yAxis, ButtonState button,
                                   ref float trackingStickNow, ref float orthogonalAxis)
    {
        if (responseMapping == "overlap")
        {
            trackingStickNow = xAxis;
            orthogonalAxis = yAxis;
        }
        else if (responseMapping == "non-overlap")
        {
            trackingStickNow = yAxis;
            orthogonalAxis = xAxis;

            if ((int)session.participantDetails["counterbalance"] > 4)
                trackingStickNow = trackingStickNow > 0.0f ? 0.0f : trackingStickNow;
            else
                trackingStickNow = trackingStickNow < 0.0f ? 0.0f : trackingStickNow;

            // counterbalance mapping yAxis tracking direction

        }

        if ((int)session.participantDetails["counterbalance"] % 2 != 0)
            decisionStickNow = (float)dsc.GetInputState().LeftStick.XAxis;
        else if ((int)session.participantDetails["counterbalance"] % 2 == 0)
            decisionStickNow = (float)dsc.GetInputState().RightStick.XAxis;

        //Debug.Log("Decision Stick Now: " + decisionStickNow);

    }

    private float CalculateTrackingDiff(int pert_direct, string responseMapping, float orthogonalAxis,
                                        float orthogonalThreshold, float diffScalar)
    {
        switch (pert_direct)
        {
            case -1:
                return HandlePerturbation(responseMapping, orthogonalAxis, orthogonalThreshold, diffScalar, -1);
            case 1:
                return HandlePerturbation(responseMapping, orthogonalAxis, orthogonalThreshold, diffScalar, 1);
            default:
                trackingStickPrev = trackingStickNow;
                return 0.0f;
        }
    }

    private float HandlePerturbation(string responseMapping, float orthogonalAxis, float orthogonalThreshold,
                                     float diffScalar, int direction)
    {
        if (responseMapping == "overlap")
        {
            bool shouldReset = Mathf.Abs(orthogonalAxis) > orthogonalThreshold ||
                               (direction == -1 && (trackingStickPrev < 0 || trackingStickNow < 0 || Mathf.Approximately(trackingStickPrev, -1.0f))) ||
                               (direction == 1 && (trackingStickPrev > 0 || trackingStickNow > 0 || Mathf.Approximately(trackingStickPrev, 1.0f)));
            trackingStickDiff = shouldReset ? 0.0f : (trackingStickNow - trackingStickPrev);
            if (direction == -1 && trackingStickDiff < 0.0f) trackingStickDiff = 0.0f;
            if (direction == 1 && trackingStickDiff > 0.0f) trackingStickDiff = 0.0f;
        }
        else if (responseMapping == "non-overlap")
        {
            trackingStickDiff = (trackingStickNow - trackingStickPrev);

            bool shouldReset = true;
            int counterbalance = (int)session.participantDetails["counterbalance"];

            if (counterbalance <= 4)
                shouldReset = Mathf.Abs(orthogonalAxis) > orthogonalThreshold || (direction == -1 && (trackingStickDiff < 0.0f || trackingStickNow < 0.0f || trackingStickPrev < 0.0f)) ||
                (direction == 1 && (trackingStickDiff < 0.0f || trackingStickNow < 0.0f || trackingStickPrev < 0.0f));
            else if (counterbalance > 4)
                shouldReset = Mathf.Abs(orthogonalAxis) > orthogonalThreshold || (direction == -1 && (trackingStickDiff > 0.0f || trackingStickNow > 0.0f || trackingStickPrev > 0.0f)) ||
                (direction == 1 && (trackingStickDiff > 0.0f || trackingStickNow > 0.0f || trackingStickPrev > 0.0f));
                     
            trackingStickDiff = shouldReset ? 0.0f : trackingStickDiff;

            trackingStickDiff = direction == -1 && counterbalance <= 4 ? trackingStickDiff : -trackingStickDiff;

            trackingStickDiff = direction == -1 && counterbalance > 4 ? -trackingStickDiff : trackingStickDiff;

        }

        trackingStickPrev = trackingStickNow;

        return (trackingStickDiff * diffScalar) / (Time.deltaTime / intendedDeltaTime);
    }

    public List<ButtonState> get_bumper_press()
    {
        List<ButtonState> BumperList = new();
        BumperList.Add(dsc.GetInputState().LeftBumper);
        BumperList.Add(dsc.GetInputState().RightBumper);
        return BumperList;
    }

    public bool HeldStill(float stillStart, float errorTrialDuration, CursorClass cursorClass)
    {

        if (((Time.time - stillStart) > (1.25f+(4.0f-errorTrialDuration))))
        {
            return true;
        }
        else
            return false;
    }

    public void RefreshControllers()
    {
        _controllerCount = DualSense.GetControllerCount();
        _dualSenseControllers = DualSense.GetControllers();
        if (_controllerCount > 0)
            dsc = _dualSenseControllers[0];
    }

}
