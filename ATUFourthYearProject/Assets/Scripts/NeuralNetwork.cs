using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine;

public class NeuralNetwork
{
    // specifices the dimensions of layers for the neural network
    [JsonProperty] private int[] _layers;
    // 2 dimensional array containing the neurons of the network
    private float[][] _neurons;
    // 2 dimensional array containing the biases of the neural network
    [JsonProperty] private float[][] _biases;
    // 3 dimensional array containing the weights of the neural network
    [JsonProperty] private float[][][] _weights;//weights

    // simple constructor for neural network, takes a 1 dimensional int array containing the dimensions of the neural network
    public NeuralNetwork(int[] layers)
    {
        // Set local layers variable to passed in layers parameter
        _layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            _layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork otherNetwork)
    {
        // Set this layers instance to layers from other network
        _layers = new int[otherNetwork._layers.Length];
        Array.Copy(otherNetwork._layers, _layers, otherNetwork._layers.Length);

        // Copy neurons from otherNetwork instance to this one
        _neurons = new float[otherNetwork._neurons.Length][];
        for (int i = 0; i < otherNetwork._neurons.Length; i++)
        {
            _neurons[i] = new float[otherNetwork._neurons[i].Length];
            Array.Copy(otherNetwork._neurons[i], _neurons[i], otherNetwork._neurons[i].Length);
        }

        // Copy biases from otherNetwork instance to this one, is made more complicated by it being dimensional
        _biases = new float[otherNetwork._biases.Length][];
        for (int i = 0; i < otherNetwork._biases.Length; i++)
        {
            _biases[i] = new float[otherNetwork._biases[i].Length];
            Array.Copy(otherNetwork._biases[i], _biases[i], otherNetwork._biases[i].Length);
        }

        // Copy weights from other network instance to this one, is most complicated because it's 3 dimensional
        _weights = new float[otherNetwork._weights.Length][][];
        for (int i = 0; i < otherNetwork._weights.Length; i++)
        {
            _weights[i] = new float[otherNetwork._weights[i].Length][];
            for (int j = 0; j < otherNetwork._weights[i].Length; j++)
            {
                // The inner array is copied, as with c#, the Copy method doesn't work properly with multi dimension arrays
                _weights[i][j] = new float[otherNetwork._weights[i][j].Length];
                Array.Copy(otherNetwork._weights[i][j], _weights[i][j], otherNetwork._weights[i][j].Length);
            }
        }

        //Mutate(SimulationManager.Instance().MutationChance, SimulationManager.Instance().mutationValue);
        Mutate(30, 0.05f);

        string jsoned = JsonConvert.SerializeObject(this);
    }

    //create empty storage array for the neurons in the network, neurons store the values calculated durind the FeedForward method
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < _layers.Length; i++)
        {
            neuronsList.Add(new float[_layers[i]]);
        }
        _neurons = neuronsList.ToArray();
    }

    //initializes and populates array for the biases being held within the network, biases act as flat modifiers to the values calculated in neurons
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
                _biases[i][j] = (UnityEngine.Random.Range(0f, 100) > chance) ? _biases[i][j] += UnityEngine.Random.Range(-val, val) : _biases[i][j];
            }
        }

        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                for (int k = 0; k < _weights[i][j].Length; k++)
                {
                    _weights[i][j][k] = (UnityEngine.Random.Range(0f, 100) > chance) ? _weights[i][j][k] += UnityEngine.Random.Range(-val, val) : _weights[i][j][k];

                }
            }
        }
    }
}
