using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_MazeWalker : MonoBehaviour {
    public float distanceTravelled;
    public DNA dna;
    public GameObject eyes;
    

    bool seeFrontWall = true;
    bool seeLeftWall = true;
    bool seeRightWall = true;
    bool DNAConsidersPhysics = false;

    int DNALength = 5;
    int DNAMaxValues = 3;

    Vector3 startingPoint;

    public void Init () {
        /*
         * Initialize DNA
         * 0 forward
         * 1 left
         * 2 right
         */

        dna = new DNA ( DNALength , DNAMaxValues, false );
    }

    private void Start () {
        startingPoint = transform.position;
    }

    private void Update () {

        //Check environment for walls
        seeFrontWall = false;
        seeRightWall = false;
        seeLeftWall = false;

        //check for front wall

        RaycastHit frontHit;
        if ( Physics.Raycast ( eyes.transform.position , eyes.transform.forward , out frontHit , 1.0f ) ) {
            if ( frontHit.collider.gameObject.tag == "wall" ) {
                seeFrontWall = true;
                Debug.DrawLine ( eyes.transform.position , frontHit.transform.position , Color.red );
            }
        }

        //check for wall at the right

        RaycastHit rightHit;
        if ( Physics.Raycast ( eyes.transform.position , eyes.transform.right , out rightHit , 1.0f ) ) {
            if ( rightHit.collider.gameObject.tag == "wall" ) {
                seeRightWall = true;
                Debug.DrawLine ( eyes.transform.position , rightHit.transform.position , Color.red );
            }
        }

        //check for wall at the left

        RaycastHit leftHit;
        if ( Physics.Raycast ( eyes.transform.position , eyes.transform.right * -1 , out leftHit , 1.0f ) ) {
            if ( leftHit.collider.gameObject.tag == "wall" ) {
                seeLeftWall = true;
                Debug.DrawLine ( eyes.transform.position , leftHit.transform.position , Color.red );
            }
        }
    }

    private void FixedUpdate () {
        //Read DNA
        float turn = 0;
        float move = 0;

        if ( seeFrontWall ) {

            if ( seeLeftWall && !seeRightWall ) {
                if ( dna.GetGene ( 0 ) == 0 ) {
                    move = 1;

                } else if ( dna.GetGene ( 0 ) == 1 ) {
                    turn = Random.Range ( 10.0f , 180.0f );
                } else if ( dna.GetGene ( 0 ) == 2 ) {
                    turn = Random.Range ( -10.0f , -180.0f );
                }
            } else if ( seeRightWall && !seeLeftWall ) {
                if ( dna.GetGene ( 1 ) == 0 ) {
                    move = 1;

                } else if ( dna.GetGene ( 1 ) == 1 ) {
                    turn = Random.Range ( 10.0f , 180.0f );
                } else if ( dna.GetGene ( 1 ) == 2 ) {
                    turn = Random.Range ( -10.0f , -180.0f );
                }
            } else if ( seeLeftWall && seeRightWall ) {
                if ( dna.GetGene ( 2 ) == 0 ) {
                    move = 1;

                } else if ( dna.GetGene ( 2 ) == 1 ) {
                    turn = Random.Range ( 10.0f , 180.0f );
                } else if ( dna.GetGene ( 2 ) == 2 ) {
                    turn = Random.Range ( -10.0f , -180.0f );
                }
            } else {
                if ( dna.GetGene ( 3 ) == 0 ) {
                    move = 1;

                } else if ( dna.GetGene ( 3 ) == 1 ) {
                    turn = Random.Range ( 10.0f , 180.0f );
                } else if ( dna.GetGene ( 3 ) == 2 ) {
                    turn = Random.Range ( -10.0f , -180.0f );
                }
            }

        } else if ( !seeFrontWall ) {
            if ( dna.GetGene ( 4 ) == 0 ) {
                move = 1;

            } else if ( dna.GetGene ( 4 ) == 1 ) {
                turn = Random.Range ( 10.0f , 180.0f );
            } else if ( dna.GetGene ( 4 ) == 2 ) {
                turn = Random.Range ( -10.0f , -180.0f );
            }
        }

        transform.Translate ( 0 , 0 , move * 0.1f );
        transform.Rotate ( 0 , turn , 0 );

        distanceTravelled = Vector3.Distance ( startingPoint , transform.position );
    }
}
