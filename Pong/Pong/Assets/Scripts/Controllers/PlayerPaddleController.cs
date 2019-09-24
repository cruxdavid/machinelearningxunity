using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour {

    [SerializeField]
    private float paddleSpeed;

    private void Update () {
        if ( Input.GetKey ( KeyCode.DownArrow ) ) {
            MovePaddle ( false );
        } else if ( Input.GetKey ( KeyCode.UpArrow ) ) {
            MovePaddle ( true );
        }
    }

    void MovePaddle ( bool up ) {

        transform.position = new Vector3 ( transform.position.x , GetYPosition ( up ) , transform.position.z );

    }

    float GetYPosition ( bool up ) {
        float y = up ? ( transform.position.y + paddleSpeed * Time.deltaTime ) : ( transform.position.y + paddleSpeed * Time.deltaTime * -1.0f );
        y = Mathf.Clamp ( y , 8.8f , 17.4f );
        return y;
    }
}
