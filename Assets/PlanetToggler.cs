using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetToggler : MonoBehaviour
{
    public int planetNumber = -1;
    public Main mainScript ;
    public int shadows = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GameObject.Find("MainController").GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        if( shadows == 1){
            if(transform.localScale.x == 0.1f){
                gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                shadows=0;
            }
        }
    }

    void setPlanetNumber(int n){
        planetNumber = n;
    }

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0) && planetNumber != -1){
            mainScript.updateActive(planetNumber);
        }
    }


}