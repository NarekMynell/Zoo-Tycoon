using UnityEngine;

[ExecuteAlways]
public class WidthSeterByChildren : MonoBehaviour
{
    [SerializeField] private float _minWidth = 0f;
    [SerializeField] private float _delta;

    void Update()
    {
        float totalWidth = _delta;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                RectTransform childRect = child as RectTransform;
                if (childRect != null)
                {
                    totalWidth += childRect.rect.width;
                }
            }
        }
        if(totalWidth < _minWidth) totalWidth = _minWidth;
        (transform as RectTransform).sizeDelta = new (totalWidth, (transform as RectTransform).rect.height);
    }
}
