using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject AIBrain;

    [SerializeField]
    PlayerPaddleController playerControls;

    public void SetupControls (bool isAI) {
        if (isAI) {
            Destroy (playerControls);
            AIBrain.SetActive (true);
        } 
    }
}
