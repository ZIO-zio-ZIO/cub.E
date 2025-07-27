using UnityEngine;

public class VictoryCanvasController : MonoBehaviour
{
    public CanvasGroup victoryTextGroup;
    public RectTransform whitePanel;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
