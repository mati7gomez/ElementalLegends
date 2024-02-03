using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Atk1End : StateMachineBehaviour
{
    BasePlayerController basePlayerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
        basePlayerController.ChangePlayerState("finishingattack", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController.DisableCombo();
        basePlayerController.ChangePlayerState("finishingattack", false);
    }
}
