using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    bool enter = false;
    bool doorIsOpened = false;

    private Animator animator;

    [SerializeField] private AudioClip buttonOpen;
    [SerializeField] private AudioClip buttonClose;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        OpenDoor();
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.F) && enter)
        {
            SoundManager.Instance.Play(buttonOpen);
            animator.SetBool("OpenDoor", true);
            doorIsOpened = true;
        }
    }

    public void CloseDoor()
    {
        if (Input.GetKeyDown(KeyCode.G) && enter)
        {
            SoundManager.Instance.Play(buttonClose);
            animator.SetBool("OpenDoor", false);
            doorIsOpened = false;
        }
    }

    // Display a simple info message when player is inside the trigger area
    void OnGUI()
    {
        GUIStyle doorButton = new GUIStyle(GUI.skin.button);
        doorButton.fontSize = 50;
        doorButton.alignment = TextAnchor.MiddleCenter;

        GUI.backgroundColor = new Color(0, 0, 0, 0);

        if (enter && PauseMenu.GameIsPaused == false)
        {
            if (doorIsOpened == false)
            {
                GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height / 2 - 40, 720, 80), "Press F to open the door", doorButton);
            }

            else
            {
                GUI.Box(new Rect(Screen.width / 2 - 360, Screen.height / 2 - 40, 720, 80), "Press G to close the door", doorButton);
            }
            
        }
    }

    // Activate the Main function when Player enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    // Deactivate the Main function when Player exit the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}