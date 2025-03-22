using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextNotification : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _fadeDuration = .4f;

    [Header("UI")]
    [SerializeField] private Image image;

    [SerializeField] private Sprite _okSprite;
    [SerializeField] private Sprite _goodSprite;
    [SerializeField] private Sprite _greatSprite;
    [SerializeField] private Sprite _excellentSprite;
    [SerializeField] private Sprite _missSprite;

    private void Awake()
    {
        image.enabled = false;
    }

    public void MakeNotification(RewardType rating)
    {
        switch (rating)
        {
            case RewardType.Ok:
                image.sprite = _okSprite;
                break;
            case RewardType.Good:
                image.sprite = _goodSprite;
                break;
            case RewardType.Great:
                image.sprite = _greatSprite;
                break;
            case RewardType.Excellent:
                image.sprite = _excellentSprite;
                break;
            case RewardType.None:
                image.sprite = _missSprite;
                break;
        }

        StartCoroutine(ShowNotification());
    }

    private IEnumerator ShowNotification()
    {
        image.enabled = true;

        float timer = 0f;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - timer / _fadeDuration);
            yield return null;
        }

        image.enabled = false;
    }
}
