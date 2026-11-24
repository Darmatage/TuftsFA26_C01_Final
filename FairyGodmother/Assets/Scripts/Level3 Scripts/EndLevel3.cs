using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel3 : MonoBehaviour
{
    public string sceneToLoad = "GameplayScene";   // must match the scene name exactly

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[EndLevel3] hit by '{collision.gameObject.name}' (tag: {collision.gameObject.tag})");

        if (!collision.gameObject.CompareTag("Player")) return;

        Debug.Log("loading");
        SceneManager.LoadScene(sceneToLoad);
    }
}
