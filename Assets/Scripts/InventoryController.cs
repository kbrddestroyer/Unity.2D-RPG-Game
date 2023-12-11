using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance { get => instance; }

    [SerializeField, Range(0, 128)] private int maxSize;
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private Image headItemGUI;
    [SerializeField] private Image bodyItemGUI;
    [SerializeField] private Image bootsItemGUI;
    [SerializeField] private Image weaponItemGUI;
    [SerializeField] private Image defenceItemGUI;
    [SerializeField] private Image healsItemGUI;
    [SerializeField] private TMP_Text healsCounter;

    private CollectableBase headItem;
    private CollectableBase bodyItem;
    private CollectableBase bootsItem;
    private CollectableBase weaponItem;
    private CollectableBase defenceItem;
    private CollectableBase healingItem;
    private int healsCount = 0;

    public float Defence
    {
        get
        {
            return (
                ((headItem) ? headItem.Buff : 1) *
                ((bodyItem) ? bodyItem.Buff : 1) *
                ((defenceItem) ? defenceItem.Buff : 1)
                );
        }
    }

    public float Speed
    {
        get => (bootsItem) ? bootsItem.Buff : 1;
    }

    public float Damage {
        get => (weaponItem) ? weaponItem.Buff : 1;
    } 

    private CollectableBase ChangeAttachedCollectable(CollectableBase _old, CollectableBase _new, Image gui)
    {
        if (_old) Destroy(_old.gameObject);
        gui.gameObject.SetActive(true);
        gui.sprite = _new.sprite;
        return _new;
    }

    public bool AddElement(CollectableBase collectable)
    {
        switch (collectable.Type)
        {
            case CollectableType.HEAD:
                {
                    if (!headItem || collectable.Buff > headItem.Buff)
                    {
                        headItem = ChangeAttachedCollectable(headItem, collectable, headItemGUI);
                    }
                    else return false;
                } break;
            case CollectableType.BODY:
                {
                    if (!bodyItem || collectable.Buff > bodyItem.Buff)
                    {
                        bodyItem = ChangeAttachedCollectable(bodyItem, collectable, bodyItemGUI);
                    }

                    else return false;
                }
                break;
            case CollectableType.BOOTS:
                {
                    if (!bootsItem || collectable.Buff > bootsItem.Buff)
                    {
                        bootsItem = ChangeAttachedCollectable(bootsItem, collectable, bootsItemGUI);
                    }

                    else return false;
                }
                break;
            case CollectableType.WEAPON:
                {
                    if (!weaponItem || collectable.Buff > weaponItem.Buff)
                    {
                        bootsItem = ChangeAttachedCollectable(weaponItem, collectable, weaponItemGUI);
                    }

                    else return false;
                }
                break;
            case CollectableType.DEFENCE:
                {
                    if (!defenceItem || collectable.Buff > defenceItem.Buff)
                    {
                        defenceItem = ChangeAttachedCollectable(defenceItem, collectable, defenceItemGUI);
                    }

                    else return false;
                }
                break;
            case CollectableType.HEAL:
                {
                    if (healsCount < 64)
                    {
                        healsCount++;
                        healsItemGUI.gameObject.SetActive(true);
                        healsItemGUI.sprite = collectable.sprite;
                        healsCounter.text = healsCount.ToString();
                    }
                    else return false;
                } break;
        }

        return true;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryGUI.SetActive(!inventoryGUI.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.H)) { 
            if (healsCount > 0)
            {
                PlayerController.Instance.HP += (int) healingItem.Buff;
                healsCount--;

                healsCounter.text = healsCount.ToString();
                if (healsCount <= 0)
                {
                    headItemGUI.gameObject.SetActive(false);
                }
            }
        }
    }
}
