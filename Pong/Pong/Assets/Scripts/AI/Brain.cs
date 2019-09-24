using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public GameObject paddle;
    public GameObject ball;
    Rigidbody2D ballRigidBody;
    float yVelocity;
    float paddleMinY = 8.8f;
    float paddleMaxY = 17.4f;
    float paddleMaxSpeed = 15;
    public float numSaved = 0;
    public float numMissed = 0;

    ANN ann;

    // Start is called before the first frame update
    void Start () {
        ann = new ANN ( 6 , 1 , 1 , 4 , 0.11 );
        ballRigidBody = ball.GetComponent<Rigidbody2D> ();
    }

    List<double> Run ( double ballXPosition , double ballYPosition , double ballXVelocity , double ballYVelocity , double paddleXPosition , double paddleYPosition , double paddleVelocity , bool train ) {
        List<double> inputs = new List<double> ();
        List<double> outputs = new List<double> ();

        // Adding Inputs
        // Ball position
        inputs.Add ( ballXPosition );
        inputs.Add ( ballYPosition );
        // Ball velocity
        inputs.Add ( ballXVelocity );
        inputs.Add ( ballYVelocity );
        // Paddle position
        inputs.Add ( paddleXPosition );
        inputs.Add ( paddleYPosition );

        //Add outputs
        // Paddle velocity
        outputs.Add ( paddleVelocity );

        if ( train ) {
            return ( ann.Train ( inputs , outputs ) );
        } else {
            return ( ann.CalcOutput ( inputs , outputs ) );
        }


    }

    // Update is called once per frame
    void Update () {
        float posY = Mathf.Clamp ( paddle.transform.position.y + ( yVelocity * Time.deltaTime * paddleMaxSpeed ) , paddleMinY , paddleMaxY );
        paddle.transform.position = new Vector3 ( paddle.transform.position.x , posY , paddle.transform.position.z );

        List<double> output = new List<double> ();
        int layerMask = 1 << 8;

        RaycastHit2D hit = Physics2D.Raycast ( ball.transform.position , ballRigidBody.velocity , 1000 , layerMask );

        if ( hit.collider != null ) {

            if ( hit.collider.gameObject.tag == "tops" ) {
                Vector3 reflection = Vector3.Reflect ( ballRigidBody.velocity , hit.normal );
                hit = Physics2D.Raycast ( hit.point , reflection , 1000 , layerMask );
            }

            if ( hit.collider != null && hit.collider.gameObject.tag == "backwall" ) {
                float dy = ( hit.point.y - paddle.transform.position.y );

                output = Run ( ball.transform.position.x , ball.transform.position.y , ballRigidBody.velocity.x , ballRigidBody.velocity.y , paddle.transform.position.x , paddle.transform.position.y , dy , true );

                yVelocity = ( float ) output[0];
            }

        } else {
            yVelocity = 0;
        }
    }
}
