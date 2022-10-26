using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Main : MonoBehaviour
{

    //Planet Arrays
    public GameObject[] planetPrefabs = new GameObject[9];
    //sizes of planets (sun ==0 )
    public double[] planetRadius = {432.69, 1.516, 3.760, 3.959, 2.106, 
                43.441, 36.184, 15.759, 15.299}; 
    public string[] planetNames = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
    // used for toggling on and off planets: index of planets / actual planet objects
    public List<int> activePlanetIndices;
    public List<GameObject> activeObjects;
    
    public int activeObjCount;

    //range : 10 -> 10
    // sun edge at -8.5
    public double leftScreen = -7;
    public double rightScreen = 8.5;
    public double sunEdge = -8;

    //sum of sizes of active planets
    double sum = 122.024; 

    //total space = 18.5, total white space = 4.5
    // 14.5 units of space to fit sum * 2 things
    // scale = (r / sum) * 14.5
    // spot = prevSpot (in units) + scaled R + 0.5 , 5 , 0

    //UI System
    public GameObject UI;
    public GameObject textPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach( GameObject g in planetPrefabs){
            GameObject newGO = Instantiate(g);
            activePlanetIndices.Add(i);
            activeObjects.Add(newGO);
            i++;
        }
        drawPlanets();

        displayNames();

        //small UI planets for spawning 
        for(int j = 1; j < 9; j++){
            GameObject newGO = Instantiate(planetPrefabs[j]);
            newGO.transform.position = new Vector3( (float) ( -2.8 + ((double)j)*0.7 ) , 0.45f, -8.0f);
            newGO.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activePlanetIndices.Count != activeObjCount && activeObjCount !=1){
            drawPlanets();

            displayNames();
        }
    }

    // called to draw all active Planets
    void drawPlanets(){
        
        //if there are no planets to draw: return and set active obj count to 0
        if(activePlanetIndices.Count == 1){
            activeObjCount = 0;
            return;
        }

        int i = 0;
        double prevR = 0.0;
        double prevX = 0.0;
        double cameraRad = 10.0;
        double constPadding = 4.0 / ((double)activeObjects.Count-1.0);
        double extraPad =0;
        double drawSize = (Math.Abs(leftScreen) + rightScreen) - 4;
        foreach(GameObject g in activeObjects){
            int index = activePlanetIndices[i];
            //scale / transform to default position 
            if( i==0 ){
                double scaleDiameter = (planetRadius[index]/sum) * (drawSize);
                //one planet case
                if(activeObjCount == 2){
                    scaleDiameter = (planetRadius[index] / sum) * 8.0;
                }
                g.transform.position = new Vector3( (float) (sunEdge-(scaleDiameter/2.0)), 5, (float) (scaleDiameter/2));
                g.transform.localScale = new Vector3((float)scaleDiameter,(float)scaleDiameter,(float)scaleDiameter);
            }
            else{
                double scaleDiameter = (planetRadius[index]/sum) * (drawSize);

                // new x coord based on prev planets location and size, as well as current size
                double newX = prevX + prevR + scaleDiameter/2 + constPadding;
                if(i==1){
                    newX -=constPadding ;
                } 

                // only one planet
                if(activeObjects.Count == 2){
                    newX = 2.0 - leftScreen;
                    scaleDiameter = 8;
                }

                //swwapped to orthographic projections, fizes perspective issues requiring semi circle


                g.transform.position = new Vector3( (float) (newX + leftScreen) , 5, (float) 0);
                g.transform.localScale = new Vector3((float)scaleDiameter,(float)scaleDiameter,(float)scaleDiameter);
                
                prevX = newX;
                prevR=  scaleDiameter /2.0;
            }
            i++;
        }
        activeObjCount = i;
    }

    // displays names of current active planets under current locations
    void displayNames(){
        
        //UI RANGE: x: -1920 -> 1920
        //          y:  +-1080
        //          z: keep constant

        //to do:
        foreach(Transform child in UI.transform){
            Destroy(child.gameObject);
        }

        if(activeObjCount==0) return;

        // loop throughall planets except sun
        for(int i = 1; i < activePlanetIndices.Count; i++){
            GameObject currPlanetObj = activeObjects[i];

            //spawn new text on UI
            GameObject newText = Instantiate(textPrefab);
            TMPro.TextMeshProUGUI t = newText.GetComponent<TextMeshProUGUI>();
            t.SetText(planetNames[activePlanetIndices[i]]);
            newText.transform.SetParent(UI.transform);
            
            // set width based on planet R
            RectTransform rt = newText.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2 ( Math.Max(0.24f, (float)currPlanetObj.transform.localScale.x), 0.3f);
            
            //we have text, now edit position
            float newYValue = currPlanetObj.transform.position.y - currPlanetObj.transform.localScale.x/2 - 0.3f; 
            newText.transform.position = new Vector3( currPlanetObj.transform.position.x, newYValue, currPlanetObj.transform.position.z);
            
        }

    }

    // function called by UI planets to toggle visibilty of planets, updates active lists
    // param: int n of planet index 
    public void updateActive(int n){
        //planet is active, toggle it off
        if(activePlanetIndices.Contains(n)){
            int index = activePlanetIndices.IndexOf(n);
            activePlanetIndices.RemoveAt(index);
            GameObject toBeDeleted = activeObjects[index];
            activeObjects.RemoveAt((index));
            Destroy(toBeDeleted);
            sum-=planetRadius[n];
        }else{
            //planet is not active, turn it on
            activePlanetIndices.Insert( activePlanetIndices.Count,n);
            activePlanetIndices.Sort();
            //insert planet object at new loccation in planet order
            activeObjects.Insert(activePlanetIndices.IndexOf(n), Instantiate(planetPrefabs[n]));
            sum+=planetRadius[n];
        }

    }
}
