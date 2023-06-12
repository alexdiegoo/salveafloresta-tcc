using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CucaLife : MonoBehaviour
{

    [Header("Life Settings")]
    public float immunityDuration = 5f; // Duração da imunidade em segundos
    public bool isImmune = false; 
    public int maxHealth = 10; // Pontos de vida máximos da cuca
    public int currentHealth; // Pontos de vida atuais da cuca
    public bool isDead = false; // Flag para verificar se a cuca foi derrotado

    private Material blinkMaterial;

    void Start()
    {
        blinkMaterial = GetComponentInChildren<MeshRenderer>().material;
    }

    void Update()
    {
        
    }

     // Função chamada quando o chefe é atingido
    public void TakeDamage(int damage)
    {
        Debug.Log(currentHealth);
        if (isDead)
            return;

    
        if(!isImmune)
        {
            StartCoroutine(ApplyImmunity());
            currentHealth -= damage;
        }

        // Verificar se o chefe foi derrotado
        if (currentHealth <= 0)
        {
            isDead = true;
            BossDefeated();
        }
    }

    // Função chamada quando o chefe é derrotado
    private void BossDefeated()
    {
        Invoke("LoadScene", 5f);

        if(GetComponent<CucaController>() != null)
        {
            GetComponent<CucaController>().enabled = false;
        }
    }

    void LoadScene()
    {
        WinGame();
        SceneManager.LoadScene("FinishCutscene");
    }

    public void WinGame()
    {
        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("CompletedLevel"))
        {

            PlayerPrefs.SetInt("CompletedLevel", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator ApplyImmunity()
    {
        isImmune = true;
        Debug.Log("Boss está imune");
        // Aqui você pode adicionar efeitos visuais ou lógica de imunidade,
        // como alterar a cor do jogador ou mostrar um escudo protetor.

        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f); 
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f); 

        isImmune = false;
        Debug.Log("Boss não está mais inume");

        yield return null;
    }
}
