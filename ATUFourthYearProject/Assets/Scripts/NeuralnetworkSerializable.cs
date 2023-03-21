using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkSerializable
{
    [SerializeField] int[] layers;
    [SerializeField] float[] neurons;
    [SerializeField] float[] biases;
    [SerializeField] float[] weights;

    public NeuralNetworkSerializable(int[] layers, float [][] biases, float [][][] weights)
    {
        this.layers = layers;
        this.biases = BiasesTo1D(biases);

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

    public float[][] BiasesTo2D()
    {
        return null;
    }

    private float[] WeightsTo1D(float[][][] weights)
    {
        List<float> weightsList = new List<float>();
        for (int i = 1; i < layers.Length; i++)
        {
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < layers[i]; j++)
            {
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    weightsList.Add(weights[i][j][k]);
                }
            }
        }
        return weightsList.ToArray();
    }

    public float[] WeightsTo3D()
    {
        return null;
    }
}
