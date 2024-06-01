using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGUI;

namespace XModules.Main
{
    public class CreateBookView : XBaseView
    {
        [SerializeField]
        XButton closeBtn;

        [SerializeField]
        XButton nextStep;

        [SerializeField]
        XInputField titleInput;

        [SerializeField]
        XInputField bookInput;

        // Start is called before the first frame update
        void Start()
        {
            closeBtn.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.CloseView("CreateBookView");
            });

            nextStep.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.CloseView("CreateBookView");
                XGUI.XGUIManager.Instance.OpenView("ProjectView");
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
