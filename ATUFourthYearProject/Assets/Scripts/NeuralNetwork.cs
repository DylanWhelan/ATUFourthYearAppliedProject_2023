using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork
{
    private int[] layers;//layers    
    private float[][] neurons;//neurons    
    private float[][] biases;//biasses    
    private float[][][] weights;//weights
    private NeuralNetworkSerializable neuralNetworkSerializable;
    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();

        neuralNetworkSerializable = new NeuralNetworkSerializable(layers, biases, weights);
    }
    public NeuralNetwork(NeuralNetworkSerializable neuralNetworkSerializable)
    {
        layers = neuralNetworkSerializable.GetLayers();
        InitNeurons();
        biases = neuralNetworkSerializable.GetBiases();
        weights = neuralNetworkSerializable.GetWeights();
    }

    //create empty storage array for the neurons in the network.
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    //initializes and populates array for the biases being held within the network.
    private void InitBiases()
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            float[] bias = new float[layers[i]];
            for (int j = 0; j < layers[i]; j++)
            {
                bias[j] = UnityEngine.Random.Range(-1f, 1f);
            }
            biasList.Add(bias);
        }
        biases = biasList.ToArray();
    }

    //initializes random array for the weights being held in the network.
    private void InitWeights()
    {
        int numWeights = 0;
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
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
        weights = weightsList.ToArray();
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
            neurons[0][i] = inputs[i];        
        }        
        for (int i = 1; i < layers.Length; i++)        
        {                    
            for (int j = 0; j < neurons[i].Length; j++)            
            {                
                float summedValue = 0f;               
                for (int k = 0; k < neurons[i - 1].Length; k++)  
                {                    
                    summedValue += weights[i - 1][j][k] * neurons[i - 1][k];      
                }                
                neurons[i][j] = Activate(summedValue + biases[i][j]);            
            }        
        }        
        return neurons[neurons.Length - 1];    
    }

    public NeuralNetworkSerializable GetNeuralNetworkSerializable()
    {
        return neuralNetworkSerializable;
    }
}
