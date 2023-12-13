// ReSharper disable All

using System.Reflection;
using System;
using System.Collections.Generic;

#pragma warning disable CS0169 // Field is never used
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#nullable enable

namespace DeepEqualsGenerator.Tests.Models;

[GenerateDeepEquals]
[GenerateDeepEquals(typeof(List<Customer>))]
public class Customer
{
    public string Name { get; init; }

    public string Address;

    private int _private;

    public int? NullableInt { get; }

    public Contact? Contact { get; }
    
    public string? NullableString { get; }
    
    public Nullable<decimal> NullableDecimal { get; }

    public List<int> IntList { get; }

    public List<Order> Orders { get; }

    public IEnumerable<ChildStruct> ChildStructs { get; }
    
    public DateOnly Date { get; }
}

public struct Contact
{
    public string Telephone { get; }

    public Dictionary<string, int> Keys;
    
    public Dictionary<Order, Order> Orders;
    
    public HashSet<int> Ints;
}

public struct ChildStruct
{
    public int Id;

    public double? DoubleNull;

    public float Float { get; }
}

public class Order
{
    public int CustomerId { get; }

    public IEnumerable<BindingFlags?>? Flags;

    public Order[] Children;
}

[GenerateDeepEquals]
public class OrderExtended : Order
{
    public DateTimeOffset? Date { get; init; }
}