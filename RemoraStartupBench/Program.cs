using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using HarmonyLib;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Gateway.Events;
using Remora.Rest.Extensions;
using RemoraStartupBench;

BenchmarkRunner.Run<Bench>();

[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class Bench
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        new Harmony("benchmark.fec")
            .PatchAll();
    }

    [Benchmark]
    public JsonSerializerOptions Original()
    {
        CompileHelper.UseFEC = false;
        var options = new JsonSerializerOptions();
        options.AddDataObjectConverter<IGuildCreate, GuildCreate>();
        return options;
    }

    [Benchmark(Baseline = true)]
    public JsonSerializerOptions FastCompiler()
    {
        CompileHelper.UseFEC = true;
        var options = new JsonSerializerOptions();
        options.AddDataObjectConverter<IGuildCreate, GuildCreate>();
        return options;
    }
}