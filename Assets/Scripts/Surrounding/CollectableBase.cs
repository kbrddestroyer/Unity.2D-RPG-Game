using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBase : MonoBehaviour, ICollectable
{
    [SerializeField] private Sprite guiSprite;
    [SerializeField] private CollectableType type;
    [SerializeField, Range(0f, 10f)] private float buff;
    public Sprite sprite { get => guiSprite; }
    public CollectableType Type { get => type; }
    public float Buff { get => buff; }

    public virtual void Collect()
    {
        if (InventoryController.Instance.AddElement(this))
        {
            Destroy(this.GetComponent<SpriteRenderer>());
            this.transform.parent = InventoryController.Instance.transform;
            this.transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            Collect();
        }
    }
}
