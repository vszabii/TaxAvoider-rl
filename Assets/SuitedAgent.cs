using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SuitedAgent : Agent
{
    private Rigidbody m_rigidbody;
    private float m_speed = 10;


    public GameObject Spawner;

    private Vector3 startingPosition = new Vector3(0.0f, 0.2f, 0f);

    private float boundXLeft = -9f;
    private float boundXRight = 9f;

    private enum ACTIONS
    {
        LEFT = 0,
        NOTHING = 1,
        RIGHT = 2
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPosition;
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
        }
        else if (horizontal == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else
        {
            actions[0] = (int)ACTIONS.NOTHING;
        }

    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        switch (actionTaken)
        {
            case (int)ACTIONS.NOTHING:
                break;
            case (int)ACTIONS.LEFT:
                if (transform.localPosition.x > boundXLeft)
                    transform.position += -Vector3.right * m_speed * Time.deltaTime;
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case (int)ACTIONS.RIGHT:
                if (transform.localPosition.x < boundXRight)
                    transform.position += Vector3.right * m_speed * Time.deltaTime;
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }

        AddReward(0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TAX")
        {
            var parent = Spawner.transform;
            int numberOfChildren = parent.childCount;

            for (int i = 0; i < numberOfChildren; i++)
            {
                if (parent.GetChild(i).tag == "TAX")
                {
                    Destroy(parent.GetChild(i).gameObject);
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("End of episode");
            EndEpisode();
        }
    }

}