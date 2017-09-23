using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour {

    public Rigidbody ball;

    public float fitness = 0;

    public bool ended = false;

    float lifeSpan = 0;

	// Use this for initialization
	void Start () {

        Vector3 velocity = Vector3.right;

        velocity *= 10.0f;
        velocity = Quaternion.Euler(0, Random.Range(0, 360), 0) * velocity;

        ball.velocity = velocity;
        fitness = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        lifeSpan += Time.fixedDeltaTime;

		if (ball.transform.localPosition.y < -1.5f && !ended)
        {
            ended = true;
            fitness = lifeSpan;
        }

        if (ball.velocity.magnitude < 0.01f && !ended)
        {
            ended = true;
            fitness = lifeSpan + 100000.0f / lifeSpan;
        }
	}
}
