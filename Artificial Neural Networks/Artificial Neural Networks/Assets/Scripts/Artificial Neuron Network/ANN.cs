﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN {

    public int numInputs;
    public int numOutputs;
    public int numHidden;
    public int numNPerHidden;
    public double alpha;
    List<Layer> layers = new List<Layer> ();

    public ANN ( int nInput , int nOutput , int nHidden , int nNeuronPerHidden , double a ) {
        numInputs = nInput;
        numOutputs = nOutput;
        numHidden = nHidden;
        numNPerHidden = nNeuronPerHidden;
        alpha = a;

        if ( numHidden > 0 ) {
            layers.Add ( new Layer ( numNPerHidden , numInputs ) );

            for ( int i = 0; i < numHidden - 1; i++ ) {
                layers.Add ( new Layer ( numNPerHidden , numNPerHidden ) );
            }

            layers.Add ( new Layer ( numOutputs , numNPerHidden ) );
        } else {
            layers.Add ( new Layer ( numOutputs , numInputs ) );
        }
    }

    public List<double> Go ( List<double> inputValues , List<double> desiredOutput ) {
        List<double> outputs = new List<double> ();

        if ( inputValues.Count != numInputs ) {
            Debug.Log ( "ERROR: Number of Inputs must be " + numInputs );
            return outputs;
        }

        List<double> inputs = new List<double> ( inputValues );
        for ( int i = 0; i < numHidden + 1; i++ ) { // Loop through layers
            if ( i > 0 ) { // If not the input layer, pass the output from the previous layers. 
                inputs = new List<double> ( outputs );
            }
            outputs.Clear ();

            for ( int j = 0; j < layers[i].numNeurons; j++ ) { // Loop through neurons
                // N stores the weight for all inputs
                double N = 0;
                layers[i].neurons[j].inputs.Clear ();

                for ( int k = 0; k < layers[i].neurons[j].numInputs; k++ ) { // Loop through inputs
                    layers[i].neurons[j].inputs.Add ( inputs[k] );
                    N += layers[i].neurons[j].weights[k] * inputs[k];
                }

                N -= layers[i].neurons[j].bias;

                if ( i == numHidden ) {
                    layers[i].neurons[j].output = ActivationFunctionO ( N );
                } else {
                    layers[i].neurons[j].output = ActivationFunction ( N );
                }
                outputs.Add ( layers[i].neurons[j].output );
            }
        }

        UpdateWeights ( outputs , desiredOutput );

        return outputs;
    }

    void UpdateWeights ( List<double> outputs , List<double> desiredOutput ) {
        double error;

        for ( int i = numHidden; i >= 0; i-- ) { //back propagation, passing error from output layer to previous layers
            for ( int j = 0; j < layers[i].numNeurons; j++ ) {
                if ( i == numHidden ) {
                    error = desiredOutput[j] - outputs[j];
                    layers[i].neurons[j].errorGradient = outputs[j] * ( 1 - outputs[j] ) * error;
                    //errorGradient calculated with Delta Rule: en.wikipedia.org/wiki/Delta_rule
                } else {
                    layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * ( 1 - layers[i].neurons[j].output );
                    double errorGradSum = 0;
                    for ( int p = 0; p < layers[i + 1].numNeurons; p++ ) {
                        errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradSum;
                }

                for ( int k = 0; k < layers[i].neurons[j].numInputs; k++ ) {
                    if ( i == numHidden ) {
                        error = desiredOutput[j] - outputs[j];
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error;
                    } else {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                    }
                }

                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
            }
        }
    }

    //see en.wikipedia.org/wiki/Activation_function
    double ActivationFunction ( double value ) {
        return ReLu ( value );
    }

    double ActivationFunctionO ( double value ) {
        return Sigmoid ( value );
    }

    // Activation functions:
    double Step ( double value ) { //aka binary step
        if ( value < 0 ) {
            return 0;
        } else {
            return 1;
        }
    }

    double Sigmoid ( double value ) { // aka logistic softstep
        double k = ( double ) System.Math.Exp ( value );
        return k / ( 1.0f + k );
    }

    double TanH ( double value ) { // for outputs with (-) values.
        return ( 2 * ( Sigmoid ( 2 * value ) ) - 1 );
    }

    double ArcTan ( double value ) {
        return Mathf.Atan ( ( float ) value );
    }

    double Sinusoid ( double value ) {
        return Mathf.Sin ( ( float ) value );
    }

    double ElliotSig_SoftSign ( double value ) {
        return ( value / ( 1 + Mathf.Abs ( ( float ) value ) ) );
    }

    double ReLu ( double value ) { // Rectified Linear Unit
        if ( value > 0 ) {
            return value;
        } else {
            return 0;
        }
    }

    double LeakyReLu ( double value ) {
        // Same as ReLu for postive values, except it has a slope for negative values.
        if ( value < 0 ) {
            return 0.01 * value;
        } else {
            return value;
        }
    }




}
