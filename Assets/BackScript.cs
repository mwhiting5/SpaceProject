using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackScript : MonoBehaviour
{

    public Main mainScript ;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = GameObject.Find("MainController").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)  ){
            mainScript.returnToOverview();
        } 

    }
}
