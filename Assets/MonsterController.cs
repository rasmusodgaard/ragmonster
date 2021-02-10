using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class MonsterController : MonoBehaviour
{
    private Rigidbody2D rb_front;
    private float inputHorizontal;
    private float inputVertical;

    public float force = 1;

    [OnValueChanged("ValueAdjustment")]
    public float legTorque = 20;

    [OnValueChanged("ValueAdjustment")]
    public float bodySpringTension = 5;

    //Joints
    private SpringJoint2D[] bodyJoints;
    private List<RelativeJoint2D> upperLegs, lowerLegs;

    private void Awake()
    {
        rb_front = GameObject.FindWithTag("Body_Front").GetComponent<Rigidbody2D>();

        upperLegs = new List<RelativeJoint2D>();
        lowerLegs = new List<RelativeJoint2D>();


        bodyJoints = GetComponentsInChildren<SpringJoint2D>();
        GetLegJoints();
    }

    private void ValueAdjustment()
    {
        foreach(var joint in upperLegs)
        {
            joint.maxTorque = legTorque;
        }
        foreach(var joint in lowerLegs)
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
                upperLegs.Add(joint);
            }
            else if(joint.name == "lowerLeg")
            {
                lowerLegs.Add(joint);
            }
        }
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if(inputHorizontal != 0)
        {
            rb_front.AddForce((Vector2.one * inputHorizontal) * force, ForceMode2D.Impulse);
            print("Force");
        }
        if(inputVertical != 0)
        {
            rb_front.AddForce((Vector2.one * inputHorizontal) * force, ForceMode2D.Impulse);
        }
    }
}
