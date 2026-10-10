using UnityEngine;

namespace ThuyKieu.Dialogue
{
    [RequireComponent(typeof(Animator))]
    public class Chapter01HandPose : MonoBehaviour
    {
        private Animator _animator;
        public Transform Target { get; set; }
        public AvatarIKGoal Hand { get; set; }
        public float Weight { get; set; }
        public Transform LookAtTarget { get; set; }
        private void Awake() => _animator = GetComponent<Animator>();
        private void OnAnimatorIK(int layerIndex)
        {
            if (LookAtTarget != null && Weight > 0)
            {
                _animator.SetLookAtWeight(Weight * .8f, .65f, .7f, 0, .5f);
                _animator.SetLookAtPosition(LookAtTarget.position);
            }
            if (Target == null || Weight <= 0) return;
            _animator.SetIKPositionWeight(Hand, Weight);
            _animator.SetIKPosition(Hand, Target.position);
        }
    }
}
