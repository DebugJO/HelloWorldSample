// Reference : Brian Lagunas : https://www.youtube.com/watch?v=J5YQ8MwMYMw

using System.Dynamic;

using dynamic dynamicObj = new Dynamic();
dynamicObj.FirstName = "길동";
dynamicObj.AddProperty("LastName", "홍");
dynamicObj["Age"] = 32;

try
{
    Console.WriteLine($"1 : {dynamicObj.FirstName}");
    Console.WriteLine($"2 : {dynamicObj.LastName}");
    Console.WriteLine($"3 : {dynamicObj["Age"]}");
}
catch (Exception ex)
{
    Console.WriteLine("Error Message : " + ex.Message);
}

class Dynamic : DynamicObject, IDisposable
{
    private bool disposing = false;

    readonly Dictionary<string, object> _dictionary = new();

    public Dynamic()
    {
        if (disposing)
            return;
    }

    public object this[string propertyName]
    {
        get
        {
            return _dictionary[propertyName];
        }
        set
        {
            AddProperty(propertyName, value);
        }
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        return _dictionary.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        AddProperty(binder.Name, value!);
        return true;
    }

    public void AddProperty(string name, object value)
    {
        _dictionary[name] = value;
    }

    public void Dispose()
    {
        if (disposing)
            return;
        disposing = true;
        Console.WriteLine($"Dispose {nameof(Dynamic)}");
        GC.SuppressFinalize(this);
    }
}
