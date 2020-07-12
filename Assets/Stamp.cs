using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stamp : MonoBehaviour
{
    public Canvas canvas;
    public Collider2D table;
    public Image stampImage;
    public Collision collision;

    public GameObject stampPrefab;

    public void BeginDrag() {
        stampImage.rectTransform.anchoredPosition = new Vector2(0, 0.5f);
        SetToMouse();
    }

    public void Drag() {
        SetToMouse();
    }

    public void EndDrag() {
        stampImage.rectTransform.anchoredPosition = Vector2.zero;

        SetToMouse();

        if (collision.IsColliding) {
            // find highest page and stamp that one
            var others = collision.AllColliders;
            StampPageController topPage = null;

            foreach (var other in others) {
                StampPageController page = other.GetComponent<StampPageController>();
                if (page != null && (topPage == null || topPage.paper.canvas.sortingOrder < page.paper.canvas.sortingOrder)) {
                    topPage = page;
                }
            }
            
            if (topPage != null) {
                GameObject.Instantiate(stampPrefab, transform.position, Quaternion.identity, topPage.paper.canvasGroup.transform);
                topPage.Stamped();
            }
        }
    }

    private void SetToMouse() {
        transform.position = Utilities.ClampInRect(
            canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition).ToVector2(),
            new Rect(table.bounds.min, table.bounds.size)
        );
    }
}
