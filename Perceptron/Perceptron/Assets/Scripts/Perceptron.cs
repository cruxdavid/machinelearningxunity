using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Perceptron : MonoBehaviour {

    // public int trainingSessions;
    public GameObject npc;

    List<TrainingSet> trainingSets = new List<TrainingSet> ();
    double[] weights = { 0 , 0 };
    double bias = 0;
    double totalError = 0;

    //public SimpleGrapher simpleGrapher;

    private void Start () {
        InitialiseWeights ();

        //DrawAllPoints ();
        // Train ( trainingSessions );
        //simpleGrapher.DrawRay ((float)(-(bias/weights[1])/(bias/weights[0])), (float)(-bias/weights[1]),Color.red);
        //Debug.Log ( "Test 0 0 :" + CalcOutput ( 0 , 0 ) );
        //Debug.Log ( "Test 0 1 :" + CalcOutput ( 0 , 1 ) );
        //Debug.Log ( "Test 1 0 :" + CalcOutput ( 1 , 0 ) );
        //Debug.Log ( "Test 1 1 :" + CalcOutput ( 1 , 1 ) );
    }

    private void Update () {
        if (Input.GetKeyDown("s")) {
            SaveWeights ();
        }else if (Input.GetKeyDown("l")) {
            LoadWeights ();
        }
    }

    void InitialiseWeights () {
        for ( int i = 0; i < weights.Length; i++ ) {
            weights[i] = Random.Range ( -1.0f , 1.0f );
        }
        bias = Random.Range ( -1.0f , 1.0f );
    }

    void LoadWeights () {
        string path = Application.dataPath + "/weights.txt";
        if (File.Exists(path)) {
            var sr = File.OpenText (path);
            string line = sr.ReadLine ();
            string[] w = line.Split (',');
            weights[0] = System.Convert.ToDouble (w[0]);
            weights[1] = System.Convert.ToDouble (w[1]);
            bias = System.Convert.ToDouble (w[2]);
        }
    }

    void SaveWeights () {
        string path = Application.dataPath + "/weights.txt";
        var sr = File.CreateText (path);
        sr.WriteLine (weights[0]+","+weights[1]+","+bias);
        sr.Close();
    }

    public void SendInput ( double i1 , double i2 , double o ) {
        //react 
        double result = CalcOutput ( i1 , i2 );
        Debug.Log ( result );
        if ( result == 0 ) {
            npc.GetComponent<Animator> ().SetTrigger ( "Crouch" );
            npc.GetComponent<Rigidbody> ().isKinematic = false;
        } else {
            npc.GetComponent<Rigidbody> ().isKinematic = true;
        }

        //learn from it next time
        TrainingSet ts = new TrainingSet ();
        ts.input = new double[2] { i1 , i2 };
        ts.output = o;
        trainingSets.Add ( ts );
        Train ();
    }

    void Train () {
        for ( int i = 0; i < trainingSets.Count; i++ ) {
            UpdateWeights ( i );
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
        return ActivationFunction ( DotProductBias ( weights , trainingSets[i].input ) );
    }

    double CalcOutput ( double i1 , double i2 ) {

        double[] inp = { i1 , i2 };
        return ActivationFunction ( DotProductBias ( weights , inp ) );
    }

    double ActivationFunction ( double dotProdcut ) {
        if ( dotProdcut > 0 ) {
            return 1;
        } else {
            return 0;
        }
    }

    //void DrawAllPoints () {
    //    for ( int i = 0; i < trainingSets.Length; i++ ) {
    //        if (trainingSets[i].output == 0) {
    //            simpleGrapher.DrawPoint ((float)trainingSets[i].input[0], ( float ) trainingSets[i].input[1], Color.magenta );
    //        } else {
    //            simpleGrapher.DrawPoint ( ( float ) trainingSets[i].input[0] , ( float ) trainingSets[i].input[1] , Color.green );
    //        }
    //    }
    //}


}

[System.Serializable]
public class TrainingSet {
    public double[] input;
    public double output;
}
