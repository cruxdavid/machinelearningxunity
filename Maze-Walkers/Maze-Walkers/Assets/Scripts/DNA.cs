using System.Collections.Generic;
using UnityEngine;

public class DNA {
    List<int> genes = new List<int> ();
    int dnaLength;
    int maxValues;

    public DNA ( int l , int v ) {
        dnaLength = l;
        maxValues = v;
        SetRandom ();
    }

    public void SetRandom () {
        genes.Clear ();
        for ( int i = 0; i < dnaLength; i++ ) {
            genes.Add ( Random.Range ( 0 , maxValues ) );
        }
    }

    public void SetInt ( int pos , int value ) {
        genes[pos] = value;
    }

    public void Combine ( DNA parent1 , DNA parent2 ) {
        for ( int i = 0; i < dnaLength; i++ ) {
            if ( i < dnaLength / 2.0 ) {
                int g = parent1.genes[i];
                genes[i] = g;
            } else {
                int g = parent2.genes[i];
                genes[i] = g;
            }
        }
    }

    public void Mutate () {
        genes[Random.Range ( 0 , dnaLength )] = Random.Range ( 0 , maxValues );
    }

    public int GetGene (int pos) {
        return genes[pos];
    }

}
