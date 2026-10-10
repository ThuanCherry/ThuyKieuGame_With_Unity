using ThuyKieu.Environment;
using UnityEngine;

namespace ThuyKieu.Interaction
{
    public sealed class ProximityDoor : MonoBehaviour
    {
        [SerializeField] private Transform _player, _left, _right;
        [SerializeField] private Chapter01Audio _audio;
        [SerializeField] private float _openDistance = 4.2f;
        [SerializeField] private bool _storyControlled;
        [SerializeField] private float _ajarAngle = 12;
        [SerializeField] private Collider _storyBlocker;
        private bool _open;
        private float _angle;
        public bool IsOpen => _angle > 85;
        public void SetStoryOpen(bool open)
        {
            _storyControlled = true;
            if (open == _open) return;
            _open = open;
            if (_storyBlocker != null) _storyBlocker.enabled = !open;
            if (_audio != null) _audio.PlayDoor(open, transform.position);
        }
        private void Update()
        {
            if (_player == null) return;
            Vector3 delta = _player.position - transform.position; delta.y = 0;
            bool open = _storyControlled ? _open : delta.sqrMagnitude < Mathf.Pow(_openDistance + (_open ? .8f : 0), 2);
            if (open != _open) { _open = open; if (_audio != null) _audio.PlayDoor(open, transform.position); }
            _angle = Mathf.MoveTowards(_angle, _open ? 100 : _storyControlled ? _ajarAngle : 0, 150 * Time.deltaTime);
            if (_left != null) _left.localRotation = Quaternion.Euler(0, _angle, 0);
            if (_right != null) _right.localRotation = Quaternion.Euler(0, -_angle, 0);
        }
    }
}
