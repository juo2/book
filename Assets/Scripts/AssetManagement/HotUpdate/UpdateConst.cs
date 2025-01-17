﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateConst
{
    private static Dictionary<int, string> s_UpdateLanguage = new Dictionary<int, string>()
    { 
        {1000,@"<color=#44DB72FF>检查游戏版本！</color>"},
        {1010,@"<color=#F34343FF>内存卡版本文件内容为空！</color>"},
        {1020,@"<color=#F34343FF>内存卡版本文件解析异常！</color>"},

        {1100,@"<color=#F34343FF>内置版本文件内容为空！</color>"},
        {1101,@"<color=#F34343FF>内置版本文件解析异常！</color>"},

        {1201,@"<color=#F34343FF>版本检查发生异常！ {0}</color>"},

        {5000,@"<color=#44DB72FF>启动更新：{0} 后台更新：{1} CSharp重启：{2} 总文件数：{3}</color>"},
        {5001,@"<color=#44DB72FF>后台下载：{0}</color>"},

        {5005,@"<color=yellow>下载成功：{0}/{1}  {2}</color>"},
        {5006,@"<color=#F34343FF>下载失败：{0} error: {1}</color>"},
        

        {10000,@"<color=#44DB72FF>版本更新完成！</color>"},
        {10002,@"<color=#44DB72FF>正在校验文件</color>"},
        {10003,@"<color=#44DB72FF>正在更新文件</color>"},

        {10010,"游戏发现新内容，立即更新体验！\n <size=25>(资源大小为<color=#44DB72FF>{0}</color>流量）</size>"},
        {10011,"游戏发现新内容，立即更新体验！\n <size=25>(资源大小为<color=#44DB72FF>{0}</color>流量，建议连接WIFI）</size>"},


        {11001,@"检查版本"},
        {11002,@"检查服务器版本"},
        {11003,@"检查本地文件列表"},
        {11004,@"下载资产清单"},
        {11005,@"校验本地文件{0:F}%"},
        {11006,@"正在更新文件{0}/{1}  文件数{2}/{3}  {4}/s"},
        {11007,@"正在初始化游戏配置，即将进入游戏"},
        {11008,@"正在解压游戏资源(此过程不消耗流量)"},

        {11206,@"确 定"},
        {11207,@"取 消"},

        {11301,@"提示"},
        {11302,"<color=#f34343>当前没有可使用的网络\n请连接正确的网络4G/Wifi！</color>\n<color=#44DB72FF>连接网络后将自动继续</color>"},
        {11303,"<color=#F34343FF>文件下载发生异常！\n<size=25><color=#44DB72FF>可以尝试重试或是重启游戏修复</color></size>\n<size=12>{0}</size></color>"},
        {11304,@"再试一把"},
        {11305,@"重 试"},
        {11306,"资源检查发生异常！\n<color=#F34343FF><size=12>{0}</size></color>\n请点击【重试】进行重新加载\n\n<color=#75ffd0><size=22>（若多次重试无效，请联系客服）</size></color>"},
        {11307,"网络连接失败，请重试。\n<color=#75ffd0><size=22>（若多次重试无效，请联系客服）</size></color>"},

        {12000,@"声音组件初始化失败!"},
        {12001,@"材质组件初始化失败!"},

        {13000,@"请求权限"},
        {13001,"尊敬的玩家，请您授予游戏运行时需要的所有权限，所有请求只是为玩家提供友好服务，请放心授予。\n"},
        {13002,"以下必要权限需要您授予：\n"},
        {13003,@"存储空间权限，用户保存游戏更新资源。"},
        {13004,@"如拒绝则无法进入游戏。"},
        {13005,"以下必要权限被您拒绝，请进入设置页面授权：\n"},
        {13006,@"拒 绝"},
        {13007,@"允 许"},
        {13008,@"设 置"},
        {13009,@"退 出"},
        {13010,"尊敬的玩家，因为您永久拒绝了相机权限，所以无法正常使用头像功能，如需使用请进入设置界面授权。"},
        {13011,"尊敬的玩家，因为您永久拒绝了录音权限，所以无法正常使用语音功能，如需使用请进入设置界面授权。"},
        {13012,"尊敬的玩家，您已更新到最新内容\n请【重启游戏】获得更好体验" },
        {13013,"<color=red>文件il2cpp解压发生异常！\n<size=25><color=#0B7F1AFF>可以尝试重试或是重启游戏修复</color></size>\n<size=12>{0}</size></color>"},

    };

    public static string GetLanguage(int id)
    {
       return s_UpdateLanguage.ContainsKey(id) ? s_UpdateLanguage[id] : id.ToString();
    }
}
