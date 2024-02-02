using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Hurt : StateMachineBehaviour
{
    private BasePlayerController basePlayerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
        basePlayerController.ChangePlayerState("busy", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        basePlayerController.ChangePlayerState("gettinghurt", false);
        basePlayerController.ChangePlayerState("busy", false);
    }
}
