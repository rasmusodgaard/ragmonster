using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class MonsterController : MonoBehaviour
{
    [Header("General controls")]
    public float force = 1;

    [Header("Body")]

    [OnValueChanged("ValueAdjustment")]
    public float bodySpringTension = 5;

    [Header("Legs")]

    [OnValueChanged("ValueAdjustment")]
    public float legTorque = 20;


    //Movement related
    private Rigidbody2D rb_front;
    private float inputHorizontal;
    private float inputVertical;

    //Joints
    private List<Joint2D> allJoints = new List<Joint2D>();
    private SpringJoint2D[] bodyJoints;
    private List<RelativeJoint2D> upperLegJoints, lowerLegJoints;

    private HingeJoint2D[] ragdollJoints;

    private void Awake()
    {
        rb_front = GameObject.FindWithTag("Body_Front").GetComponent<Rigidbody2D>();
        Init();
    }

    private void Init()
    {
        upperLegJoints = new List<RelativeJoint2D>();
        lowerLegJoints = new List<RelativeJoint2D>();

        ragdollJoints = GetComponentsInChildren<HingeJoint2D>(true);
        bodyJoints = GetComponentsInChildren<SpringJoint2D>();
        GetLegJoints();
        allJoints = AssembleAllJoints();
    }

    private List<Joint2D> AssembleAllJoints()
    {
        List<Joint2D> output = new List<Joint2D>();

        foreach(var joint in upperLegJoints)
        {
            output.Add(joint);
        }
        foreach(var joint in lowerLegJoints)
        {
            output.Add(joint);
        }
        foreach(var joint in bodyJoints)
        {
            output.Add(joint);
        }
        foreach(var joint in ragdollJoints)
        {
            output.Add(joint);
        }

        return output;
    }

    private void ValueAdjustment()
    {
        print("ValueAdjustment");
        foreach(var joint in upperLegJoints)
        {
            joint.maxTorque = legTorque;
        }
        foreach(var joint in lowerLegJoints)
        {
            joint.maxTorque = legTorque;
        }
        foreach(var joint in bodyJoints)
        {
            joint.frequency = bodySpringTension;
        }
    }

    private void GetLegJoints()
    {
        RelativeJoint2D[] relativeJoints = GetComponentsInChildren<RelativeJoint2D>();
        foreach(var joint in relativeJoints)
        {
            if(joint.name == "upperLeg")
            {
                upperLegJoints.Add(joint);
            }
            else if(joint.name == "lowerLeg")
            {
                lowerLegJoints.Add(joint);
            }
        }
    }

    public void CountHingejoints()
    {
        print(GetComponentsInChildren<HingeJoint2D>().Length + " hingejoints");
    }

    public void TransferReferencesBetweenJoints()
    {
        Init();
        print("run function with " + ragdollJoints.Length + " hingejoints.");

        foreach(var joint in ragdollJoints)
        {

            Joint2D[] joints = joint.GetComponents<Joint2D>();
            foreach(var item in joints)
            {
                if(item.enabled)
                    joint.connectedBody = item.connectedBody;
            }

        }
    }


    private void ToggleRagdoll()
    {
        foreach(var joint in allJoints)
        {
            joint.enabled = !joint.enabled;
        }
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.R))
        {
            ToggleRagdoll();
        }
    }

    private void FixedUpdate()
    {
        if(!Mathf.Approximately(inputHorizontal, 0.0f))
        {
            rb_front.AddForce((Vector2.one * inputHorizontal) * force, ForceMode2D.Impulse);
        }
        if(!Mathf.Approximately(inputVertical, 0.0f))
        {
            rb_front.AddForce((Vector2.one * inputHorizontal) * force, ForceMode2D.Impulse);
        }
    }
}
