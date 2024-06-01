using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XGUI;
using static XGUI.XListView;

namespace XModules.Main
{
    public class ProjectData
    {
        public string desc;
        public string prompt;
        public string characterName;
    }

    public class ProjectView : XBaseView
    {
        [SerializeField]
        XButton closeBtn;

        [SerializeField]
        Dropdown styleDropdown;

        [SerializeField]
        XButton exportBtn;

        [SerializeField]
        XButton characterBtn;

        [SerializeField]
        XButton regenBtn;

        [SerializeField]
        XButton deepfakeBtn;

        [SerializeField]
        XButton addBtn;

        [SerializeField]
        XListView xListView;

        Dictionary<int, ProjectItem> itemDic = new Dictionary<int, ProjectItem>();

        public List<ProjectData> projectDatas = new List<ProjectData>();

        // Start is called before the first frame update
        void Start()
        {
            xListView.onCreateRenderer.AddListener(onListCreateRenderer);
            xListView.onUpdateRenderer.AddListener(onListUpdateRenderer);

            closeBtn.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.CloseView("ProjectView");
            });

            exportBtn.onClick.AddListener(() => 
            { 
                
            });

            characterBtn.onClick.AddListener(() => 
            {
                XGUI.XGUIManager.Instance.OpenView("RoleView");
            });

            regenBtn.onClick.AddListener(() => 
            { 
            
            });

            deepfakeBtn.onClick.AddListener(() => 
            { 
            
            });

            addBtn.onClick.AddListener(() => 
            { 
                
            });

            for(int i =0;i< 8;i++)
            {
                ProjectData projectData = new ProjectData();
                projectData.desc = "这是文章的描述";
                projectData.characterName = "主角1";
                projectData.prompt = "这是prompt";

                projectDatas.Add(projectData);
            }

            xListView.dataCount = projectDatas.Count;
            xListView.ForceRefresh();
        }

        void onListCreateRenderer(ListItemRenderer listItem)
        {
            //Debug.Log("GalManager_Choice onListCreateRenderer");
            ProjectItem projectItem = listItem.gameObject.GetComponent<ProjectItem>();
            itemDic[listItem.instanceID] = projectItem;

        }

        void onListUpdateRenderer(ListItemRenderer listItem)
        {
            ProjectItem projectItem = itemDic[listItem.instanceID];
            ProjectData projectData = projectDatas[listItem.index];
            projectItem.Refresh(projectData);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
