
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level3Order : MonoBehaviour
{
    public GameObject textObject;
    public TMP_Text dialogueText;

    private bool playerNearby = false;
    private int dialogueIndex = 0;

    void Start()
    {
        textObject.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            dialogueIndex = 0;

            textObject.SetActive(true);
            dialogueText.text = "I want to order something that you can find on Level 3.";
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.T))
        {
            if (dialogueIndex == 0)
            {
                dialogueText.text = "Gotcha!";
                dialogueIndex = 1;
            }
            else if (dialogueIndex == 1)
            {
                textObject.SetActive(false);
                SceneManager.LoadScene("GameplayScene");
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
            textObject.SetActive(false);
        }
    }
}
