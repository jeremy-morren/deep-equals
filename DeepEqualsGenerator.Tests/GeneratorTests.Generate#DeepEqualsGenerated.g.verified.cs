﻿//HintName: DeepEqualsGenerated.g.cs
// <auto-generated/>
// This file was automatically generated by the DeepEquals source generator.
// Do not edit this file manually since it will be automatically overwritten.
// ReSharper disable All
#nullable disable
#pragma warning disable SYSLIB0050 // ObjectIDGenerator obsolete
using System;
namespace DeepEqualsGenerator
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DeepEquals", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerStepThroughAttribute()]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal class GeneratedDeepEquals
    {
        private static readonly global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>[] AllMethods = new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>[]
        {
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.Customer), Static_DeepEqualsGenerator_Tests_Models_Customer),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.Contact?), Static_DeepEqualsGenerator_Tests_Models_Contact_Nullable),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.Contact), Static_DeepEqualsGenerator_Tests_Models_Contact),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlyDictionary<string, int>), Static_IReadOnlyDictionary_string__int),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlyDictionary<global::DeepEqualsGenerator.Tests.Models.Order, global::DeepEqualsGenerator.Tests.Models.Order>), Static_IReadOnlyDictionary_DeepEqualsGenerator_Tests_Models_Order__DeepEqualsGenerator_Tests_Models_Order),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.Order), Static_DeepEqualsGenerator_Tests_Models_Order),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IEnumerable<global::System.Reflection.BindingFlags?>), Static_IEnumerable_Reflection_BindingFlags_Nullable),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlySet<global::DeepEqualsGenerator.Tests.Models.Order>), Static_IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlyList<int>), Static_IReadOnlyList_int),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Order>), Static_IReadOnlyList_DeepEqualsGenerator_Tests_Models_Order),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IEnumerable<global::DeepEqualsGenerator.Tests.Models.ChildStruct>), Static_IEnumerable_DeepEqualsGenerator_Tests_Models_ChildStruct),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.ChildStruct), DeepEqualsGenerator_Tests_Models_ChildStruct),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Customer>), Static_IReadOnlyList_DeepEqualsGenerator_Tests_Models_Customer),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::DeepEqualsGenerator.Tests.Models.OrderExtended), Static_DeepEqualsGenerator_Tests_Models_OrderExtended),
            new global::System.Collections.Generic.KeyValuePair<global::System.Type, Delegate>(typeof(global::System.Collections.Generic.List<global::DeepEqualsGenerator.Tests.Models.Customer>), List_DeepEqualsGenerator_Tests_Models_Customer),
        };

        private readonly global::System.Runtime.Serialization.ObjectIDGenerator _lIds = new global::System.Runtime.Serialization.ObjectIDGenerator();
        private readonly global::System.Runtime.Serialization.ObjectIDGenerator _rIds = new global::System.Runtime.Serialization.ObjectIDGenerator();

        private static bool Static_DeepEqualsGenerator_Tests_Models_Customer(global::DeepEqualsGenerator.Tests.Models.Customer l, global::DeepEqualsGenerator.Tests.Models.Customer r) => new GeneratedDeepEquals().DeepEqualsGenerator_Tests_Models_Customer(l,r);

        private bool DeepEqualsGenerator_Tests_Models_Customer(global::DeepEqualsGenerator.Tests.Models.Customer l, global::DeepEqualsGenerator.Tests.Models.Customer r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            return
                l.Address == r.Address &&
                l.Name == r.Name &&
                l.NullableInt == r.NullableInt &&
                l.NullableString == r.NullableString &&
                l.NullableDecimal == r.NullableDecimal &&
                DeepEqualsGenerator_Tests_Models_Contact_Nullable(l.Contact, r.Contact) &&
                IReadOnlyList_int(l.IntList, r.IntList) &&
                IReadOnlyList_DeepEqualsGenerator_Tests_Models_Order(l.Orders, r.Orders) &&
                IEnumerable_DeepEqualsGenerator_Tests_Models_ChildStruct(l.ChildStructs, r.ChildStructs);
        }

        private static bool Static_DeepEqualsGenerator_Tests_Models_Contact_Nullable(global::DeepEqualsGenerator.Tests.Models.Contact? l, global::DeepEqualsGenerator.Tests.Models.Contact? r) => new GeneratedDeepEquals().DeepEqualsGenerator_Tests_Models_Contact_Nullable(l,r);

        private bool DeepEqualsGenerator_Tests_Models_Contact_Nullable(global::DeepEqualsGenerator.Tests.Models.Contact? l, global::DeepEqualsGenerator.Tests.Models.Contact? r)
        {
            return l.HasValue == r.HasValue && (!l.HasValue || DeepEqualsGenerator_Tests_Models_Contact(l.Value, r.Value));
        }

        private static bool Static_DeepEqualsGenerator_Tests_Models_Contact(global::DeepEqualsGenerator.Tests.Models.Contact l, global::DeepEqualsGenerator.Tests.Models.Contact r) => new GeneratedDeepEquals().DeepEqualsGenerator_Tests_Models_Contact(l,r);

        private bool DeepEqualsGenerator_Tests_Models_Contact(global::DeepEqualsGenerator.Tests.Models.Contact l, global::DeepEqualsGenerator.Tests.Models.Contact r)
        {
            return
                l.Telephone == r.Telephone &&
                IReadOnlyDictionary_string__int(l.Keys, r.Keys) &&
                IReadOnlyDictionary_DeepEqualsGenerator_Tests_Models_Order__DeepEqualsGenerator_Tests_Models_Order(l.Orders, r.Orders);
        }

        private static bool Static_IReadOnlyDictionary_string__int(global::System.Collections.Generic.IReadOnlyDictionary<string, int> l, global::System.Collections.Generic.IReadOnlyDictionary<string, int> r) => new GeneratedDeepEquals().IReadOnlyDictionary_string__int(l,r);

        private bool IReadOnlyDictionary_string__int(global::System.Collections.Generic.IReadOnlyDictionary<string, int> l, global::System.Collections.Generic.IReadOnlyDictionary<string, int> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            if (l.Count != r.Count) return false;
            foreach (var pair in l)
            {
                var key = pair.Key;
                var lv = pair.Value;
                if (!r.TryGetValue(key, out var rv)) return false;
                if (lv != rv) return false;
            }
            return true;
        }

        private static bool Static_IReadOnlyDictionary_DeepEqualsGenerator_Tests_Models_Order__DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlyDictionary<global::DeepEqualsGenerator.Tests.Models.Order, global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlyDictionary<global::DeepEqualsGenerator.Tests.Models.Order, global::DeepEqualsGenerator.Tests.Models.Order> r) => new GeneratedDeepEquals().IReadOnlyDictionary_DeepEqualsGenerator_Tests_Models_Order__DeepEqualsGenerator_Tests_Models_Order(l,r);

        private bool IReadOnlyDictionary_DeepEqualsGenerator_Tests_Models_Order__DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlyDictionary<global::DeepEqualsGenerator.Tests.Models.Order, global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlyDictionary<global::DeepEqualsGenerator.Tests.Models.Order, global::DeepEqualsGenerator.Tests.Models.Order> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            if (l.Count != r.Count) return false;
            foreach (var pair in l)
            {
                var key = pair.Key;
                var lv = pair.Value;
                if (!r.TryGetValue(key, out var rv)) return false;
                if (!DeepEqualsGenerator_Tests_Models_Order(lv, rv)) return false;
            }
            return true;
        }

        private static bool Static_DeepEqualsGenerator_Tests_Models_Order(global::DeepEqualsGenerator.Tests.Models.Order l, global::DeepEqualsGenerator.Tests.Models.Order r) => new GeneratedDeepEquals().DeepEqualsGenerator_Tests_Models_Order(l,r);

        private bool DeepEqualsGenerator_Tests_Models_Order(global::DeepEqualsGenerator.Tests.Models.Order l, global::DeepEqualsGenerator.Tests.Models.Order r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            return
                l.CustomerId == r.CustomerId &&
                IEnumerable_Reflection_BindingFlags_Nullable(l.Flags, r.Flags) &&
                IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order(l.Children, r.Children);
        }

        private static bool Static_IEnumerable_Reflection_BindingFlags_Nullable(global::System.Collections.Generic.IEnumerable<global::System.Reflection.BindingFlags?> l, global::System.Collections.Generic.IEnumerable<global::System.Reflection.BindingFlags?> r) => new GeneratedDeepEquals().IEnumerable_Reflection_BindingFlags_Nullable(l,r);

        private bool IEnumerable_Reflection_BindingFlags_Nullable(global::System.Collections.Generic.IEnumerable<global::System.Reflection.BindingFlags?> l, global::System.Collections.Generic.IEnumerable<global::System.Reflection.BindingFlags?> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            using var le = l.GetEnumerator();
            using var re = r.GetEnumerator();
            while (true)
            {
                var ln = le.MoveNext();
                var rn = re.MoveNext();
                if (ln != rn) return false;
                if (!ln) return true;
                if (le.Current != re.Current) return false;
            }
            throw new NotImplementedException();
        }

        private static bool Static_IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlySet<global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlySet<global::DeepEqualsGenerator.Tests.Models.Order> r) => new GeneratedDeepEquals().IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order(l,r);

        private bool IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlySet<global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlySet<global::DeepEqualsGenerator.Tests.Models.Order> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            var count = l.Count;
            if (count != r.Count) return false;
            foreach (var lv in l)
            {
                if (!r.Contains(lv)) return false;
            }
            return true;
        }

        private static bool Static_IReadOnlyList_int(global::System.Collections.Generic.IReadOnlyList<int> l, global::System.Collections.Generic.IReadOnlyList<int> r) => new GeneratedDeepEquals().IReadOnlyList_int(l,r);

        private bool IReadOnlyList_int(global::System.Collections.Generic.IReadOnlyList<int> l, global::System.Collections.Generic.IReadOnlyList<int> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            var count = l.Count;
            if (count != r.Count) return false;
            for (var i = 0; i < count; ++i)
            {
                if (l[i] != r[i]) return false;
            }
            return true;
        }

        private static bool Static_IReadOnlyList_DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Order> r) => new GeneratedDeepEquals().IReadOnlyList_DeepEqualsGenerator_Tests_Models_Order(l,r);

        private bool IReadOnlyList_DeepEqualsGenerator_Tests_Models_Order(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Order> l, global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Order> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            var count = l.Count;
            if (count != r.Count) return false;
            for (var i = 0; i < count; ++i)
            {
                if (!DeepEqualsGenerator_Tests_Models_Order(l[i], r[i])) return false;
            }
            return true;
        }

        private static bool Static_IEnumerable_DeepEqualsGenerator_Tests_Models_ChildStruct(global::System.Collections.Generic.IEnumerable<global::DeepEqualsGenerator.Tests.Models.ChildStruct> l, global::System.Collections.Generic.IEnumerable<global::DeepEqualsGenerator.Tests.Models.ChildStruct> r) => new GeneratedDeepEquals().IEnumerable_DeepEqualsGenerator_Tests_Models_ChildStruct(l,r);

        private bool IEnumerable_DeepEqualsGenerator_Tests_Models_ChildStruct(global::System.Collections.Generic.IEnumerable<global::DeepEqualsGenerator.Tests.Models.ChildStruct> l, global::System.Collections.Generic.IEnumerable<global::DeepEqualsGenerator.Tests.Models.ChildStruct> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            using var le = l.GetEnumerator();
            using var re = r.GetEnumerator();
            while (true)
            {
                var ln = le.MoveNext();
                var rn = re.MoveNext();
                if (ln != rn) return false;
                if (!ln) return true;
                if (!DeepEqualsGenerator_Tests_Models_ChildStruct(le.Current, re.Current)) return false;
            }
            throw new NotImplementedException();
        }

        private static bool DeepEqualsGenerator_Tests_Models_ChildStruct(global::DeepEqualsGenerator.Tests.Models.ChildStruct l, global::DeepEqualsGenerator.Tests.Models.ChildStruct r)
        {
            return
                l.Id == r.Id;
        }

        private static bool Static_IReadOnlyList_DeepEqualsGenerator_Tests_Models_Customer(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Customer> l, global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Customer> r) => new GeneratedDeepEquals().IReadOnlyList_DeepEqualsGenerator_Tests_Models_Customer(l,r);

        private bool IReadOnlyList_DeepEqualsGenerator_Tests_Models_Customer(global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Customer> l, global::System.Collections.Generic.IReadOnlyList<global::DeepEqualsGenerator.Tests.Models.Customer> r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            var count = l.Count;
            if (count != r.Count) return false;
            for (var i = 0; i < count; ++i)
            {
                if (!DeepEqualsGenerator_Tests_Models_Customer(l[i], r[i])) return false;
            }
            return true;
        }

        private static bool Static_DeepEqualsGenerator_Tests_Models_OrderExtended(global::DeepEqualsGenerator.Tests.Models.OrderExtended l, global::DeepEqualsGenerator.Tests.Models.OrderExtended r) => new GeneratedDeepEquals().DeepEqualsGenerator_Tests_Models_OrderExtended(l,r);

        private bool DeepEqualsGenerator_Tests_Models_OrderExtended(global::DeepEqualsGenerator.Tests.Models.OrderExtended l, global::DeepEqualsGenerator.Tests.Models.OrderExtended r)
        {
            if (object.ReferenceEquals(l, r)) return true;
            if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;

            long lId = _lIds.GetId(l, out bool lFirst);
            long rId = _rIds.GetId(r, out bool rFirst);
            if (lFirst != rFirst || lId != rId) return false;

            return
                l.Date == r.Date &&
                l.CustomerId == r.CustomerId &&
                IEnumerable_Reflection_BindingFlags_Nullable(l.Flags, r.Flags) &&
                IReadOnlySet_DeepEqualsGenerator_Tests_Models_Order(l.Children, r.Children);
        }

        private static bool List_DeepEqualsGenerator_Tests_Models_Customer(global::System.Collections.Generic.List<global::DeepEqualsGenerator.Tests.Models.Customer> l, global::System.Collections.Generic.List<global::DeepEqualsGenerator.Tests.Models.Customer> r) => new GeneratedDeepEquals().IReadOnlyList_DeepEqualsGenerator_Tests_Models_Customer(l,r);
    }
}
