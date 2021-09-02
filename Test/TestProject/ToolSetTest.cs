using BenchmarkDotNet.Attributes;
using System;

namespace TestProject
{
    public class ToolSetTest
    {
        [Benchmark]
        public MyCs ExpressionGetInstance() => ToolSet.Generic.GenericActivator<MyCs>.Create();

        [Benchmark]
        public MyCs NewInstance() => new MyCs();

        [Benchmark]
        public MyCs ActivatorInstance() => Activator.CreateInstance<MyCs>();

        [Benchmark]
        public MyCs GAInstance() => ToolSet.Generic.GenericActivator.CreateInstance<MyCs>();
    }

    public class MyCs
    {
        public string Name { get; set; }
    }

}