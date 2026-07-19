using BenchmarkDotNet.Running;

using Benchmarks.Fx.Net;
using Benchmarks.Fx.Net.Interactions;
using Benchmarks.Fx.Net.OptionBench;
using Benchmarks.Fx.Net.ResultBench;

BenchmarkRunner.Run<ResultHeavyPayloadBenchmarks>();