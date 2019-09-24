using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private enum GameType { Player , AI};

    [SerializeField]
    GameType gameType = GameType.Player;

    [SerializeField]
    GameObject player1;

    [SerializeField]
    MoveBall ball;

    private void Start () {
        if (gameType == GameType.AI) {
            player1.GetComponent<SetupPlayer> ().SetupControls (true);
        }
    }

    private void Update () {
        if ( Input.GetKeyDown ( "space" ) ) {
            ball.ResetBall ();
        }

        if ( Input.GetKeyDown ( "r" ) ) {
            SceneManager.LoadScene ( 0 , LoadSceneMode.Single );
        }
    }
    
}
