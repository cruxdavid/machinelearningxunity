using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    MoveBall ball;

    private void Update () {
        if ( Input.GetKeyDown ( "space" ) ) {
            ball.ResetBall ();
        }

        if ( Input.GetKeyDown ( "r" ) ) {
            SceneManager.LoadScene ( 0 , LoadSceneMode.Single );
        }
    }
    
}
