using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XGUI;

namespace XModules.Main
{
    public class ProjectItem : MonoBehaviour
    {
        [SerializeField]
        XText numLabel;

        [SerializeField]
        XText descLabel;

        [SerializeField]
        XInputField promptInput;

        [SerializeField]
        XButton regenBtn;

        [SerializeField]
        XButton reDescBtn;

        [SerializeField]
        XImage image;

        [SerializeField]
        Dropdown characterDropdown;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Refresh(ProjectData projectData)
        {

        }
    }
}
