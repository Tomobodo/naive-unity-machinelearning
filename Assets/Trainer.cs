using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trainer : MonoBehaviour {

    public GameObject training;

    public int column = 5;
    public int row = 5;

    public float spaceBeetween = 8.0f;

    public float keepRatio = 0.25f;
    public float mutateRatio = 0.50f; 

    public float mutationAmplitude = 1.0f;

    public float timeScale = 1.0f;

    public Text timeText;

    public int generation = 0;

    List<Training> trainings;

    float bestFitness = 0;

    List<List<float>> lastBestFit;

    int nbToKeep = 0;
    int nbToMutate = 0;

    // Use this for initialization
    void Start () {
        lastBestFit = new List<List<float>>();

        int total = column * row;

        nbToKeep = Mathf.RoundToInt(total * keepRatio);
        nbToMutate = Mathf.RoundToInt(total * mutateRatio);

        SpawnTraining();
	}

    void SpawnTraining(bool useBests = false)
    {
        trainings = new List<Training>();

        int bestIndex = 0;
        int nbMutated = 0;

        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < column; ++j)
            {
                GameObject newTraining = Instantiate(training, transform);
                newTraining.transform.position = Vector3.forward * i * spaceBeetween + Vector3.right * j * spaceBeetween;
                Training trainingScript = newTraining.GetComponent<Training>();
                trainings.Add(trainingScript);

                AIAgent agent = newTraining.GetComponentInChildren<AIAgent>();

                if (useBests && bestIndex < lastBestFit.Count)
                {
                    agent.SetBrainConfig(lastBestFit[bestIndex]);
                    bestIndex++;
                } else if (useBests && nbMutated < nbToMutate)
                {
                    List<float> mutatedConfig = new List<float>();

                    foreach(float gene in lastBestFit[nbMutated % lastBestFit.Count])
                    {
                        mutatedConfig.Add(gene + Random.Range(-mutationAmplitude / 2, mutationAmplitude / 2));
                    }

                    agent.SetBrainConfig(mutatedConfig);

                    nbMutated++;
                }
            }
        }

        generation++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            timeScale++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (timeScale > 1f)
            {
                timeScale--;
            }
            else
            {
                timeScale = 0.1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextGeneration();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        Time.timeScale = timeScale;
        
        if (AllTrainingAreDead())
        {
            NextGeneration();
        }

        timeText.text = "Best fit : " + bestFitness +
                        "\nGeneration : " + generation +
                        "\nTimeScale : " + timeScale;
    }

    void NextGeneration()
    {
        SaveBestFit();
        DestroyAllTraining();
        SpawnTraining(true);
    }

    void SaveBestFit()
    {
        trainings.Sort(new TrainingComparer());

        lastBestFit.Clear();

        for(int i = 0; i < nbToKeep; ++i)
        {
            AIAgent agent = trainings[i].GetComponentInChildren<AIAgent>();
            lastBestFit.Add(agent.GetBrainConfig());
        }

        bestFitness = trainings[0].fitness;
    }

    void DestroyAllTraining ()
    {
        foreach(Training currentTraining in trainings)
        {
            Destroy(currentTraining.gameObject);
        }

        trainings.Clear();
    }

    bool AllTrainingAreDead()
    {
        foreach (Training currentTraining in trainings)
        {
            if (currentTraining.ended != true)
            {
                return false;
            }
        }

        return true;
    }
}
