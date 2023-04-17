using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine;

public class NeuralNetwork
{
    [JsonProperty] private int[] _layers;//layers    
    private float[][] _neurons;//neurons    
    [JsonProperty] private float[][] _biases;//biasses    
    [JsonProperty] private float[][][] _weights;//weights
    public NeuralNetwork(int[] layers)
    {
        _layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            _layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
        Mutate(10, 0.02f);
    }

    public NeuralNetwork(NeuralNetwork otherNetwork)
    {
        // Copy layers
        _layers = new int[otherNetwork._layers.Length];
        Array.Copy(otherNetwork._layers, _layers, otherNetwork._layers.Length);

        // Copy neurons
        _neurons = new float[otherNetwork._neurons.Length][];
        for (int i = 0; i < otherNetwork._neurons.Length; i++)
        {
            _neurons[i] = new float[otherNetwork._neurons[i].Length];
            Array.Copy(otherNetwork._neurons[i], _neurons[i], otherNetwork._neurons[i].Length);
        }

        // Copy biases
        _biases = new float[otherNetwork._biases.Length][];
        for (int i = 0; i < otherNetwork._biases.Length; i++)
        {
            _biases[i] = new float[otherNetwork._biases[i].Length];
            Array.Copy(otherNetwork._biases[i], _biases[i], otherNetwork._biases[i].Length);
        }

        // Copy weights
        _weights = new float[otherNetwork._weights.Length][][];
        for (int i = 0; i < otherNetwork._weights.Length; i++)
        {
            _weights[i] = new float[otherNetwork._weights[i].Length][];
            for (int j = 0; j < otherNetwork._weights[i].Length; j++)
            {
                _weights[i][j] = new float[otherNetwork._weights[i][j].Length];
                Array.Copy(otherNetwork._weights[i][j], _weights[i][j], otherNetwork._weights[i][j].Length);
            }
        }

        Mutate(10, 0.02f);

        string jsoned = JsonConvert.SerializeObject(this);
        Debug.Log("We got to here!");
        Debug.Log(jsoned);
    }

    //create empty storage array for the neurons in the network.
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < _layers.Length; i++)
        {
            neuronsList.Add(new float[_layers[i]]);
        }
        _neurons = neuronsList.ToArray();
    }

    //initializes and populates array for the biases being held within the network.
    private void InitBiases()
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < _layers.Length; i++)
        {
            float[] bias = new float[_layers[i]];
            for (int j = 0; j < _layers[i]; j++)
            {
                bias[j] = UnityEngine.Random.Range(-1f, 1f);
            }
            biasList.Add(bias);
        }
        _biases = biasList.ToArray();
    }

    //initializes random array for the weights being held in the network.
    private void InitWeights()
    {
        int numWeights = 0;
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < _layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = _layers[i - 1];
            for (int j = 0; j < _neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-1f, 1f);
                    numWeights++;
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        _weights = weightsList.ToArray();
        Debug.Log("Number of Weights in Neural Net" + numWeights);
    }


    public float Activate(float value)
    {
        return (float)Math.Tanh(value);
    }

    public float[] FeedForward(float[] inputs)    
    {        
        for (int i = 0; i < inputs.Length; i++)        
        {            
            _neurons[0][i] = inputs[i];        
        }        
        for (int i = 1; i < _layers.Length; i++)        
        {                    
            for (int j = 0; j < _neurons[i].Length; j++)            
            {                
                float summedValue = 0f;               
                for (int k = 0; k < _neurons[i - 1].Length; k++)  
                {                    
                    summedValue += _weights[i - 1][j][k] * _neurons[i - 1][k];      
                }                
                _neurons[i][j] = Activate(summedValue + _biases[i][j]);            
            }        
        }        
        return _neurons[_neurons.Length - 1];    
    }

    public void Mutate(int chance, float val)
    {
        for (int i = 0; i < _biases.Length; i++)
        {
            for (int j = 0; j < _biases[i].Length; j++)
            {
                _biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ? _biases[i][j] += UnityEngine.Random.Range(-val, val) : _biases[i][j];
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    _weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5) ? _weights[i][j][k] += UnityEngine.Random.Range(-val, val) : _weights[i][j][k];

                }
            }
        }
    }
}
