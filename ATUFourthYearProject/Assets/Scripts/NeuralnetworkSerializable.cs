using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkSerializable
{
    [SerializeField] int[] layers;
    [SerializeField] float[] biases;
    [SerializeField] float[] weights;

    public NeuralNetworkSerializable(int[] layers, float [][] biases, float [][][] weights)
    {
        this.layers = layers;
        this.biases = BiasesTo1D(biases);
        this.weights = WeightsTo1D(weights);

    }

    public int[] GetLayers()
    {
        return layers;
    }

    private float[] BiasesTo1D(float [][] biases)
    {
        List<float> biasList = new List<float>();
        for (int i = 0; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                biasList.Add(biases[i][j]);
            }
        }
        return biasList.ToArray();
    }

    public float[][] GetBiases()
    {
        int arrayIndex = 0;
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            float[] bias = new float[layers[i]];
            for (int j = 0; j < layers[i]; j++)
            {
                bias[j] = biases[arrayIndex++];
            }
            biasList.Add(bias);
        }
        return biasList.ToArray();
    }

    private float[] WeightsTo1D(float[][][] weights)
    {
        int counter = 0;
        List<float> weightsList = new List<float>();
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weightsList.Add(weights[i][j][k]);
                    counter++;
                }
                // Debug.Log("Sub Row: " + j + " has sub cols: " + weights[i][j].Length);
            }
            Debug.Log("Row: " + i + " has sub rows: " + weights[i].Length);
        }
        //Debug.Log("Weights List Length = " + weightsList.Count + " Counter: " + counter);
        return weightsList.ToArray();
    }

    public float[][][] GetWeights()
    {
        int arrayIndex = 0;
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < layers[i]; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    try
                    {
                        neuronWeights[k] = weights[arrayIndex++];
                    }
                    catch(System.IndexOutOfRangeException)
                    {
                        Debug.Log("Exception Occurred when trying to access array index: " + (arrayIndex-1));
                    }
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        return weightsList.ToArray();
    }
}
