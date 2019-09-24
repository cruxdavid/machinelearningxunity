using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {

    Vector3 ballStartPosition;
    Rigidbody2D rb;
    float speed = 400;
    public AudioSource blip;
    public AudioSource blop;

    private void OnEnable () {
        ScoreController.goal += ScoreController_Goal;
    }

    private void OnDisable () {
        ScoreController.goal -= ScoreController_Goal;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        ballStartPosition = transform.position;
        ResetBall ();
    }

    private void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.gameObject.tag == "backwall" ) {
            blop.Play ();
        } else {
            blip.Play ();
        }
    }

    public void ResetBall () {
        transform.position = ballStartPosition;
        rb.velocity = Vector3.zero;
        Vector3 dir = new Vector3 ( Random.Range ( -300 , 300 ) , Random.Range ( -100 , 100 ) , 0 ).normalized;
        StartCoroutine ( AddForceToBall ( dir ) );
    }

    IEnumerator AddForceToBall (Vector3 dir) {
        yield return new WaitForSeconds (0.5f);
        rb.AddForce ( dir * speed );
    }

    void ScoreController_Goal () {
        ResetBall ();
    }
}
