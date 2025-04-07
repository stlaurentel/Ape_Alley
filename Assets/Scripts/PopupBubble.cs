using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PopupBubble : MonoBehaviour
{
    public float duration = 2f;
    public TextMeshProUGUI bubbleText;

    public void Show(string message)
    {
        bubbleText.text = message;
        StartCoroutine(DestroyAfterTime(duration));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < time)
        {
            transform.position = startPos + Vector3.up * (t / time);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

}
