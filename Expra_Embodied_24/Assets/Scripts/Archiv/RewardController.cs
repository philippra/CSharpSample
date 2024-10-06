using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UXF;

public class RewardController : MonoBehaviour
{
    public Session session;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cursor"))
        {
            Debug.Log("Reward collected!");
            session.EndCurrentTrial();

            return true;
        }

        return false;
    }
}
