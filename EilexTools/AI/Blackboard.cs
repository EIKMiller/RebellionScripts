using System;
using System.Collections.Generic;
using UnityEngine;
public class Blackboard
{
    private BaseCharacterController _Owner;
    public BaseCharacterController Owner { get => _Owner; }
    private List<BlackboardProperty> _Properties = new List<BlackboardProperty>();

    public Blackboard(BaseCharacterController owner)
    {
        _Owner = owner;
    }

    public void SetValueAsInt(string key, int value)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
            {
                if(prop is BlackboardInt p)
                {
                    p.Value = value;
                    return;
                }
            }
        }

        _Properties.Add(new BlackboardInt(key, value));
    }

    public int GetValueAsInt(string key)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
                if(prop is BlackboardInt p)
                    return p.Value;
        }

        return 0;
    }

    public void SetValueAsFloat(string key, float value)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
            {
                if(prop is BlackboardFloat p)
                {
                    p.Value = value;
                    return;
                }
            }
        }

        _Properties.Add(new BlackboardFloat(key, value));
    }

    public float GetValueAsFloat(string key)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
                if(prop is BlackboardFloat p)
                    return p.Value;
        }

        return 0f;
    }

    public void SetValueAsBool(string key, bool value)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
            {
                if(prop is BlackboardBool p)
                {
                    p.Value = value;
                    return;
                }
            }
        }

        _Properties.Add(new BlackboardBool(key, value));
    }

    public bool GetValueAsBool(string key)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
                if(prop is BlackboardBool p)
                    return p.Value;
        }

        return false;
    }

    public void SetValueAsVector(string key, Vector3 value)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
            {
                if(prop is BlackboardVector p)
                {
                    p.Value = value;
                    return;
                }
            }
        }

        _Properties.Add(new BlackboardVector(key, value));
    }

    public Vector3 GetValueAsVector(string key)
    {
        foreach(var prop in _Properties)
            if(prop == key)
                if(prop is BlackboardVector p)
                    return p.Value;

        return Vector3.zero;
    }

    public void SetValueAsGameObject(string key, GameObject value)
    {
        foreach(var prop in _Properties)
        {
            if(prop == key)
            {
                if(prop is BlackboardGameObject p)
                {
                    p.Value = value;
                    return;
                }
            }
        }
        _Properties.Add(new BlackboardGameObject(key, value));
    }

    public GameObject GetValueAsGameObject(string key)
    {
        foreach(var prop in _Properties)
            if(prop == key)
                if(prop is BlackboardGameObject p)
                    return p.Value;

        return null;
    }
    
}

public class BlackboardProperty
{
    public string Key;

    public BlackboardProperty(string key)
    {
        Key = key;
    }

    public static bool operator ==(BlackboardProperty a, string key)
    {
        if(a.Key == key)
            return true;

        return false;
    }

    public static bool operator !=(BlackboardProperty a, string key)
    {
        if(a == key)
            return false;

        return false;
    }
}

public class BlackboardInt : BlackboardProperty
{
    public int Value;

    public BlackboardInt(string key, int value) : base(key)
    {
        Value = value;
    }
}

public class BlackboardFloat : BlackboardProperty
{
    public float Value;

    public BlackboardFloat(string key, float value) : base(key)
    {
        Value = value;
    }
}

public class BlackboardBool : BlackboardProperty
{
    public bool Value;

    public BlackboardBool(string key, bool value) : base(key)
    {
        Value = value;
    }
}

public class BlackboardVector : BlackboardProperty
{
    public Vector3 Value;

    public BlackboardVector(string key, Vector3 value) : base(key)
    {
        Value = value;
    }
}

public class BlackboardGameObject : BlackboardProperty
{
    public GameObject Value;

    public BlackboardGameObject(string key, GameObject value) : base(key)
    {
        Value = value;
    }
}

