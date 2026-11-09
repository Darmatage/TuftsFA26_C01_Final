using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Transform[] elements = new Transform[5];
    public GameObject buttonJump;
    public GameObject platformActivate;

    public Animator platformAnimator;

    private bool nearJumpButton = false;
    private bool nearPlatformButton = false;
    private bool platformActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform element in elements)
        {
            if (element != null)
            {
                Animator anim = element.GetComponent<Animator>();

                if (anim != null)
                {
                    anim.enabled = false;
                }
            }
        }

        // Disable the other platform animation initially
        if (platformAnimator != null)
        {
            platformAnimator.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Jump button interaction
            if (nearJumpButton)
            {
                foreach (Transform element in elements)
                {
                    if (element != null)
                    {
                        Animator anim = element.GetComponent<Animator>();

                        if (anim != null)
                        {
                            anim.enabled = true;
                        }
                    }
                }
            }

            // Platform button interaction (only once)
            if (nearPlatformButton && !platformActivated)
            {
                if (platformAnimator != null)
                {
                    platformActivated = true;

                    platformAnimator.enabled = true;
                    platformAnimator.Play("PlatformMove", 0, 0f);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered: " + other.name);

        if (other.transform == buttonJump.transform ||
            other.transform.IsChildOf(buttonJump.transform))
        {
            nearJumpButton = true;
            Debug.Log("Near jump button!");
        }

        if (other.transform == platformActivate.transform ||
            other.transform.IsChildOf(platformActivate.transform))
        {
            nearPlatformButton = true;
            Debug.Log("Near platform button!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == buttonJump.transform ||
            other.transform.IsChildOf(buttonJump.transform))
        {
            nearJumpButton = false;
        }

        if (other.transform == platformActivate.transform ||
            other.transform.IsChildOf(platformActivate.transform))
        {
            nearPlatformButton = false;
        }
    }
}
