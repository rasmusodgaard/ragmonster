using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class MonsterController : MonoBehaviour
{
    [Header("General controls")]
    public float forceVertical = 1;
    public float forceHorizontal = 1;

    [Header("Body")]

    [OnValueChanged("ValueAdjustment")]
    public float bodySpringTension = 5;

    [OnValueChanged("ValueAdjustment"), Range(0, 1)]
    public float bodyDampingRatio = 0.1f;

    [Header("Legs")]

    [OnValueChanged("ValueAdjustment")]
    public float legMaxTorque = 20;

    [OnValueChanged("ValueAdjustment")]
    public float legMaxForce = 20;

    [OnValueChanged("ValueAdjustment"), Range(0, 1)]
    public float legCorrectionScale = 0.3f;

    [Header("Head")]

    [OnValueChanged("ValueAdjustment")]
    public float neckMaxTorque = 20;

    [OnValueChanged("ValueAdjustment")]
    public float neckMaxForce = 20;

    [OnValueChanged("ValueAdjustment"), Range(0, 1)]
    public float neckCorrectionScale = 20;

    [Header("Tail")]

    [OnValueChanged("ValueAdjustment")]
    public float tailMaxTorque = 20;

    [OnValueChanged("ValueAdjustment")]
    public float tailMaxForce = 20;

    [OnValueChanged("ValueAdjustment"), Range(0, 1)]
    public float tailCorrectionScale = 20;



    //Movement related
    private Rigidbody2D rb_front;
    private float inputHorizontal;
    private float inputVertical;

    //Joints
    private List<Joint2D> allJoints = new List<Joint2D>();
    private SpringJoint2D[] bodyJoints;
    private List<RelativeJoint2D> upperLegJoints, lowerLegJoints, neckJoints, tailJoints;

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
        neckJoints = new List<RelativeJoint2D>();
        tailJoints = new List<RelativeJoint2D>();

        ragdollJoints = GetComponentsInChildren<HingeJoint2D>(true);
        bodyJoints = GetComponentsInChildren<SpringJoint2D>();
        SortBodyParts();
        allJoints = AssembleAllJoints();

        ValueAdjustment();
    }

    private void ValueAdjustment()
    {
        print("ValueAdjustment");
        foreach(var joint in upperLegJoints)
        {
            joint.maxTorque = legMaxTorque;
            joint.maxForce = legMaxForce;
            joint.correctionScale = legCorrectionScale;
        }
        foreach(var joint in lowerLegJoints)
        {
            joint.maxTorque = legMaxTorque;
            joint.maxForce = legMaxForce;
            joint.correctionScale = legCorrectionScale;
        }
        foreach(var joint in bodyJoints)
        {
            joint.frequency = bodySpringTension;
            joint.dampingRatio = bodyDampingRatio;
        }
        foreach(var joint in neckJoints)
        {
            joint.maxTorque = neckMaxTorque;
            joint.maxForce = neckMaxForce;
            joint.correctionScale = neckCorrectionScale;
        }
        foreach(var joint in tailJoints)
        {
            joint.maxTorque = tailMaxTorque;
            joint.maxForce = tailMaxForce;
            joint.correctionScale = tailCorrectionScale;
        }
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
        foreach(var joint in neckJoints)
        {
            output.Add(joint);
        }
        foreach(var joint in tailJoints)
        {
            output.Add(joint);
        }

        return output;
    }



    private void SortBodyParts()
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
            else if(joint.name == "neckJoint")
            {
                neckJoints.Add(joint);
            }
            else if(joint.name == "tailJoint")
            {
                tailJoints.Add(joint);
            }
        }
    }

    [ContextMenu("Attach hinge joints")]
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
            rb_front.AddForce((Vector2.right * inputHorizontal) * forceHorizontal, ForceMode2D.Impulse);
        }
        if(!Mathf.Approximately(inputVertical, 0.0f))
        {
            rb_front.AddForce((Vector2.up * inputVertical) * forceVertical, ForceMode2D.Impulse);
        }
    }
}
