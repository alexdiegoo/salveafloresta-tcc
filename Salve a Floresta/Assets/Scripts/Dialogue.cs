using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{

    public string skillKey;
    public PlayerAbilitiesController playerAbilitiesController;

    public string[] dialogueNpc;
    public int dialogueIndex;

    GameController gameController;
    public string nameNpc;
    public Sprite spriteNpc;

    public bool readyToSpeak;
    public bool startDialogue;

    private float playerSpeed;
    private bool dialogueCompleted = false;
    private bool textDisplayed = false; // Nova variável para controlar se o texto foi totalmente exibido

    [SerializeField] private GameObject player;
    [SerializeField] private float typingSpeed = 0.1f;


    void Start()
    {
        gameController = GameController.gameController;
        gameController.dialoguePanel.SetActive(false);
        playerSpeed = player.GetComponent<Player>().speed;

        dialogueCompleted = false;
        readyToSpeak = false;
        startDialogue = false;
        dialogueIndex = 0;

    }

    void Update()
    {
        if (readyToSpeak)
        {
            if (!startDialogue)
            {
                player.GetComponent<Player>().speed = 0f;
                player.GetComponent<Player>().inDialogue = true;
                StartDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (textDisplayed) // Verificar se o texto foi exibido completamente
                {
                    NextDialogue();
                }
                else
                {
                    // Exibir todo o texto imediatamente
                    StopAllCoroutines();
                    gameController.dialogueText.text = dialogueNpc[dialogueIndex];
                    textDisplayed = true;
                }
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

        if (dialogueIndex < dialogueNpc.Length)
        {
            if (textDisplayed) // Verificar se o texto foi exibido completamente
            {
                StartCoroutine(ShowDialogue(0f));
            }
            else
            {
                StartCoroutine(ShowDialogue(typingSpeed));
            }
            textDisplayed = false; // Redefinir a variável para o próximo diálogo
        }
        else
        {
            if(GetComponent<PlayerAbilitiesController>() != null)
            {
                playerAbilitiesController.ReleaseSkill(skillKey);
            }

            dialogueCompleted = true;
            readyToSpeak = false;
            gameController.dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
            player.GetComponent<Player>().speed = playerSpeed;
            player.GetComponent<Player>().inDialogue = false;
        }
    }

    public void StartDialogue()
    {
        gameController.nameNpcText.text = nameNpc;
        gameController.imageNpc.sprite = spriteNpc;
        startDialogue = true;
        dialogueIndex = 0;
        gameController.dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue(typingSpeed));
    }

    IEnumerator ShowDialogue(float speed)
    {
        gameController.dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            gameController.dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        textDisplayed = true; // Marcar que o texto foi exibido completamente
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !dialogueCompleted)
        {
            readyToSpeak = true;
        }
    }
}
