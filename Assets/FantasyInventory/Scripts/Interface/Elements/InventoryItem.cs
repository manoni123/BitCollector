﻿using System;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents inventory item and handles drag & drop operations.
    /// </summary>
    public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Image Frame;
        public Text Count;
        public Item Item;
        public PauseManager pauseManager;
        public string Group;

        private GameObject _phantom;
        private RectTransform _rect;
        private float _clickTime;

        public static InventoryItem DragTarget;
        public static Action<Item> OnDoubleClick;
        public static Action<Item> OnDragStarted;
        public static Action<Item> OnDragCompleted;
        public static Action<Item> OnItemSelectShop;
        public static Action<Item> OnItemSelected;

        public void Start()
        {
            Icon.sprite = ImageCollection.Instance.GetIcon(Item.Id);
            pauseManager = FindObjectOfType<PauseManager>();
        }

        /// <summary>
        /// Called from button script
        /// </summary>
        public void OnPress()
        {
            if (pauseManager.shopShown)
            {
                if (OnItemSelectShop != null)
                {
                    OnItemSelectShop(Item);
                }
            }
            else
            {
                OnItemSelected(Item);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (OnDoubleClick != null && Mathf.Abs(eventData.clickTime - _clickTime) < 0.5f) // If double click
            {
                OnDoubleClick(Item);
            }

            _clickTime = eventData.clickTime;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (DragTarget != null || Item.Params.Tags.Contains(ItemTag.NotForSale)) return;

            var canvas = FindInParents<Canvas>(gameObject);

            _phantom = Instantiate(gameObject);
            _phantom.transform.SetParent(canvas.transform, true);
            _phantom.transform.SetAsLastSibling();
            _phantom.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
            _phantom.GetComponent<CanvasGroup>().blocksRaycasts = false;
            _phantom.GetComponent<InventoryItem>().Count.text = "1";
            _rect = canvas.GetComponent<RectTransform>();
            SetDraggedPosition(eventData);
            DragTarget = this;

            if (OnDragStarted != null)
            {
                OnDragStarted(Item);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            if (DragReceiver.DropReady && OnDragCompleted != null)
            {
                OnDragCompleted(Item);
            }

            DragTarget = null;
            DragReceiver.DropReady = false;
            Destroy(_phantom, 0.25f);

            foreach (var graphic in _phantom.GetComponentsInChildren<Graphic>())
            {
                graphic.CrossFadeAlpha(0f, 0.25f, true);
            }
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            Vector3 mouse;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, data.position, data.pressEventCamera, out mouse))
            {
                var rect = _phantom.GetComponent<RectTransform>();

                rect.position = mouse;
                rect.rotation = _rect.rotation;
            }
        }

        private static T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;

            var comp = go.GetComponent<T>();

            if (comp != null) return comp;

            var t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }

            return comp;
        }
    }
}