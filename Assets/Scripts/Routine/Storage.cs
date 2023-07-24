using System;
using System.Collections.Generic;


public class Storage
{

    //<type ?>
    Dictionary<string, Object> list = new Dictionary<string, Object>();

    public static string[] varNameArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" };

    public void writeValue(string name, Object obj)
    {
        if(list.ContainsKey(name))
        {
            list.Remove(name);
        }
        list.Add(name, obj);
        if(name != "posX" && name != "posY" && name != "posZ" && name != "color") UnityEngine.Debug.Log(name + ": " + obj);
    }

    public bool readValue(string name, out Object value)
    {
        if (list.TryGetValue(name, out Object val))
        {
            value = val;
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }

    public Dictionary<string, Object>.KeyCollection listNames()
    {
        return list.Keys;
    }

}
