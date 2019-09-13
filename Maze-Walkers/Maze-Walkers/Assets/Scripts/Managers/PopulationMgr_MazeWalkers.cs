using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationMgr_MazeWalkers : MonoBehaviour {

    public GameObject botPrefab;
    public int populationSize = 10;
    public static float elapsed = 0;
    public float trialTime = 60;
    int generation = 1;

    List<GameObject> population = new List<GameObject> ();
    GUIStyle guiStyle = new GUIStyle ();

    private void OnGUI () {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;

        GUI.BeginGroup ( new Rect ( 10 , 10 , 250 , 150 ) );
        GUI.Box ( new Rect ( 0 , 0 , 140 , 140 ) , "Stats" , guiStyle );
        GUI.Label ( new Rect ( 10 , 25 , 200 , 30 ) , "Gen: " + generation , guiStyle );
        GUI.Label ( new Rect ( 10 , 50 , 200 , 30 ) , string.Format ( "Time: {0:0.00}" , elapsed ) , guiStyle );
        GUI.Label ( new Rect ( 10 , 75 , 200 , 30 ) , "Population: " + populationSize , guiStyle );
        GUI.EndGroup ();
    }

    private void Start () {
        for ( int i = 0; i < populationSize; i++ ) {
            GameObject bot = Instantiate ( botPrefab , transform.position , Quaternion.identity );
            bot.GetComponent<Brain_MazeWalker> ().Init ();
            population.Add ( bot );
        }
    }

    private void Update () {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime) {
            BreedNewPopulation ();
            elapsed = 0;
        }
    }

    GameObject Breed (GameObject parent1, GameObject parent2) {
        GameObject offspring = Instantiate (botPrefab,transform.position,Quaternion.identity);
        Brain_MazeWalker b = offspring.GetComponent<Brain_MazeWalker> ();

        if (Random.Range(0,100) == 1) {
            b.Init ();
            b.dna.Mutate ();
        } else {
            b.Init ();
            b.dna.Combine (parent1.GetComponent<Brain_MazeWalker>().dna, parent2.GetComponent<Brain_MazeWalker>().dna);
        }

        return offspring;
    }

    void BreedNewPopulation () {
        List<GameObject> sortedList = population.OrderBy ( o => o.GetComponent<Brain_MazeWalker> ().distanceTravelled).ToList ();
        population.Clear ();

        for ( int i = ( int ) ( sortedList.Count / 2.0f ) - 1; i < sortedList.Count - 1; i++ ) {
            population.Add ( Breed ( sortedList[i] , sortedList[i + 1] ) );
            population.Add ( Breed ( sortedList[i + 1] , sortedList[i] ) );
        }

        for ( int i = 0; i < sortedList.Count; i++ ) {
            Destroy ( sortedList[i] );
        }

        generation++;

    }
}
