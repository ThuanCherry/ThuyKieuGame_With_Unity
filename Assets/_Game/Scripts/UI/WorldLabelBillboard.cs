using UnityEngine;

namespace ThuyKieu.UI
{
    public class WorldLabelBillboard : MonoBehaviour
    {
        private Renderer _renderer;
        private void Awake() { _renderer = GetComponent<Renderer>(); }
        private void LateUpdate()
        {
            if (Camera.main == null) return;
            transform.rotation = Camera.main.transform.rotation;
            if (_renderer != null) _renderer.enabled = Vector3.Distance(transform.position, Camera.main.transform.position) > 3;
        }
    }
}
