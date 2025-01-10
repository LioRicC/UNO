using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject currentLocation;
    public GameObject AnimationHandler;
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component from the skipAnimationHandler GameObject
        animator = AnimationHandler.GetComponent<Animator>();
    }

    public void PlayAnimationHere(string animationName)
    {
        // Move skipAnimationHandler to the position and rotation of currentLocation
        AnimationHandler.transform.position = currentLocation.transform.position;
        AnimationHandler.transform.rotation = currentLocation.transform.rotation;


        if (animationName == "skip")
        {
            animator.Play("skippanim", 0, 0.25f);
        }// Trigger the animation to play once
        
        if (animationName == "plus2")
        {
            animator.Play("+2anim", 0, 0.25f);
        }
        if (animationName == "plus4")
        {
            animator.Play("+4anim", 0, 0.25f);
        }

    }
}
