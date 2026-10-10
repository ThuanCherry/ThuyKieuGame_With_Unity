using System.Collections;
using ThuyKieu.Dialogue;
using ThuyKieu.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThuyKieu.Core
{
    public class Chapter01ContractPresentation : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _player;
        [SerializeField] private Animator _playerAnimator, _maAnimator;
        [SerializeField] private Transform _ma, _paper, _tablePaperPose, _maPresentPose;
        [SerializeField, FormerlySerializedAs("_writingSeat")] private Transform _writingPosition;
        [SerializeField] private Chapter01HandPose _playerHand, _maHand;
        [SerializeField] private Transform _carryingHandTarget, _writingHandTarget;
        [SerializeField] private GameObject _brush;
        private bool _presented;
        public bool IsActing { get; private set; }
        public bool IsWriting { get; private set; }
        public bool Presented => _presented;

        private void Awake() { _paper.gameObject.SetActive(false); _brush.SetActive(false); }

        private void LateUpdate()
        {
            if (IsWriting)
            {
                Vector3 facing = _player.transform.position - _ma.position; facing.y = 0;
                if (facing.sqrMagnitude > .01f)
                    _ma.rotation = Quaternion.RotateTowards(_ma.rotation, Quaternion.LookRotation(facing), 180 * Time.deltaTime);
            }
            if (!IsWriting || !_brush.activeSelf) return;
            Vector3 hand = _playerAnimator.GetBoneTransform(HumanBodyBones.RightHand).position;
            Vector3 tip = _writingHandTarget.position - Vector3.up * .10f;
            Vector3 shaft = (hand - tip).normalized;
            _brush.transform.SetPositionAndRotation(tip + shaft * .13f, Quaternion.FromToRotation(Vector3.up, shaft));
        }

        public void Bow() { if (!IsActing) StartCoroutine(Greet()); }
        private IEnumerator Greet()
        {
            IsActing = true;
            _maHand.Weight = 0;
            _maAnimator.SetBool("ContractActing", true);
            _maAnimator.CrossFadeInFixedTime("Bowing", .18f, 0);
            yield return new WaitForSeconds(3.5f);
            _maAnimator.SetBool("ContractActing", false);
            _maAnimator.CrossFadeInFixedTime("Idle", .18f, 0);
            _maHand.Weight = 0;
            IsActing = false;
        }

        public void Present() { if (!_presented && !IsActing) StartCoroutine(PresentPaper()); }
        private IEnumerator PresentPaper()
        {
            IsActing = true;
            _maAnimator.SetBool("DialogueTalking", false);
            _maAnimator.SetBool("ContractActing", true);
            _maAnimator.CrossFadeInFixedTime("Idle", .15f, 0);
            float speed = 1.4f;
            while (Vector3.Distance(_ma.position, _maPresentPose.position) > .02f)
            {
                Vector3 direction = _maPresentPose.position - _ma.position; direction.y = 0;
                _ma.rotation = Quaternion.RotateTowards(_ma.rotation, Quaternion.LookRotation(direction), 240 * Time.deltaTime);
                _ma.position = Vector3.MoveTowards(_ma.position, _maPresentPose.position, speed * Time.deltaTime);
                _maAnimator.SetFloat("Speed", .5f);
                yield return null;
            }
            _maAnimator.SetFloat("Speed", 0);
            for (float t = 0; t < .3f; t += Time.deltaTime)
            { _ma.rotation = Quaternion.RotateTowards(_ma.rotation, _maPresentPose.rotation, 360 * Time.deltaTime); yield return null; }
            // Reach to the robe first. The document remains concealed until the hand arrives.
            _carryingHandTarget.localPosition = new Vector3(-.12f, 1.05f, .12f);
            _maHand.Target = _carryingHandTarget; _maHand.Hand = AvatarIKGoal.LeftHand;
            for (float t = 0; t < .35f; t += Time.deltaTime)
            { _maHand.Weight = Mathf.SmoothStep(0, 1, t / .35f); yield return null; }
            var hand = _maAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
            _paper.SetParent(hand, true);
            Quaternion uprightPaper = _ma.rotation * Quaternion.Euler(90, 0, 0);
            // Grip the rear edge, so the leading edge can reach the low table without
            // stretching the actor's arm to the centre of the sheet.
            _paper.SetPositionAndRotation(hand.position + uprightPaper * Vector3.forward * .23f, uprightPaper);
            _paper.gameObject.SetActive(true);
            _maHand.LookAtTarget = _tablePaperPose;
            Vector3 start = _carryingHandTarget.position;
            _maHand.Weight = 1;
            for (float t = 0; t < .65f; t += Time.deltaTime)
            {
                _carryingHandTarget.position = Vector3.Lerp(start, _tablePaperPose.position + Vector3.up * .04f, Mathf.SmoothStep(0, 1, t / .65f));
                Vector3 towardsTable = _tablePaperPose.position - hand.position; towardsTable.y = 0;
                Quaternion tilt = Quaternion.LookRotation(towardsTable) * Quaternion.Euler(Mathf.Lerp(90, 50, t / .65f), 0, 0);
                _paper.SetPositionAndRotation(hand.position + tilt * Vector3.forward * .23f, tilt);
                yield return null;
            }
            _paper.SetParent(_tablePaperPose.parent, true);
            Vector3 releasePosition = _paper.position;
            Quaternion releaseRotation = _paper.rotation;
            for (float t = 0; t < .3f; t += Time.deltaTime)
            {
                float settle = Mathf.SmoothStep(0, 1, t / .3f);
                _paper.SetPositionAndRotation(Vector3.Lerp(releasePosition, _tablePaperPose.position, settle),
                    Quaternion.Slerp(releaseRotation, _tablePaperPose.rotation, settle));
                yield return null;
            }
            _paper.SetPositionAndRotation(_tablePaperPose.position, _tablePaperPose.rotation);
            _presented = true;
            for (float t = 0; t < .3f; t += Time.deltaTime)
            { _maHand.Weight = 1 - t / .3f; yield return null; }
            _maHand.Weight = 0; _maHand.LookAtTarget = null;
            _maAnimator.SetBool("ContractActing", false);
            IsActing = false;
        }

        public void Sign() { if (_presented && !IsActing) StartCoroutine(SignPaper()); }
        private IEnumerator SignPaper()
        {
            IsActing = IsWriting = true;
            var capsule = _player.GetComponent<CharacterController>();
            Vector3 originalPosition = _player.transform.position;
            Quaternion originalRotation = _player.transform.rotation;
            bool capsuleEnabled = capsule.enabled;
            bool movementEnabled = _player.enabled;
            _player.PositionLocked = true; capsule.enabled = false;
            _player.enabled = false;
            _playerAnimator.SetBool("ContractActing", true);
            _playerAnimator.SetBool("DialogueTalking", false);
            _playerAnimator.CrossFadeInFixedTime("Idle", .15f, 0);
            Vector3 start = _player.transform.position;
            for (float t = 0; t < 1.2f; t += Time.deltaTime)
            {
                _player.transform.position = Vector3.Lerp(start, _writingPosition.position, t / 1.2f);
                _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, _writingPosition.rotation, 240 * Time.deltaTime);
                _playerAnimator.SetFloat("Speed", .5f);
                yield return null;
            }
            _player.transform.SetPositionAndRotation(_writingPosition.position, _writingPosition.rotation);
            _playerAnimator.SetFloat("Speed", 0);
            _playerAnimator.SetBool("IsSitting", false);
            int writingLayer = _playerAnimator.GetLayerIndex("Standing writing");
            _playerAnimator.SetLayerWeight(writingLayer, 0);
            _playerAnimator.Play("Writing", writingLayer, 0);
            _playerHand.Target = _writingHandTarget; _playerHand.Hand = AvatarIKGoal.RightHand;
            _playerHand.LookAtTarget = _writingHandTarget;
            _brush.SetActive(true);
            Vector3 writingTarget = _writingHandTarget.position;
            // A signature only needs a short passage of the authored writing clip.
            for (float t = 0; t < 2.8f; t += Time.deltaTime)
            {
                _playerAnimator.SetLayerWeight(writingLayer, Mathf.Clamp01(t / .2f));
                _writingHandTarget.position = writingTarget + new Vector3(Mathf.Sin(t * 8) * .012f, 0, Mathf.Sin(t * 5) * .008f);
                _playerHand.Weight = Mathf.Clamp01(t / .2f) * Mathf.Clamp01((2.8f - t) / .2f);
                yield return null;
            }
            _playerHand.Weight = 0; _playerHand.LookAtTarget = null; _brush.SetActive(false);
            _writingHandTarget.position = writingTarget;
            for (float t = 0; t < .25f; t += Time.deltaTime)
            { _playerAnimator.SetLayerWeight(writingLayer, 1 - t / .25f); yield return null; }
            _playerAnimator.SetLayerWeight(writingLayer, 0);
            for (float t = 0; t < .65f; t += Time.deltaTime)
            {
                _player.transform.position = Vector3.Lerp(_writingPosition.position, originalPosition, t / .65f);
                _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, originalRotation, 240 * Time.deltaTime);
                _playerAnimator.SetFloat("Speed", .5f);
                yield return null;
            }
            _player.transform.SetPositionAndRotation(originalPosition, originalRotation);
            _playerAnimator.SetFloat("Speed", 0);
            _playerAnimator.SetBool("ContractActing", false);
            capsule.enabled = capsuleEnabled; _player.PositionLocked = false;
            _player.enabled = movementEnabled;
            IsActing = IsWriting = false;
        }
    }
}
