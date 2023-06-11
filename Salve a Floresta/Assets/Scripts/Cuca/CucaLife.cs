using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaLife : MonoBehaviour
{

    [Header("Life Settings")]
    public float immunityDuration = 5f; // Duração da imunidade em segundos
    public bool isImmune = false; 
    public int maxHealth = 10; // Pontos de vida máximos da cuca
    public int currentHealth; // Pontos de vida atuais da cuca
    public bool isDead = false; // Flag para verificar se a cuca foi derrotado


    void Start()
    {
        
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

        currentHealth -= damage;

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
        // Executar ações quando o chefe é derrotado, como tocar uma animação, desativar colisores, etc.

        // Exemplo: Destruir o objeto do chefe após 2 segundos
        Destroy(gameObject, 2f);
    }
}
