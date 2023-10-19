using UnityEngine;
using UnityEditor.Animations; // Import the UnityEditor.Animations namespace

public class AnimatorStateNames : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // Check if an Animator component is found
        if (animator != null)
        {
            // Get the AnimatorController
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller != null)
            {
                // Iterate through the layers and state machines to retrieve the names of all animation states
                foreach (AnimatorControllerLayer layer in controller.layers)
                {
                    foreach (ChildAnimatorStateMachine stateMachine in layer.stateMachine.stateMachines)
                    {
                        foreach (ChildAnimatorState state in stateMachine.stateMachine.states)
                        {
                            Debug.Log("Animation State Name: " + state.state.name);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
        }
    }
}
