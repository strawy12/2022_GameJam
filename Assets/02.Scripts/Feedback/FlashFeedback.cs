using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FlashFeedback : Feedback
{
    [SerializeField] private GameObject flashObject;
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private Color color = Color.red;
    private bool _isHitFlash = false;
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        _spriteRenderers = flashObject.GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public override void CompletePrevFeedBack()
    {
        StopAllCoroutines();
    }

    public override void CreateFeedBack()
    {
        if (_isHitFlash) return;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {

        Color[] originColors = new Color[_spriteRenderers.Count];
        for (int i = 0; i < 5; i++)
        {
            int j = 0;
           foreach(SpriteRenderer sprite in _spriteRenderers)
            {
                originColors[j++] = sprite.color;
                sprite.color = color;
            }
            yield return new WaitForSeconds(0.1f);
            j = 0;
            foreach (SpriteRenderer sprite in _spriteRenderers)
            {
                sprite.color = originColors[j++];
            }
        }
    }
}
