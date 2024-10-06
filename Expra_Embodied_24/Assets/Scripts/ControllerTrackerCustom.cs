using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UXF
{

    public class ControllerTrackerCustom : Tracker
    {
        [SerializeField]
        CursorControls CursorControls;

        [SerializeField]
        Session session;


        public override string MeasurementDescriptor => "movement";
        public override IEnumerable<string> CustomHeader => new string[] { "pos_x", "pos_y", "pos_z", "rot_x", "rot_y", "rot_z", "TrackingStickNow", "DecisionStickNow", "GyroState"};

        /// <summary>
        /// Returns current position and rotation values
        /// </summary>
        /// <returns></returns>
        protected override UXFDataRow GetCurrentValues()
        {
            // get position and rotation
            Vector3 p = gameObject.transform.position;
            Vector3 r = gameObject.transform.eulerAngles;

            float trackingStickNow = CursorControls.trackingStickNow;

            float decisionStickNow = CursorControls.decisionStickNow;

            float gyroState = CursorControls.get_gyro_state(CursorControls.dsc.GetInputState());

            // if ((int)session.participantDetails["counterbalance"] % 2 != 0)
            //     decisionStickNow = (float)cursorControls.dsc.GetInputState().LeftStick.XAxis;
            // else if ((int)session.participantDetails["counterbalance"] % 2 == 0)
            //     decisionStickNow = (float)cursorControls.dsc.GetInputState().RightStick.XAxis;

            // return position, rotation (x, y, z) as an array
            var values = new UXFDataRow()
            {
                ("pos_x", p.x),
                ("pos_y", p.y),
                ("pos_z", p.z),
                ("rot_x", r.x),
                ("rot_y", r.y),
                ("rot_z", r.z),
                ("TrackingStickNow", trackingStickNow),
                ("DecisionStickNow", decisionStickNow),
                ("GyroState", gyroState)
            };

            return values;
        }

    }

}
