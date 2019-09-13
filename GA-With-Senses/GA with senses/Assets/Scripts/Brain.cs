﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    int DNALength = 2;
    public float timeAlive;
    public float timeWalking;
    public DNA dna;
    public GameObject eyes;
    bool alive = true;
    bool seeGround = true;

    public GameObject ethanPrefab;
    GameObject ethan;

    private void OnDestroy () {
        Destroy (ethan);
    }

    private void OnCollisionEnter ( Collision collision ) {
        if ( collision.gameObject.tag == "dead" ) {
            alive = false;
            timeAlive = 0;
            timeWalking = 0;
        }
    }

    public void Init () {
        /*
         * Initialize DNA
         * 0 forward
         * 1 left
         * 2 right
         */

        dna = new DNA ( DNALength , 3 );
        timeAlive = 0;
        alive = true;

        ethan = Instantiate (ethanPrefab, transform.position, transform.rotation);
        ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = transform;
    }

    private void Update () {
        if ( !alive ) return;

        //Debug.DrawRay ( eyes.transform.position , eyes.transform.forward * 10 , Color.red , 10 );
        seeGround = false;
        RaycastHit hit;
        if ( Physics.Raycast ( eyes.transform.position , eyes.transform.forward * 10 , out hit ) ) {
            if ( hit.collider.gameObject.tag == "platform" ) {
                seeGround = true;
            }
        }

        timeAlive = PopulationManager.elapsed;

        //read DNA
        float turn = 0;
        float move = 0;

        if ( seeGround ) {
            //make move relative to character and always move forward
            if ( dna.GetGene ( 0 ) == 0 ) {
                move = 1;
                timeWalking += 1;
            } else if ( dna.GetGene ( 0 ) == 1 ) {
                turn = -90;
            } else if ( dna.GetGene ( 0 ) == 2 ) {
                turn = 90;
            }
        } else {
            if ( dna.GetGene ( 1 ) == 0 ) {
                move = 1;
                timeWalking += 1;
            } else if ( dna.GetGene ( 1 ) == 1 ) {
                turn = -90;
            } else if ( dna.GetGene ( 1 ) == 2 ) {
                turn = 90;
            }
        }

        transform.Translate ( 0 , 0 , move * 0.1f );
        transform.Rotate ( 0 , turn , 0 );
    }
}
