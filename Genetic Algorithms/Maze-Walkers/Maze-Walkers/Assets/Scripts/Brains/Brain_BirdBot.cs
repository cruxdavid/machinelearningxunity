using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_BirdBot : MonoBehaviour {
    public DNA dna;
    public GameObject eyes;

    public float timeAlive = 0;
    public float distanceTravelled = 0;
    public int crash = 0;

    int DNALenght = 5;
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeFloor = false;
    bool seeTop = false;
    bool alive = true;
    Rigidbody2D rb;
    Vector3 startPosition;

    public void Init () {

        /*
         * Initialise DNA
         * 0 forward
         * 1 upwall
         * 2 downwall
         * 3 normal upward
         */

        dna = new DNA ( DNALenght , 200 , true );
        transform.Translate ( Random.Range ( -1.5f , 1.5f ) , Random.Range ( -1.5f , 1.5f ) , 0 );
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D> ();
    }

    private void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.gameObject.tag == "dead" ) {
            alive = false;
        }
    }

    private void OnCollisionStay2D ( Collision2D collision ) {
        if ( collision.gameObject.tag == "floor" || collision.gameObject.tag == "roof" || collision.gameObject.tag == "upwall" || collision.gameObject.tag == "downwall" ) {
            crash++;
        }
    }

    private void Update () {
        if ( !alive ) return;

        seeUpWall = false;
        seeDownWall = false;
        seeTop = false;
        seeFloor = false;

        RaycastHit2D hit = Physics2D.Raycast ( eyes.transform.position , eyes.transform.right , 1.5f );
        if ( hit.collider != null ) {

            if ( hit.transform.tag == "upwall" ) {
                seeUpWall = true;
                Debug.DrawLine ( eyes.transform.position , hit.transform.position , Color.red );
            } else if ( hit.transform.tag == "downwall" ) {
                seeDownWall = true;
                Debug.DrawLine ( eyes.transform.position , hit.transform.position , Color.red );
            }
        }

        hit = Physics2D.Raycast ( eyes.transform.position , eyes.transform.up , 1.5f );
        if ( hit.collider != null ) {

            if ( hit.transform.tag == "roof" ) {
                seeTop = true;
                Debug.DrawLine ( eyes.transform.position , hit.transform.position , Color.red );

            }
        }

        hit = Physics2D.Raycast ( eyes.transform.position , -eyes.transform.up , 1.5f );
        if ( hit.collider ) {

            if ( hit.transform.tag == "floor" ) {
                seeFloor = true;
                Debug.DrawLine ( eyes.transform.position , hit.collider.transform.position , Color.red );
            }
        }

        timeAlive = PopulationMgr_BirdBots.elapsed;

    }

    private void FixedUpdate () {
        if ( !alive ) return;
        //read DNA
        float upforce = 0;
        float forwardForce = 1.0f;

        if ( seeUpWall ) {
            upforce = dna.GetGene ( 0 );
        } else if ( seeDownWall ) {
            upforce = dna.GetGene ( 1 );
        } else if ( seeTop ) {
            upforce = dna.GetGene ( 2 );
        } else if ( seeFloor ) {
            upforce = dna.GetGene ( 3 );
        } else {
            upforce = dna.GetGene ( 4 );
        }

        rb.AddForce ( transform.right * forwardForce );
        rb.AddForce ( transform.up * upforce * 0.1f );
        distanceTravelled = Vector3.Distance ( startPosition , transform.position );
    }
}
