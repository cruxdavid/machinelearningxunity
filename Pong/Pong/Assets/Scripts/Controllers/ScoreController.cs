using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static System.Action goal;

    [SerializeField]
    Text scoreText;
    int score = 0;

    private void Start () {
        scoreText.text = ""+score;
    }

    private void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.transform.tag == "ball" ) {
            score++;
            scoreText.text = "" + score;
            if (goal != null) {
                goal.Invoke ();
            }
        }
    }
}
