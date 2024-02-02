using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Roll : StateMachineBehaviour
{
    BasePlayerController basePlayerController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        basePlayerController.ChangePlayerState("rolling", false);
        basePlayerController.ChangePlayerState("busy", false);
    }
}
