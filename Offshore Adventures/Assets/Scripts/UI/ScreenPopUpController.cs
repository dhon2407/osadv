using UnityEngine;
using System.Collections;

public class ScreenPopUpController : MonoBehaviour
{
    private static ScreenPopUpController @this;

    [SerializeField]
    private CanvasGroup hud = null;
    private UI.MessageBox messageBox;

    private void Awake()
    {
        @this = this;
        messageBox = GetComponentInChildren<UI.MessageBox>();
    }

    private void Start()
    {
        Show("Captain, package is already loaded, we need to deliver it to a port nearby.");
    }

    public static void Show(string message)
    {
        @this.gameObject.SetActive(true);
        @this.messageBox.Show(message);
        @this.hud.alpha = 0;
    }

    public void Hide()
    {
        hud.alpha = 1;
        gameObject.SetActive(false);
    }

    public void BackgroundTap()
    {
        Hide();
    }
}
