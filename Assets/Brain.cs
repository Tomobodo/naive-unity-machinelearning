using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Brain
{
    List<List<Neuron>> layers;

    Dictionary<string, Neuron> inputs;
    Dictionary<string, Neuron> outputs;

    List<float> config;

    public Brain(List<float> config = null)
	{
        inputs = new Dictionary<string, Neuron>();
        outputs = new Dictionary<string, Neuron>();

        layers = new List<List<Neuron>>();

        this.config = config;
	}

    public void AddInput(string name)
    {
        inputs[name] = new Neuron();
    }

    public void SetInput(string name, float value)
    {
        Neuron neuron = inputs[name];
        if (neuron != null)
        {
            neuron.SetValue(value);
        }
    }

    public void AddLayer(int length)
    {
        List<Neuron> layer = new List<Neuron>();

        int layerNum = layers.Count;

        for (int i = 0; i < length; ++i)
        {
            Neuron neuron = new Neuron();

            if (layerNum == 0)
            {
                foreach(Neuron input in inputs.Values)
                {
                    neuron.Connect(input, Random.Range(-0.5f, 0.5f));
                }
            }
            else
            {
                List<Neuron> layerToConnect = layers[layerNum - 1];
                foreach(Neuron input in layerToConnect)
                {
                    neuron.Connect(input, Random.Range(-0.5f, 0.5f));
                }
            }

            layer.Add(neuron);
        }

        layers.Add(layer);
    }

    public void AddOutput(string name) 
    {
        Neuron neuron = new Neuron();

        foreach(Neuron input in layers[layers.Count - 1])
        {
            neuron.Connect(input, Random.Range(-0.5f, 0.5f));
        }

        outputs[name] = neuron;
    }

    public void Update()
    {
        foreach(List<Neuron> layer in layers)
        {
            foreach(Neuron neuron in layer)
            {
                neuron.ConputeValue();
            }
        }

        foreach(Neuron output in outputs.Values)
        {
            output.ConputeValue();
        }
    }

    public float GetOutput(string name)
    {
        Neuron output = outputs[name];
        if (output != null)
        {
            return output.GetOutput();
        } else
        {
            return 0;
        }
    }

    public List<float> GetConfig()
    {
        List<float> rep = config;

        if (rep == null)
        {
            rep = new List<float>();

            foreach (List<Neuron> layer in layers)
            {
                foreach (Neuron neuron in layer)
                {
                    foreach(Connection connection in neuron.GetInputs())
                    {
                        rep.Add(connection.weight);
                    }
                }
            }

            foreach (Neuron output in outputs.Values)
            {
                foreach (Connection connection in output.GetInputs())
                {
                    rep.Add(connection.weight);
                }
            }
        }

        return rep;
    }

    public void SetConfig(List<float> config)
    {
        this.config = config;

        int i = 0;

        foreach (List<Neuron> layer in layers)
        {
            foreach (Neuron neuron in layer)
            {
                foreach (Connection c in neuron.GetInputs())
                {
                    c.weight = config[i];
                    i++;
                }
            }
        }

        foreach (Neuron neuron in outputs.Values)
        {
            foreach (Connection c in neuron.GetInputs())
            {
                c.weight = config[i];
                i++;
            } 
        }
    } 

    override public string ToString()
    {
        string rep = "";
        foreach(float w in GetConfig())
        {
            rep += w + ", ";
        }
        return rep;
    }
}
