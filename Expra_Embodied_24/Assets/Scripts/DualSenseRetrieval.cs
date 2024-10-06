using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualSenseUnity;

public class DualSenseRetrieval : MonoBehaviour
{
    private uint _controllerCount;
    private List<DualSenseController> _dualSenseControllers;

    // Start is called before the first frame update
    void Start()
    {
        _controllerCount = DualSense.GetControllerCount();
        _dualSenseControllers = DualSense.GetControllers();
    }

    // Update is called once per frame
    //void LateUpdate()
    //{
        //DualSense.ControllerCountChanged += RefreshControllers;
        //RefreshControllers();
    //}

    private void RefreshControllers()
    {
        _controllerCount = DualSense.GetControllerCount();
        _dualSenseControllers = DualSense.GetControllers();
    }
}