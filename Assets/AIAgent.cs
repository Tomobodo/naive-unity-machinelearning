using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour {

    public Rigidbody ball;

    Brain brain;

    private void Awake()
    {
        brain = new Brain();

        brain.AddInput("ballX");
        brain.AddInput("ballZ");

        brain.AddInput("ballVelocityX");
        brain.AddInput("ballVelocityZ");

        brain.AddLayer(4);

        brain.AddOutput("rotationX");
        brain.AddOutput("rotationZ");
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        brain.SetInput("ballX", ball.transform.localPosition.x);
        brain.SetInput("ballZ", ball.transform.localPosition.z);

        brain.SetInput("ballVelocityX", ball.velocity.x);
        brain.SetInput("ballVelocityZ", ball.velocity.z);

        brain.Update();

        Vector3 eulerRotation = new Vector3(
            (brain.GetOutput("rotationX") - 0.5f) * 90,
            0,
            (brain.GetOutput("rotationZ") - 0.5f) * 90
        );

        transform.eulerAngles = eulerRotation;
    }

    public List<float> GetBrainConfig()
    {
        return brain.GetConfig();
    }

    public void SetBrainConfig(List<float> config)
    {
        brain.SetConfig(config);
    }
}
