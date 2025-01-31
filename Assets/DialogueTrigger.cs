using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    private bool isPlayerInTrigger = false;
    private Walk playerMovement;

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetMouseButtonDown(0))
        {
            ActiveDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            playerMovement = other.GetComponent<Walk>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            dialogueBox.SetActive(false);
        }
    }

    void ActiveDialogue()
    {
        dialogueBox.SetActive(!dialogueBox.activeSelf);

        if (dialogueBox.activeSelf)
        {
            playerMovement.canMove = false;
        }
        else
        {
            playerMovement.canMove = true;
        }
    }

}
