using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class city : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kursor;
    public Sprite spr_attack;
    public Sprite spr_incity;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        kursor.GetComponent<SpriteRenderer>().sprite= spr_attack;
    }
}
