using System;
using System.Collections;
using System.Collections.Generic;

enum Op
{
    NONE,
    ADD,
    SUB,
    MULT,
    DIV
}

/// <summary>
/// Read, Write, Calculate and maybe other virtual Operations
/// Current Calculation Support for: STRING | FLOAT
/// </summary>
public class Operation : Block
{
    Op op = Op.NONE;
    string key = null;

    //Alternative types of Values
    string name1 = null;
    Object value1 = null;

    string name2 = null;
    Object value2 = null;

    public void calculate(ref Storage storage)
    {
        if (key == null) return;

        //if name is set, use that value
        if (name1 != null)
        {
            if (storage.readValue(name1, out Object val))
            {
                value1 = val;
            }
        }
        if (name2 != null)
        {
            if (storage.readValue(name2, out Object val))
            {
                value2 = val;
            }
        }

        switch (op)
        {
            case Op.NONE:
                {
                    storage.writeValue(key, value1);
                } 
                break;

            case Op.ADD:
                {
                    System.Object result = value1;
                    //Current Supported Types: FLOAT | STRING
                    if (value1.GetType() == value2.GetType())
                    {
                        if (value1 is float) result = (float)value1 + (float)value2;
                        if (value1 is string) result = (string)value1 + (string)value2;
                    }
                    //IF OPERATION DIDNT SUCCEED, WE IGNORE VALUE2!!
                    storage.writeValue(key, result);
                }
                break;

            case Op.SUB:
                {
                    System.Object result = value1;
                    //Current Supported Types: FLOAT | STRING
                    if (value1.GetType() == value2.GetType())
                    {
                        if (value1 is float) result = (float)value1 - (float)value2;
                    }
                    //IF OPERATION DIDNT SUCCEED, WE IGNORE VALUE2!!
                    storage.writeValue(key, result);
                }
                break;

            case Op.MULT:
                {
                    System.Object result = value1;
                    //Current Supported Types: FLOAT
                    if (value1.GetType() == value2.GetType())
                    {
                        if (value1 is float) result = (float)value1 * (float)value2;
                    }
                    //IF OPERATION DIDNT SUCCEED, WE IGNORE VALUE2!!
                    storage.writeValue(key, result);
                }
                break;

            case Op.DIV:
                {
                    System.Object result = value1;
                    //Current Supported Types: FLOAT
                    if (value1.GetType() == value2.GetType())
                    {
                        if (value1 is float) result = (float)value1 / (float)value2;
                    }
                    //IF OPERATION DIDNT SUCCEED, WE IGNORE VALUE2!!
                    storage.writeValue(key, result);
                }
                break;

            default: break;
        }
    }

    public void setOpNone(string key, string name1, Object value1)
    {
        op = Op.NONE;
        this.key = key;
        this.name1 = name1;
        this.value1 = value1;
    }

    private void assign(string key, string name1, Object value1, string name2, Object value2)
    {
        this.key = key;
        this.name1 = name1;
        this.value1 = value1;
        this.name2 = name2;
        this.value2 = value2;
    }

    public void setOpAdd(string key, string name1, Object value1, string name2, Object value2)
    {
        op = Op.ADD;
        assign(key, name1, value1, name2, value2);
    }

    public void setOpSub(string key, string name1, Object value1, string name2, Object value2)
    {
        op = Op.SUB;
        assign(key, name1, value1, name2, value2);
    }

    public void setOpMult(string key, string name1, Object value1, string name2, Object value2)
    {
        op = Op.MULT;
        assign(key, name1, value1, name2, value2);
    }

    public void setOpDiv(string key, string name1, Object value1, string name2, Object value2)
    {
        op = Op.DIV;
        assign(key, name1, value1, name2, value2);
    }
}
