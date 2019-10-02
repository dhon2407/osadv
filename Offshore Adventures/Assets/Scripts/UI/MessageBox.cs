using UnityEngine;
using TMPro;

namespace UI
{
    public class MessageBox : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI message = null;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void Show(string message)
        {
            this.message.text = message;
            transform.localScale = Vector3.one;
        }
    }
}