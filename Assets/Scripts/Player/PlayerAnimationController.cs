using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerBrain playerBrain;
    private bool wasMovingBackward;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerBrain = FindFirstObjectByType<PlayerBrain>();
    }

    private void Update()
    {

    }
}