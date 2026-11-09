
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsStarter : MonoBehaviour
{
    public GameObject level3Starter;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == level3Starter)
        {
            Debug.Log("loading level 3");
            SceneManager.LoadScene("Level3_Khrys");
        }
    }
}

