using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGUI;

namespace XModules.Main
{
    public class MainItem : MonoBehaviour
    {
        [SerializeField]
        XButton closeBtn;

        [SerializeField]
        XImage image;

        [SerializeField]
        XText label;

        [SerializeField]
        MainView mainView;

        MainData mainData;

        // Start is called before the first frame update
        void Start()
        {
            closeBtn.onClick.AddListener(() => 
            {
                mainView.mainDatas.Remove(mainData);
                mainView.RefreshList();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Refresh(MainData _mainData)
        {
            mainData = _mainData;

            label.text = mainData.title;
            image.imageUrl = mainData.imageUrl;
        }
    }
}
