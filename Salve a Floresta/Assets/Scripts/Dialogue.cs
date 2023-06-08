using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public Text nameNpcText;
    public Image imageNpc;
    public Sprite spriteNpc;
    public string nameNpc;

    public bool readyToSpeak;
    public bool startDialogue;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject objectDialogue;
    private float playerSpeed;
    private bool dialogueCompleted = false;


    void Start()
    {
        dialoguePanel.SetActive(false);
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
        else if (dialogueText.text == dialogueNpc[dialogueIndex])
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
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            player.GetComponent<Player>().speed = playerSpeed;
        }
    }

    public void StartDialogue()
    {
        nameNpcText.text = nameNpc;
        imageNpc.sprite = spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text  += letter;
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
