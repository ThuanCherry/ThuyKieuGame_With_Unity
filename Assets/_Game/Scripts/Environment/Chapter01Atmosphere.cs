using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Environment
{
    public sealed class Chapter01Atmosphere : MonoBehaviour
    {
        [SerializeField] private Chapter01Director _director;
        [SerializeField] private Light _hall;
        [SerializeField] private Light _entrance;
        [SerializeField] private Light _contract;
        private Vector3 _target = new Vector3(4.2f, 0.8f, 2.4f);
        public string Moment { get; private set; } = "Opening";

        private void OnEnable() { if (_director != null) _director.CueReceived += OnCue; }
        private void OnDisable() { if (_director != null) _director.CueReceived -= OnCue; }
        private void OnCue(string key, string value)
        {
            if (key != "camera") return;
            switch (value)
            {
                case "WS_VuongGia": Moment = "Opening"; _target = new Vector3(4.2f, .8f, 2.4f); break;
                case "Reveal_MaGiamSinh": Moment = "Entrance"; _target = new Vector3(4.8f, 2.4f, 2.8f); break;
                case "Table_Contract": Moment = "Contract"; _target = new Vector3(4, 1.2f, 5.2f); break;
                case "Hero_Kieu_Rain": Moment = "Ending"; _target = new Vector3(3.8f, 1, 2.5f); break;
            }
        }
        private void Update()
        {
            float blend = 1 - Mathf.Exp(-2 * Time.deltaTime);
            if (_hall != null) _hall.intensity = Mathf.Lerp(_hall.intensity, _target.x, blend);
            if (_entrance != null) _entrance.intensity = Mathf.Lerp(_entrance.intensity, _target.y, blend);
            if (_contract != null) _contract.intensity = Mathf.Lerp(_contract.intensity, _target.z, blend);
        }
    }
}
