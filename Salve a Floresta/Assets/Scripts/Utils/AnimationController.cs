using UnityEngine;
using DragonBones;

public class AnimationController : MonoBehaviour
{
    [SerializeField] UnityArmatureComponent armatureComponent; // Referência para o componente UnityArmatureComponent da armadura

    private void Start()
    {
        PlayAnimation("Idle"); // Inicia a animação "idle" no início do jogo
    }

    public void PlayAnimation(string animationName)
    {
        if (armatureComponent.animation.lastAnimationName != null && armatureComponent.animation.lastAnimationName != animationName)
        {
            armatureComponent.animation.Play(animationName);
        }
    }

    public void StopAnimation(string animationName)
    {
        armatureComponent.animation.Stop(animationName);
    }
}
