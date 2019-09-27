using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Brain : MonoBehaviour {
    
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
