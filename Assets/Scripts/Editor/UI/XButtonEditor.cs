﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using XGUI;
using UnityEditor.UI;

[CustomEditor(typeof(XButton), true)]
[CanEditMultipleObjects]
public class XButtonEditor : ButtonEditor
{
    SerializedProperty m_LabelText;
    //SerializedProperty m_LabelTextTMP;
    //SerializedProperty m_HotSpot;
    SerializedProperty m_SelectedGraphic;
    SerializedProperty m_SelectedGameObject;
    SerializedProperty m_IsSelectChangeColor;
    SerializedProperty m_SelectColor;
    SerializedProperty m_UnSelectedGameObject;
    SerializedProperty m_IsSelected;
    SerializedProperty m_Group;
    SerializedProperty onSelect;
    SerializedProperty m_IsHasCD;
    SerializedProperty m_IsSlectImgScale;
    SerializedProperty m_CD;
    SerializedProperty m_ZoomSelectGameObject;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        m_LabelText = serializedObject.FindProperty("m_LabelText");
        //m_LabelTextTMP = serializedObject.FindProperty("m_LabelTextTMP");
        //m_HotSpot = serializedObject.FindProperty("m_HotSpot");
        m_SelectedGraphic = serializedObject.FindProperty("m_SelectedGraphic");
        m_SelectedGameObject = serializedObject.FindProperty("m_SelectedGameObject");        
        m_UnSelectedGameObject = serializedObject.FindProperty("m_UnSelectedGameObject"); 
        m_IsSelected = serializedObject.FindProperty("m_IsSelected");
        m_Group = serializedObject.FindProperty("m_Group");
        onSelect = serializedObject.FindProperty("onSelect");
        m_IsHasCD = serializedObject.FindProperty("m_IsHasCD");
        m_CD = serializedObject.FindProperty("m_CDSecond");
        m_IsSlectImgScale = serializedObject.FindProperty("m_IsSlectImgScale");
        m_IsSelectChangeColor = serializedObject.FindProperty("m_IsSelectChangeColor");
        m_SelectColor = serializedObject.FindProperty("m_SelectColor");
        m_ZoomSelectGameObject = serializedObject.FindProperty("m_ZoomSelectGameObject");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_LabelText);
        //EditorGUILayout.PropertyField(m_LabelTextTMP);
        //EditorGUILayout.PropertyField(m_HotSpot);
        EditorGUILayout.PropertyField(m_IsSelected);        

        EditorGUILayout.PropertyField(m_SelectedGraphic); 
        EditorGUILayout.PropertyField(m_SelectedGameObject);        
        EditorGUILayout.PropertyField(m_UnSelectedGameObject);
        EditorGUILayout.PropertyField(m_Group);

        EditorGUILayout.PropertyField(m_IsHasCD);
        bool isHasCD = m_IsHasCD.boolValue;
        if (isHasCD)
        {
            EditorGUILayout.PropertyField(m_CD);
        }
        EditorGUILayout.PropertyField(m_IsSlectImgScale);
        EditorGUILayout.PropertyField(m_ZoomSelectGameObject);
        EditorGUILayout.PropertyField(m_IsSelectChangeColor);
        bool isSelect = m_IsSelectChangeColor.boolValue;
        if (isSelect)
        {
            EditorGUILayout.PropertyField(m_SelectColor);
        }

        EditorGUILayout.PropertyField(onSelect);
        serializedObject.ApplyModifiedProperties();
    }
}
