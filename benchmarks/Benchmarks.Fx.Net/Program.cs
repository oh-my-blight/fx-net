using BenchmarkDotNet.Running;

using Benchmarks.Fx.Net;
using Benchmarks.Fx.Net.ResultBench;

BenchmarkRunner.Run<ResultHeavyPayloadBenchmarks>();