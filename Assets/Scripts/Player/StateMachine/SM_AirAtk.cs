using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_AirAtk : StateMachineBehaviour
{
    private BasePlayerController basePlayerController;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        basePlayerController = animator.gameObject.GetComponent<BasePlayerController>();
        basePlayerController.ChangePlayerState("attacking", false);
        basePlayerController.ChangePlayerState("busy", false);
        basePlayerController.SetRigidBodyDynamic();

    }
}
