using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet {
    public double[] input;
    public double output;
}

public class Perceptron : MonoBehaviour {
    public TrainingSet[] trainingSets;

    double[] weights = { 0 , 0 };
    double bias = 0;
    double totalError = 0;

    private void Start () {
        Train ( 8 );

        Debug.Log ( "Test 0 0 :" + CalcOutput ( 0 , 0 ) );
        Debug.Log ( "Test 0 1 :" + CalcOutput ( 0 , 1 ) );
        Debug.Log ( "Test 1 0 :" + CalcOutput ( 1 , 0 ) );
        Debug.Log ( "Test 1 1 :" + CalcOutput ( 1 , 1 ) );

    }

    void InitialiseWeights () {
        for ( int i = 0; i < weights.Length; i++ ) {
            weights[i] = Random.Range ( -1.0f , 1.0f );
        }
        bias = Random.Range ( -1.0f , 1.0f );
    }

    void Train ( int epochs ) {

        InitialiseWeights ();

        for ( int i = 0; i < epochs; i++ ) {
            totalError = 0;

            for ( int j = 0; j < trainingSets.Length; j++ ) {
                UpdateWeights ( j );
                Debug.Log ( "W1: " + weights[0] + " W2: " + weights[1] + " B: " + bias );
            }
            Debug.Log ( "TOTAL ERROR: " + totalError );
        }
    }

    void UpdateWeights ( int i ) {
        double error = trainingSets[i].output - CalcOutput ( i );

        totalError += Mathf.Abs ( ( float ) error );

        for ( int j = 0; j < weights.Length; j++ ) {
            weights[j] = weights[j] + error * trainingSets[i].input[j];
        }
        bias += error;
    }

    double DotProductBias ( double[] v1 , double[] v2 ) {

        if ( v1 == null || v2 == null ) {
            return -1;
        }

        if ( v1.Length != v2.Length ) {
            return -1;
        }

        double d = 0;

        for ( int i = 0; i < v1.Length; i++ ) {
            d += v1[i] * v2[i];
        }

        d += bias;

        return d;

    }

    double CalcOutput ( int i ) {
        double dp = DotProductBias ( weights , trainingSets[i].input );
        if ( dp > 0 ) {
            return 1;
        } else {
            return 0;
        }
    }

    double CalcOutput ( double i1 , double i2 ) {

        double[] inp = new double[] { i1 , i2 };

        double dp = DotProductBias ( weights , inp );

        if ( dp > 0 ) {
            return 1;
        } else {
            return 0;
        }

    }




}
