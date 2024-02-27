using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using DeepEqual.Syntax;
using JetBrains.Annotations;

namespace DeepEqualsGenerator;

[PublicAPI]
public static class GeneratedDeepEqualsHelpers
{
    /// <summary>
    /// Checks if two objects are equal using a generated method if available, otherwise using DeepEqual
    /// </summary>
    /// <param name="a">Item a</param>
    /// <param name="b">Item b</param>
    /// <typeparam name="T">Type to compare</typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">
    /// <paramref name="a"/> and <paramref name="b"/> are different types
    /// </exception>
    [System.Diagnostics.Contracts.Pure] //Not actually pure, but should be used as such
    public static bool IsFastDeepEqual<T>(this T? a, T? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        
        if (a.GetType() != b.GetType())
            throw new NotImplementedException("Cannot compare values of different types");

        return Invoke(a, b, GetFastDeepEqual<T>());
    }
    
    public static Func<T, T, bool> GetFastDeepEqual<T>()
    {
        if (GeneratedMethods.TryGetValue(typeof(T), out var d))
            return (Func<T, T, bool>)d;
        
        //Add loaded generated methods
        AddGeneratedMethods();
        
        if (GeneratedMethods.TryGetValue(typeof(T), out d))
            return (Func<T, T, bool>)d;

        throw new Exception($"No generated deep equals method found for {typeof(T)}");
    }

    private static bool Invoke<T>(T a, T b, Func<T, T, bool> func)
    {
        return func(a, b);
        //TODO: Fast occasionally returns false where slow returns true
        
// #if RELEASE
//         return func(a,b);
// #else
//         var slow = a.IsDeepEqual(b);
//         var fast = func(a, b);
//         if (slow == fast) return fast;
//         
//         var json = JsonSerializer.Serialize(a) == JsonSerializer.Serialize(b);
//         
//         throw new Exception($"Slow did not match fast for {typeof(T).FullName}. Slow: {slow}, Fast: {fast}, JSON: {json}");
// #endif
    }
    
    private static void AddGeneratedMethods()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
            {
                var name = a.GetName().Name;
                //Exclude assemblies that won't have generated methods
                return !name.StartsWith("Microsoft", StringComparison.Ordinal) 
                       && !name.StartsWith("System", StringComparison.Ordinal) 
                       && !name.StartsWith("Windows", StringComparison.Ordinal) 
                       && !name.StartsWith("netstandard", StringComparison.Ordinal) 
                       && !name.StartsWith("DeepEquals", StringComparison.Ordinal) 
                       && name != "mscorlib";
            });
        lock (ProcessedAssemblies)
        {
            foreach (var a in assemblies)
            {
                if (ProcessedAssemblies.Contains(a))
                    return;
                
                var type = a.GetType("DeepEqualsGenerator.GeneratedDeepEquals");
                if (type != null)
                {
                    var prop = type.GetField("AllMethods", BindingFlags.Static | BindingFlags.NonPublic)
                               ?? throw new InvalidOperationException($"Unable to get AllMethods field on {type}");
                    var methods = (IEnumerable<KeyValuePair<Type, Delegate>>)prop.GetValue(null)!;
                    foreach (var pair in methods)
                        if (!GeneratedMethods.TryAdd(pair.Key, pair.Value))
                            throw new Exception($"{pair.Key} has multiple generated DeepEquals methods");
                }
            
                ProcessedAssemblies.Add(a);
            }
        }
    }
    
    private static readonly HashSet<Assembly> ProcessedAssemblies = new();
    
    private static readonly ConcurrentDictionary<Type, Delegate> GeneratedMethods;

    static GeneratedDeepEqualsHelpers()
    {
        GeneratedMethods = new ConcurrentDictionary<Type, Delegate>();
        
        //Add primitive types
        static void Add<T>(Func<T,T,bool> d)
        {
            Debug.Assert(GeneratedMethods.TryAdd(typeof(T), d));
        }

        Add<bool>((a, b) => a == b);
        Add<byte>((a, b) => a == b);
        Add<sbyte>((a, b) => a == b);
        Add<char>((a, b) => a == b);
        Add<decimal>((a, b) => a == b);
        Add<double>((a, b) => a.Equals(b));
        Add<float>((a, b) => a.Equals(b));
        Add<int>((a, b) => a == b);
        Add<uint>((a, b) => a == b);
        Add<nint>((a, b) => a == b);
        Add<nuint>((a, b) => a == b);
        Add<long>((a, b) => a == b);
        Add<ulong>((a, b) => a == b);
        Add<short>((a, b) => a == b);
        Add<ushort>((a, b) => a == b);
        
        Add<bool?>((a, b) => a == b);
        Add<byte?>((a, b) => a == b);
        Add<sbyte?>((a, b) => a == b);
        Add<char?>((a, b) => a == b);
        Add<decimal?>((a, b) => a == b);
        Add<double?>((a, b) => a.Equals(b));
        Add<float?>((a, b) => a.Equals(b));
        Add<int?>((a, b) => a == b);
        Add<uint?>((a, b) => a == b);
        Add<nint?>((a, b) => a == b);
        Add<nuint?>((a, b) => a == b);
        Add<long?>((a, b) => a == b);
        Add<ulong?>((a, b) => a == b);
        Add<short?>((a, b) => a == b);
        Add<ushort?>((a, b) => a == b);
        
        Add<string>((a, b) => a == b);
        
        Add<object>((_, _) => throw new NotImplementedException("Raw object comparison is not supported"));
    }
}