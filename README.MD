# Remora.Discord Startup Bench

Benchmarks `DataObjectConverter` expression compilation overhead in
[Remora.Rest](https://github.com/Remora/Remora.Rest) /
[Remora.Discord](https://github.com/Remora/Remora.Discord) across .NET 9 vs
.NET 10, and evaluates [FastExpressionCompiler](https://github.com/dadhi/FastExpressionCompiler)
as a drop-in replacement.

## How It Works

[Lib.Harmony](https://github.com/pardeike/Harmony) patches
`ExpressionFactoryUtilities` at runtime to redirect
`Compile()` → `CompileFast()` without any changes to the source.

## Running

Requires .NET 9 and .NET 10 SDKs.

```bash
dotnet run -c release --framework net10.0
```

## Results
```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7840/25H2/2025Update/HudsonValley2)
Intel Core i9-14900K 3.20GHz, 1 CPU, 32 logical and 24 physical cores                                                                                                                                      
.NET SDK 10.0.103                                                                                                                                                                                          
  [Host]    : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3                                                                                                                                    
  .NET 10.0 : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3                                                                                                                                    
  .NET 9.0  : .NET 9.0.13 (9.0.13, 9.0.1326.6317), X64 RyuJIT x86-64-v3                                                                                                                                    
                                                                                                                                                                                                           

| Method       | Job       | Runtime   | Mean         | Error     | StdDev    | Ratio  | RatioSD |
|------------- |---------- |---------- |-------------:|----------:|----------:|-------:|--------:|
| Original     | .NET 10.0 | .NET 10.0 | 1,415.925 us | 6.9176 us | 6.1323 us | 407.28 |    3.43 |                                                                                                         
| FastCompiler | .NET 10.0 | .NET 10.0 |     3.477 us | 0.0299 us | 0.0265 us |   1.00 |    0.01 |
|              |           |           |              |           |           |        |         |
| Original     | .NET 9.0  | .NET 9.0  |   288.147 us | 5.3529 us | 5.0071 us |  73.46 |    1.85 |
| FastCompiler | .NET 9.0  | .NET 9.0  |     3.924 us | 0.0737 us | 0.0756 us |   1.00 |    0.03 |
```