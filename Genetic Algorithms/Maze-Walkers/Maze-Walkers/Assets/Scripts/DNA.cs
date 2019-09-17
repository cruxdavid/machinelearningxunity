using System.Collections.Generic;
using UnityEngine;

public class DNA {
    List<int> genes = new List<int> ();
    int dnaLength;
    int maxValues;
    bool considersPhysics;

    public DNA ( int l , int v, bool considersPhysics ) {
        dnaLength = l;
        maxValues = v;
        this.considersPhysics = considersPhysics;
        SetRandom ();
    }

    public void SetRandom () {
        genes.Clear ();
        for ( int i = 0; i < dnaLength; i++ ) {
            if (!considersPhysics) {
                genes.Add ( Random.Range ( 0 , maxValues ) );
            } else {
                genes.Add ( Random.Range ( -maxValues , maxValues ) );
            }
        }
    }

    public void SetInt ( int pos , int value ) {
        genes[pos] = value;
    }

    public void Combine ( DNA parent1 , DNA parent2 ) {
        for ( int i = 0; i < dnaLength; i++ ) {
            genes[i] = Random.Range ( 0 , 10 ) < 5 ? parent1.genes[i] : parent2.genes[i];
        }
    }

    public void Mutate () {
        if (!considersPhysics) {
            genes[Random.Range ( 0 , dnaLength )] = Random.Range ( 0 , maxValues );
        } else {
            genes[Random.Range ( 0 , dnaLength )] = Random.Range ( -maxValues , maxValues );
        }
        
    }

    public int GetGene (int pos) {
        return genes[pos];
    }

}
