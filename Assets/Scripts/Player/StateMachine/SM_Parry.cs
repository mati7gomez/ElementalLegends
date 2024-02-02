using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Parry : StateMachineBehaviour
{
    private BasePlayerController basePlayerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController.ChangePlayerState("busy", false);
        basePlayerController.ChangePlayerState("parrying", false);
    }
}
