using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_MazeWalker : MonoBehaviour {
    public float distanceTravelled;
    public DNA dna;
    public GameObject eyes;
    bool seeWall = true;

    int DNALength = 2;
    int DNAMaxValues = 3;

    public void Init () {
        /*
         * Initialize DNA
         * 0 forward
         * 1 left
         * 2 right
         */

        dna = new DNA ( DNALength , DNAMaxValues );
    }

    private void Awake () {
        Init ();
    }

    private void Update () {

        //Check environment for walls
        seeWall = false;
        Debug.DrawRay ( eyes.transform.position , eyes.transform.forward , Color.red , 0.1f );
        RaycastHit hit;
        if ( Physics.Raycast ( eyes.transform.position , eyes.transform.forward * 0.1f , out hit ) ) {
            if ( hit.collider.gameObject.tag == "wall" ) {
                seeWall = true;
            }
        }

        //Read DNA
        float turn = 0;
        float move = 0;

        if ( seeWall ) {
            if ( dna.GetGene ( 0 ) == 0 ) {
                move = 1;
            } else if ( dna.GetGene ( 0 ) == 1 ) {
                turn = 90.0f;
            } else if ( dna.GetGene ( 0 ) == 2 ) {
                turn = -90.0f;
            }
        } else {
            if ( dna.GetGene ( 1 ) == 0 ) {
                move = 1;
            } else if ( dna.GetGene ( 1 ) == 1 ) {
                turn = 90.0f;
            } else if ( dna.GetGene ( 1 ) == 2 ) {
                turn = -90.0f;
            }
        }

        transform.Translate (0,0,move * 0.1f);
        transform.Rotate (0,turn,0);
    }
}
