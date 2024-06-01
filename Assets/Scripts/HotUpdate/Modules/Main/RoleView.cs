using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGUI;
using static XGUI.XListView;

namespace XModules.Main
{
    public class RoleData
    {
        public string name;
    }


    public class RoleView : XBaseView
    {
        [SerializeField]
        XButton closeBtn;

        [SerializeField]
        XListView xListView;

        Dictionary<int, RoleItem> itemDic = new Dictionary<int, RoleItem>();

        public List<RoleData> roleDatas = new List<RoleData>();

        // Start is called before the first frame update
        void Start()
        {
            closeBtn.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.CloseView("RoleView");
            });


            xListView.onCreateRenderer.AddListener(onListCreateRenderer);
            xListView.onUpdateRenderer.AddListener(onListUpdateRenderer);

            for (int i = 0; i < 8; i++)
            {
                RoleData roleData = new RoleData();
                roleData.name = $"Ö÷½Ç{i}";
                roleDatas.Add(roleData);
            }

            xListView.dataCount = roleDatas.Count;
            xListView.ForceRefresh();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void onListCreateRenderer(ListItemRenderer listItem)
        {
            //Debug.Log("GalManager_Choice onListCreateRenderer");
            RoleItem roleItem = listItem.gameObject.GetComponent<RoleItem>();
            itemDic[listItem.instanceID] = roleItem;

        }

        void onListUpdateRenderer(ListItemRenderer listItem)
        {
            RoleItem roleItem = itemDic[listItem.instanceID];
            RoleData roleData = roleDatas[listItem.index];
            roleItem.Refresh(roleData);
        }
    }
}
