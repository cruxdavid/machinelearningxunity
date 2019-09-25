using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Drive : MonoBehaviour {

    public float speed = 200.0f;
    public float rotationSpeed = 100.0f;
    public float visibleDistance = 50.0f;

    List<string> collectedTrainingData = new List<string> ();
    StreamWriter tdf;

    private void Start () {
        string path = Application.dataPath + "/trainingData.txt";
        tdf = File.CreateText ( path );

    }

    // Update is called once per frame
    void Update () {
        float translationInput = Input.GetAxis ( "Vertical" );
        float rotationInput = Input.GetAxis ( "Horizontal" );

        float translation = Time.deltaTime * speed * translationInput;
        float rotation = Time.deltaTime * rotationSpeed * rotationInput;

        transform.Translate ( 0 , 0 , translation );
        transform.Rotate ( 0 , rotation , 0 );

        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0, r45Dist = 0, l45Dist = 0;

        //forward
        if ( Physics.Raycast ( transform.position , transform.forward , out hit , visibleDistance ) ) {
            fDist = 1 - Round ( hit.distance / visibleDistance );  //Normalize the distance
            Debug.DrawLine ( transform.position , hit.point , Color.red ); // forward
        }
        //right 
        if ( Physics.Raycast ( transform.position , transform.right , out hit , visibleDistance ) ) {
            rDist = 1 - Round ( hit.distance / visibleDistance );
            Debug.DrawLine ( transform.position , hit.point , Color.red ); // forward
        }
        //left  
        if ( Physics.Raycast ( transform.position , -transform.right , out hit , visibleDistance ) ) {
            lDist = 1 - Round ( hit.distance / visibleDistance );
            Debug.DrawLine ( transform.position , hit.point , Color.red ); // forward
        }
        // right45
        if ( Physics.Raycast ( transform.position , Quaternion.AngleAxis ( -45 , Vector3.up ) * transform.right , out hit , visibleDistance ) ) {
            r45Dist = 1 - Round ( hit.distance / visibleDistance );
            Debug.DrawLine ( transform.position , hit.point , Color.red ); // forward
        }
        // left45
        if ( Physics.Raycast ( transform.position , Quaternion.AngleAxis ( 45 , Vector3.up ) * -transform.right , out hit , visibleDistance ) ) {
            l45Dist = 1 - Round ( hit.distance / visibleDistance );
            Debug.DrawLine ( transform.position , hit.point , Color.red ); // forward
        }

        //td stands for training data
        string td = fDist + "," + rDist + "," + lDist + "," + r45Dist + "," + l45Dist + "," + Round ( translationInput ) + "," + Round ( rotationInput );

        if ( translationInput != 0 && rotationInput != 0 ) {
            if ( !collectedTrainingData.Contains ( td ) ) {
                collectedTrainingData.Add ( td );
            }
        }



    }

    float Round ( float x ) {
        return ( float ) System.Math.Round ( x , System.MidpointRounding.AwayFromZero ) / 2.0f;
    }

    private void OnApplicationQuit () {
        foreach ( string td in collectedTrainingData ) {
            tdf.WriteLine ( td );
        }
        tdf.Close ();
    }
}
