using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DelayedAction1 : MonoBehaviour {
       public float timer = 2f;       //set the number of seconds here
       private float theTimer = 0f;
       public bool startSearch = false;
       public TMP_Text timerDisplay;

      void Start(){
           theTimer = timer;
      }

      void Update(){
            if (Input.GetKeyDown("8")){
                  startSearch = true;
            }
      }

       void FixedUpdate(){
            if (startSearch == true){
                  theTimer -= 0.01f;
                  timer = theTimer;
                  UpdateTimer();
                  Debug.Log("time: " + theTimer);
                  if (theTimer <= 0){
                        theTimer = timer;
                        Debug.Log("I do the thing!");       //can be replaced with the desired commands
                        timerDisplay.text= "time's up";
                        startSearch = false;
                    }
              }
       }

       public void UpdateTimer(){
            string timeRemain = timer.ToString("0.0");
            timerDisplay.text = "remaining: " + timeRemain;
       }
}



//Find ingredients before time runs out! \n