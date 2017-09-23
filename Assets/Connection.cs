using System;

public class Connection
{

    public float weight = 1.0f;

    Neuron neuron;

	public Connection(Neuron neuron, float weight)
	{
        this.neuron = neuron;
        this.weight = weight;
	}

    public float GetValue()
    {
        return neuron.GetOutput() * weight;
    }
}
