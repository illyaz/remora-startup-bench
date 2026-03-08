namespace RemoraStartupBench;

using FastExpressionCompiler;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

[HarmonyPatch]
public static class CreatePropertyWriterPatch
{
    public static MethodBase TargetMethod()
    {
        var type = typeof(Remora.Rest.Json.DataObjectConverter<,>)
            .Assembly
            .GetType("Remora.Rest.Json.Reflection.ExpressionFactoryUtilities")!;

        return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Single(m => m.Name == "CreatePropertyWriter");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Callvirt
                && instruction.operand is MethodInfo { Name: nameof(LambdaExpression.Compile) })
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(CompileHelper), nameof(CompileHelper.Compile)));
            else
                yield return instruction;
        }
    }
}

public static class CompileHelper
{
    public static bool UseFEC = false;

    public static Delegate Compile(LambdaExpression expression) =>
        UseFEC
            ? expression.CompileFast() ?? expression.Compile()
            : expression.Compile();
}