using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>One-shot gestures return to neutral speech without repeating while a line stays on screen.</summary>
    public class DialogueGestureReturn : StateMachineBehaviour
    {
        [SerializeField] private int _emotion;
        [SerializeField] private bool _finishStanding;
        public void Configure(int emotion, bool finishStanding = false) { _emotion = emotion; _finishStanding = finishStanding; }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_finishStanding) animator.SetBool("IsSitting", false);
            if (_emotion > 0 && animator.GetInteger("DialogueEmotion") == _emotion)
                animator.SetInteger("DialogueEmotion", 0);
        }
    }
}
