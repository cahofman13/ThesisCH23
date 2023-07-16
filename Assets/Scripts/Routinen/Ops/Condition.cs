using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

public enum Comp
{
    EQUAL,
    LOWER,
    GREATER
}

public class Condition : Term
{
    public Comp comp = Comp.EQUAL;

    public string key1 = null;
    public Object value1 = "";     //so default evaluates to false
    public string key2 = null;
    public Object value2 = null;

    public Condition() { }

    /// <summary>
    /// Init! compare as -1,0,1 means &lt;,=,&gt; respectively
    /// </summary>
    /// <param name="compare"></param>
    /// <param name="key1"></param>
    /// <param name="value1"></param>
    /// <param name="key2"></param>
    /// <param name="value2"></param>
    public Condition(int compare, string key1, Object value1, string key2, Object value2)
    {
        this.setComp(compare, key1, value1, key2, value2);
    }
    public Condition(Condition original)
    {
        this.setComp(original.comp, original.key1, original.value1, original.key2, original.value2);
    }

    public void setComp(int compare, string key1, Object value1, string key2, Object value2)
    {
        if (compare < 0) comp = Comp.LOWER;
        else if (compare == 0) comp = Comp.EQUAL;
        else if (compare > 0) comp = Comp.GREATER;
        this.key1 = key1;
        this.key2 = key2;
        this.value1 = value1;
        this.value2 = value2;
    }
    public void setComp(Comp comp, string key1, Object value1, string key2, Object value2)
    {
        this.comp = comp;
        this.key1 = key1;
        this.key2 = key2;
        this.value1 = value1;
        this.value2 = value2;
    }

    public override Object evaluate()
    {
        throw new NotImplementedException();
    }

    public bool evalBool(ref Storage storage)
    {
        if (key1 != null)
        {
            if (storage.readValue(key1, out Object val))
            {
                value1 = val;
            }
        }
        if (key2 != null)
        {
            if (storage.readValue(key2, out Object val))
            {
                value2 = val;
            }
        }

        bool result = false;
        bool compare = false;
        try
        {
            Type type1 = value1.GetType();
            Type type2 = value2.GetType();
            compare = true;
        } catch (Exception e)
        {
            UnityEngine.Debug.LogWarning("Caught Error while getting Type, probably meaning it is NULL which is fine! \n" + e);
        }
        if (compare)
        {
            switch (comp)
            {
                case Comp.LOWER:
                    if (value1 is float) result = (float)value1 < (float)value2;
                    else if (value1 is string) result = ((string)value1).CompareTo((string)value2) < 0;
                    break;
                case Comp.EQUAL:
                    if (value1 is float) result = (float)value1 == (float)value2;
                    else if (value1 is string) result = (string)value1 == (string)value2;
                    else result = value1.Equals(value2);
                    break;
                case Comp.GREATER:
                    if (value1 is float) result = (float)value1 > (float)value2;
                    else if (value1 is string) result = ((string)value1).CompareTo((string)value2) > 0;
                    break;
                default: break;
            }
        }

        return result;
    }

}
