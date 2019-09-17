using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    //Gene for color
    public float r;
    public float g;
    public float b;

    //Gene for size
    public float x;
    public float y;

    bool dead = false;
    public float timeToDie = 0;

    SpriteRenderer sRenderer;
    Collider2D sCollider;

    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer> ();
        sCollider = GetComponent<Collider2D> ();

        sRenderer.color = new Color (r,g,b);
        transform.localScale = new Vector3 (x,y,1);
    }

    private void OnMouseDown () {
        dead = true;
        timeToDie = PopulationManager.elapsed;
        Debug.Log ("Dead At: "+ timeToDie );
        sRenderer.enabled = false;
        sCollider.enabled = false;
    }
}
