using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Brain : MonoBehaviour {

    public GameObject ball;

    ANN ann;

    float reward = 0.0f;
    List<Replay> replayMemory = new List<Replay> ();
    int mCapacity = 1000;

    float discount = 0.99f;
    float exploreRate = 100.0f;
    float maxExploreRate = 100.0f;
    float minExploreRate = 0.01f;
    float exploreDecay = 0.0001f;

    Vector3 ballStartPos;
    int failCount = 0;
    float tiltSpeed = 0.5f;

    float timer = 0;
    float maxBalanceTime = 0;

    private void Start () {
        ann = new ANN ( 3 , 2 , 1 , 6 , 0.2f );
        ballStartPos = ball.transform.position;
        Time.timeScale = 5.0f;
    }

    GUIStyle guiStyle = new GUIStyle ();

    private void OnGUI () {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
    }
}

public class Replay {

    public List<double> states;
    public double reward;

    public Replay ( double xr , double ballz , double ballvx , double r ) {
        states = new List<double> ();
        states.Add ( xr );
        states.Add ( ballz );
        states.Add ( ballvx );
        reward = r;

    }

}
