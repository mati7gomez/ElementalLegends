using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_FinishingParry : StateMachineBehaviour
{
    private BasePlayerController basePlayerController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
        basePlayerController.ChangePlayerState("finishingparry", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController.ChangePlayerState("finishingparry", false);
    }
}
