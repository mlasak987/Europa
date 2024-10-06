using UnityEngine;

public class AnimationOffseter : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Swimming", 0, Random.Range(0f, 1f));
    }
}
