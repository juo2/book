﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

namespace XGUI
{
    public class XButton : Button, XIEvent
    {

        public enum ToggleTransition
        {
            None,
            Fade
        }

        [System.Serializable]
        public class OnSelectEvent : UnityEvent { }
        public class OnPointDownEvent : UnityEvent { }
        /// <summary>
        /// Transition type.
        /// </summary>
        public ToggleTransition toggleTransition = ToggleTransition.Fade;

        [SerializeField]
        private Graphic m_SelectedGraphic;
        public UnityEngine.UI.Graphic selectedGraphic
        {
            get { return m_SelectedGraphic; }
            set
            {
                m_SelectedGraphic = value;
                PlayEffect(true);
            }
        }


        [SerializeField]
        private GameObject m_SelectedGameObject;
        public UnityEngine.GameObject selectedGameObject
        {
            get { return m_SelectedGameObject; }
            set { m_SelectedGameObject = value; }
        }

        [SerializeField]
        private GameObject m_UnSelectedGameObject;
        public UnityEngine.GameObject unSelectedGameObject
        {
            get { return m_UnSelectedGameObject; }
            set { m_UnSelectedGameObject = value; }
        }

        private Dictionary<int, Color> m_cacheColors;

        [SerializeField]
        private bool m_IsSelectChangeColor = false;
        public bool isSelectChangeColor
        {
            get { return m_IsSelectChangeColor; }
            set { Set(value); }
        }

        [SerializeField]
        private Color m_SelectColor = new Color(0.38f, 0.18f, 0.02f, 1);
        public Color SelectColor
        {
            get { return SelectColor; }
            set { SelectColor = value; }
        }

        [SerializeField]
        private bool m_IsSelected;

        public OnSelectEvent onSelect = new OnSelectEvent();
        public OnPointDownEvent onPointDown = new OnPointDownEvent();
        public bool isSelected
        {
            get { return m_IsSelected; }
            set { Set(value); }
        }

        [SerializeField]
        private XButtonGroup m_Group;
        public XButtonGroup group
        {
            get { return m_Group; }
            set
            {
                m_Group = value;
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    SetToggleGroup(m_Group, true);
                    PlayEffect(true);
                }
            }
        }

        [SerializeField]
        private bool m_EnableSelect = true;
        public bool enableSelect
        {
            get { return m_EnableSelect; }
            set { m_EnableSelect = value; }
        }


        [SerializeField] private Text m_LabelText;

        public UnityEngine.UI.Text labelText { get { return m_LabelText; } set { m_LabelText = value; } }


        public string label
        {
            get
            {

                 if (m_LabelText == null)
                    m_LabelText = (Text)transform.FindComponent(typeof(Text), "Text");
                return m_LabelText != null ? m_LabelText.text : null;

            }
            set
            {
                if (m_LabelText == null)
                    m_LabelText = (Text)transform.FindComponent(typeof(Text), "Text");
                if (m_LabelText != null)
                    m_LabelText.text = value;
            }
        }

        [SerializeField]
        private XImage m_BtnImage;
        public XImage BtnImage { get { return m_BtnImage; } set { m_BtnImage = value; } }
        public string spriteAssetName
        {
            get
            {
                if (m_BtnImage == null)
                    m_BtnImage = (XImage)transform.FindComponent(typeof(XImage));
                return m_BtnImage != null ? m_BtnImage.spriteAssetName : null;
            }
            set
            {
                if (m_BtnImage == null)
                    m_BtnImage = (XImage)transform.FindComponent(typeof(XImage));
                if (m_BtnImage != null)
                    m_BtnImage.spriteAssetName = value;
            }
        }

        [SerializeField]
        bool m_IsHasCD = false;
        [SerializeField]
        bool m_IsSlectImgScale = false;

        [SerializeField]
        bool m_ZoomSelectGameObject = false;

        [SerializeField]
        float m_CDSecond = 0.2f;
        int m_timerId = 0;
        bool m_IsCanClick = true;

        protected override void Awake()
        {
            base.Awake(); 
            InitText();
        }

        protected override void Start()
        {
            PlayEffect(true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetToggleGroup(m_Group, false);
            PlayEffect(true);
        }

        protected override void OnDisable()
        {
            SetToggleGroup(null, false);
            base.OnDisable();
        }

        private void SetToggleGroup(XButtonGroup newGroup, bool setMemberValue)
        {
            XButtonGroup oldGroup = m_Group;


            if (m_Group != null)
                m_Group.UnregisterToggle(this);


            if (setMemberValue)
                m_Group = newGroup;


            if (newGroup != null && IsActive())
                newGroup.RegisterToggle(this);


            if (newGroup != null && newGroup != oldGroup && isSelected && IsActive())
                newGroup.NotifyToggleOn(this);
        }

        private void PlayEffect(bool instant)
        {
            if (m_IsSlectImgScale && isSelected && m_SelectedGameObject != null)
            {
                m_SelectedGameObject.transform.localScale = new Vector3(1f, 0f, 1f);
                m_SelectedGameObject.transform.DOScale(Vector3.one, 0.13f);
            }

            if (m_SelectedGameObject != null)
            {
                if (m_ZoomSelectGameObject)
                {
                    m_SelectedGameObject.SetActive(true);
                    m_SelectedGameObject.transform.localScale = isSelected ? Vector3.one : Vector3.zero;
                }
                else
                {
                    m_SelectedGameObject.SetActive(isSelected);
                }
            }

            if (m_UnSelectedGameObject != null)
            {
                if (m_ZoomSelectGameObject)
                {
                    m_UnSelectedGameObject.SetActive(true);
                    m_UnSelectedGameObject.transform.localScale = !isSelected ? Vector3.one : Vector3.zero;
                }
                else
                {
                    m_UnSelectedGameObject.SetActive(!isSelected);
                }
            }

            if(m_IsSelectChangeColor) SetTextColor();

            if (m_SelectedGraphic == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                m_SelectedGraphic.canvasRenderer.SetAlpha(isSelected ? 1f : 0f);
            else
#endif
                m_SelectedGraphic.CrossFadeAlpha(isSelected ? 1f : 0f, instant ? 0f : 0.1f, true);

        }

        private void InitText()
        {
            if (labelText == null)
            {
                return;
            }
            var Image = transform.GetComponent<Image>();
            if (Image == null || Image.sprite == null) return;
            var imgName = Image.sprite.name;

        }

        private void SetTextColor()
        {
            if (m_cacheColors == null) m_cacheColors = new Dictionary<int, Color>();

            Text[] texts = this.gameObject.GetComponentsInChildren<Text>();
            foreach (Text item in texts)
            {
                int id = item.GetInstanceID();
                if (isSelected)
                {
                    if (!m_cacheColors.ContainsKey(id))
                        m_cacheColors.Add(id, item.color);
                    item.color = m_SelectColor;
                }
                else
                {
                    Color color = Color.white;
                    if (m_cacheColors.TryGetValue(id, out color))
                        item.color = color;
                }
            }
        }

        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (m_IsHasCD)
            {
                if (m_IsCanClick)
                {
                    m_timerId = TimerManager.AddTimer(() =>
                    {
                        m_IsCanClick = true;
                        TimerManager.DelTimer(m_timerId);
                    }, m_CDSecond);
                    m_IsCanClick = false;
                    OnPointEvent(eventData);
                }
            }
            else
            {
                OnPointEvent(eventData);
            }
        }

        private void OnPointEvent(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                InternalToggle();
            }
            base.OnPointerClick(eventData);
        }

        private void InternalToggle()
        {
            if (!IsActive() || !IsInteractable() || isSelected)
                return;

            if (!m_EnableSelect)
            {
                onSelect.Invoke();
                return;
            }

            isSelected = !isSelected;
        }

        void Set(bool value)
        {
            SetSelected(value, true);
        }

        public void SetSelected(bool value, bool sendCallback)
        {
            if (m_IsSelected == value)
                return;

            // if we are in a group and set to true, do group logic
            m_IsSelected = value;
            if (m_Group != null && IsActive())
            {
                if (m_IsSelected)
                {
                    m_IsSelected = true;
                    m_Group.NotifyToggleOn(this);
                    if (sendCallback)
                    {
                        UISystemProfilerApi.AddMarker("XButton.value", this);
                        onSelect.Invoke();
                    }
                }
            }

            PlayEffect(toggleTransition == ToggleTransition.None);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (onPointDown != null)
            {
                onPointDown.Invoke();
            }

        }

        public virtual void ClearEvent()
        {
            if (onClick != null)
            {
                onClick.RemoveAllListeners();
                onClick = null;
            }

            if (onSelect != null)
            {
                onSelect.RemoveAllListeners();
                onSelect = null;
            }

            if (onPointDown != null)
            {
                onPointDown.RemoveAllListeners();
                onPointDown = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.ClearEvent();

            m_cacheColors = null;
        }
    }
}
