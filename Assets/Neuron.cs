using System;
using System.Collections.Generic;

using UnityEngine;

public class Neuron
{

    float value;

    List<Connection> inputs;

	public Neuron()
	{
        value = 0;

        inputs = new List<Connection>();
	}

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void Connect(Neuron neuron, float weight)
    {
        Connection connection = new Connection(neuron, weight);
        inputs.Add(connection);
    }

    public void ConputeValue()
    {
        value = 0;

        foreach(Connection connection in inputs)
        {
            value += connection.GetValue(); 
        }
    }

    public float GetOutput()
    {
        return 1 / (1 + Mathf.Exp(-value));
    }

    public List<Connection> GetInputs()
    {
        return inputs;
    }
}
