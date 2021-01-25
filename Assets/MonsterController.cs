using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Rigidbody2D rb_front;

    public float force = 1;

    private void Awake()
    {
        print("Awake?");
        rb_front = GameObject.FindWithTag("Body_Front").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            rb_front.AddForce((Vector2.right + Vector2.up) * force, ForceMode2D.Impulse);
            print("Force");
        }
    }
}
