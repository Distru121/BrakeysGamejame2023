using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitVelocity : MonoBehaviour
{
    public float m_SpeedLimit = 100f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //set velocity back to speed limit if it exceeds said speed
		if(rb.velocity.magnitude > m_SpeedLimit)
			rb.velocity = rb.velocity.normalized * m_SpeedLimit;
    }
}
