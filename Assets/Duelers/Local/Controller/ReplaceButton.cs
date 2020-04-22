using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Controller
{
    public class ReplaceButton : MonoBehaviour
    {
        public Button.ButtonClickedEvent _buttonClickedEvent;

        private void Awake()
        {
            _buttonClickedEvent = GetComponent<Button>().onClick;
        }

        private void OnMouseUpAsButton()
        {
            _buttonClickedEvent.Invoke();
        }
    }
}