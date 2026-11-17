using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Level3Handler : MonoBehaviour
{
    public GameObject level3Object;
    private string level3ItemId = "level3finish";
    public GameObject level3End;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevel3();
    }

    void CheckLevel3()
    {
        if (SceneManager.GetActiveScene().name != "Level3_Khrys") return;

        if (GameInventory.HasItem(level3ItemId))
            level3End.SetActive(true);
    }
}
