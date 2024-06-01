using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGUI;
using static XGUI.XListView;

namespace XModules.Main
{
    public class MainData
    {
        public string title;
        public string imageUrl;
    }


    public class MainView : XBaseView
    {
        [SerializeField]
        XListView xListView;

        [SerializeField]
        XButton btn;

        Dictionary<int, MainItem> itemDic = new Dictionary<int, MainItem>();

        public List<MainData> mainDatas = new List<MainData>();

        // Start is called before the first frame update
        void Start()
        {
            xListView.onCreateRenderer.AddListener(onListCreateRenderer);
            xListView.onUpdateRenderer.AddListener(onListUpdateRenderer);

            btn.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.OpenView("CreateBookView");
            });

            for(int i = 0;i < 8; i++)
            {
                MainData mainData = new MainData();
                mainData.title = "ÏîÄ¿1";
                mainData.imageUrl = "http://appcdn.calfchat.top/defaultDir/2024-05-13/03e1fd90f175413fbc2c91fc77617e7a.jpg";

                mainDatas.Add(mainData);
            }

            xListView.dataCount = mainDatas.Count;
            xListView.ForceRefresh();
        }

        public void RefreshList()
        {
            xListView.dataCount = mainDatas.Count;
            xListView.ForceRefresh();
        }

        void onListCreateRenderer(ListItemRenderer listItem)
        {
            //Debug.Log("GalManager_Choice onListCreateRenderer");
            MainItem mainItem = listItem.gameObject.GetComponent<MainItem>();
            itemDic[listItem.instanceID] = mainItem;

        }

        void onListUpdateRenderer(ListItemRenderer listItem)
        {
            MainItem mainItem = itemDic[listItem.instanceID];
            MainData mainData = mainDatas[listItem.index];
            mainItem.Refresh(mainData);
        }
    }
}



