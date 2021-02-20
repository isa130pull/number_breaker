using UnityEngine;

public class RandomBehaviour : StateMachineBehaviour
{
    private readonly int hashRandom = Animator.StringToHash("random");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetInteger(this.hashRandom, Random.Range(0, 7));
    }
}