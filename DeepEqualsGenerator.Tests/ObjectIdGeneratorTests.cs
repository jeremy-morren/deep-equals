using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FluentAssertions;
using Shouldly;

namespace DeepEqualsGenerator.Tests;

public class ObjectIdGeneratorTests
{
    [Fact]
    public void IdsShouldBeSequential()
    {
        var l = new ObjectIDGenerator();
        var r = new ObjectIDGenerator();

        var objects = new List<object>();
        
        Assert.All(Enumerable.Range(0, 10), i =>
        {
            var o = new object();
            objects.Add(o);
            
            var lId = l.GetId(o, out var lFirst);
            var rId = r.GetId(o, out var rFirst);
            lFirst.ShouldBeTrue();
            rFirst.ShouldBeTrue();

            Assert.Equal(lId, rId);
        });
        
        Assert.All(Enumerable.Range(0, 10), i =>
        {
            var o = objects[i];
            var lId = l.GetId(o, out var lFirst);
            var rId = l.GetId(o, out var rFirst);
            lFirst.ShouldBeFalse();
            rFirst.ShouldBeFalse();
            Assert.Equal(lId, rId);
        });
    }
}