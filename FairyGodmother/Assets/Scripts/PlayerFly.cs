using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//This script goes onto the Player object
public class PlayerFly : MonoBehaviour {

    //public Animator anim;
    private Rigidbody rb;
    public float flyForce = 20f;
    public bool isAlive = true;
    bool canFly = false;
    public static bool flyEnergyEnough = true;
    public float groundPos = 0;
    public float jumpForce;

    //SoundFX:
    //public AudioSource[] flySFXs;

    void Start(){
        //anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (flyEnergyEnough){ canFly = true; }
        else { canFly = false; }

        if ((Input.GetButtonDown("Jump")) && (canFly) && (isAlive)) {
            Fly();
        }
    }

    public void Fly(){
    //move up:
        rb.linearVelocity = Vector2.up * (jumpForce/2);

    //play sound:
    /*
        int flyNum = Random.Range(0,flySFXs.Length);
        if (!flySFX.isPlaying){
            flySFXs[flyNum].Play();
        }
    */
         //anim.SetBool("Fly", true);

        //manage dropping:
        StopCoroutine(BirdDrop());
        StartCoroutine(BirdDrop());
    }

    IEnumerator BirdDrop(){
        //yield return new WaitForSeconds(0.5f);
        //rb.linearVelocity = Vector2.up * (flyForce/3);
       // yield return new WaitForSeconds(0.5f);
        //rb.linearVelocity = Vector2.up * (flyForce/4);
        //yield return new WaitForSeconds(0.5f);
        //rb.linearVelocity = Vector2.up * (flyForce/5);
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = Vector2.up * (flyForce/5);

        //Until you have a recharge timer:
        yield return new WaitForSeconds(1f);
        canFly = false;
        yield return new WaitForSeconds(2f);
        canFly = true;
    }

}