using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGUI;

namespace XModules.Main
{
    public class RoleItem : MonoBehaviour
    {
        [SerializeField]
        XText nameLabel;

        [SerializeField]
        XInputField roleInput;

        [SerializeField]
        XButton addFaceBtn;

        [SerializeField]
        XImage faceImage;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Refresh(RoleData roleData)
        {
            nameLabel.text = roleData.name;
        }
    }
}
