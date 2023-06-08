using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    GameController gameController;
    public string nameNpc;

    public bool readyToSpeak;
    public bool startDialogue;

    private float playerSpeed;
    private bool dialogueCompleted = false;

    [SerializeField] private GameObject player;
    //[SerializeField] public GameObject objectDialogue;


    void Start()
    {
        gameController = GameController.gameController;
        gameController.dialoguePanel.SetActive(false);
        playerSpeed = player.GetComponent<Player>().speed;
    }

    void Update()
    {
        if (readyToSpeak)
        {
            if (!startDialogue)
            {
                player.GetComponent<Player>().speed = 0f;
                StartDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                NextDialogue();
            }
        }
        else if (gameController.dialogueText.text == dialogueNpc[dialogueIndex])
        {
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        dialogueIndex++;

        if(dialogueIndex < dialogueNpc.Length)
        {
            StartCoroutine(ShowDialogue());
        }
        else 
        {   
            dialogueCompleted = true;
            readyToSpeak = false;
            gameController.dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            player.GetComponent<Player>().speed = playerSpeed;
            //Destroy(objectDialogue);
        }
    }

    public void StartDialogue()
    {
        gameController.nameNpcText.text = nameNpc;
        gameController.imageNpc.sprite = gameController.spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        gameController.dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        gameController.dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            gameController.dialogueText.text  += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !dialogueCompleted)
        {
            readyToSpeak = true;
        }
    }
}
