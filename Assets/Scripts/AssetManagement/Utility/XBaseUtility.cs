﻿using UnityEngine;
using System.Collections;

public static class XBaseUtility
{

    public static bool EndsWithEx(this string a, string b)
    {
        int ap = a.Length - 1;
        int bp = b.Length - 1;

        while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
        {
            ap--;
            bp--;
        }
        return (bp < 0 && a.Length >= b.Length) || (ap < 0 && b.Length >= a.Length);
    }

    public static bool StartsWithEx(this string a, string b)
    {
        int aLen = a.Length;
        int bLen = b.Length;
        int ap = 0; int bp = 0;

        while (ap < aLen && bp < bLen && a[ap] == b[bp])
        {
            ap++;
            bp++;
        }
        return (bp == bLen && aLen >= bLen) || (ap == aLen && bLen >= aLen);
    }

    //public static XLua.LuaTable ToLuaTable(this Hashtable hashtable)
    //{
    //    if (!LuaEnvironment.Instance || hashtable == null)
    //    {
    //        return null;
    //    }        

    //    XLua.LuaTable luaTable = LuaEnvironment.Instance.LuaEnv.NewTable();
    //    foreach (string key in hashtable.Keys)
    //    {
    //        luaTable.SetInPath(key, hashtable[key]);
    //    }
    //    return luaTable;
    //}
}
