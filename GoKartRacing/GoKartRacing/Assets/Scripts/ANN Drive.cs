using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ANNDrive : MonoBehaviour {

    ANN ann;
    public float visibleDistance = 200;
    public int epochs = 1000;
    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;

    bool trainingDone = false;
    float trainingProgress = 0;
    double sse = 0;
    double lastSSE = 1;

    public float translation;
    public float rotation;

    // Start is called before the first frame update
    void Start () {
        ann = new ANN ( 5 , 2 , 1 , 10 , 0.5 );
        StartCoroutine ( LoadTrainingSet () );
    }

    private void OnGUI () {
        GUI.Label ( new Rect ( 25 , 25 , 250 , 30 ) , "SSE: " + lastSSE );
        GUI.Label ( new Rect ( 25 , 40 , 250 , 30 ) , "Alpha: " + ann.alpha );
        GUI.Label ( new Rect ( 25 , 55 , 250 , 30 ) , "Trained: " + trainingProgress );
    }

    IEnumerator LoadTrainingSet () {
        string path = Application.dataPath + "/trainingData.txt";
        string line;

        if ( File.Exists ( path ) ) {
            int lineCount = File.ReadAllLines ( path ).Length;
            StreamReader tdf = File.OpenText ( path );
            List<double> calcOutputs = new List<double> ();
            List<double> inputs = new List<double> ();
            List<double> outputs = new List<double> ();

            for ( int i = 0; i < epochs; i++ ) {
                //set file pointer to beginning of file.
                sse = 0;
                tdf.BaseStream.Position = 0;

                while ( ( line = tdf.ReadLine () ) != null ) {
                    string[] data = line.Split ( ',' );
                    //if nothing to be learned ignore this line.
                    float thisError = 0;
                    if ( System.Convert.ToDouble ( data[5] ) != 0 && System.Convert.ToDouble ( data[6] ) != 0 ) {
                        inputs.Clear ();
                        outputs.Clear ();
                        inputs.Add ( System.Convert.ToDouble ( data[0] ) );
                        inputs.Add ( System.Convert.ToDouble ( data[1] ) );
                        inputs.Add ( System.Convert.ToDouble ( data[2] ) );
                        inputs.Add ( System.Convert.ToDouble ( data[3] ) );
                        inputs.Add ( System.Convert.ToDouble ( data[4] ) );

                        double o1 = Map ( 0 , 1 , -1 , 1 , System.Convert.ToSingle ( data[5] ) );
                        outputs.Add ( o1 );
                        double o2 = Map ( 0 , 1 , -1 , 1 , System.Convert.ToSingle ( data[6] ) );
                        outputs.Add ( o2 );

                        calcOutputs = ann.Train ( inputs , outputs );
                        thisError = ( ( Mathf.Pow ( ( float ) ( outputs[0] - calcOutputs[0] ) , 2 ) + Mathf.Pow ( ( float ) ( outputs[1] - calcOutputs[1] ) , 2 ) ) ) / 2.0f;
                    }
                    sse += thisError;
                }
                trainingProgress = ( float ) i / ( float ) epochs;
                sse /= lineCount;
                lastSSE = sse;

                yield return null;
            }
        }

        trainingDone = true;
    }

    float Map ( float newfrom , float newto , float origfrom , float origto , float value ) {

        if (value <= origfrom) {
            return newfrom;
        }else if (value >= origto) {
            return newto;
        }

        return ( newto - newfrom ) * ( ( value - origfrom ) / ( origto - origfrom ) ) * newfrom;
    }

    float Round (float x) {
        return ( float ) System.Math.Round (x , System.MidpointRounding.AwayFromZero) / 2.0f;
    }
}
