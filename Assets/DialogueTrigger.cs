using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject buttonYes;
    [SerializeField] private GameObject buttonNo;

    private bool isPlayerInTrigger = false;
    private bool isPlayerOnDialogue = false;
    private Walk playerMovement;

    void Start()
    {
        dialogueBox.SetActive(false);
        buttonNo.SetActive(false);
        buttonYes.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetMouseButtonDown(0) && !isPlayerOnDialogue)
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
        dialogueBox.SetActive(true);
        buttonNo.SetActive(true);
        buttonYes.SetActive(true);
        playerMovement.canMove = false;
        isPlayerOnDialogue = true;
    }

    void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        buttonNo.SetActive(false);
        buttonYes.SetActive(false);
        playerMovement.canMove = true;
        isPlayerOnDialogue = false;
    }

    public void OnYesButtonPressed()
    {
        Debug.Log("Вы нажали 'Да'");
        CloseDialogue();
    }

    public void OnNoButtonPressed()
    {
        Debug.Log("Вы нажали 'Нет'");
        CloseDialogue();
    }

}
