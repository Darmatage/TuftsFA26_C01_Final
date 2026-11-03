using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour{

      public GameHandler gameHandler;
      public AudioSource SoundFX;
      //public playerVFX playerPowerupVFX;
      //public bool isHealthPickUp = true;
      //public bool isSpeedBoostPickUp = false;

      //public int healthBoost = 50;
      //public float speedBoost = 2f;
      //public float speedTime = 2f;

      void Start(){
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            SoundFX = GetComponent<AudioSource>();
            //playerPowerupVFX = GameObject.FindWithTag("Player").GetComponent<playerVFX>();
      }

      public void OnTriggerEnter (Collider other){
            if (other.gameObject.tag == "Player"){
                  GetComponent<Collider>().enabled = false;
                  //GetComponent< AudioSource>().Play();
                  StartCoroutine(DestroyThis());
/*
                  if (isHealthPickUp == true) {
                        gameHandler.playerGetHit(healthBoost * -1);
                        //playerPowerupVFX.powerup();
                  }

                  if (isSpeedBoostPickUp == true) {
                        other.gameObject.GetComponent<PlayerMove>().speedBoost(speedBoost, speedTime);
                        //playerPowerupVFX.powerup();
                  }
  */
            }
      }

      IEnumerator DestroyThis(){
            SoundFX.Play();
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
      }

}