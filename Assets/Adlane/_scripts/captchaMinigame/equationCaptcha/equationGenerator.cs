using System;
using System.Collections.Generic;
using UnityEngine;

public static class EquationGenerator
{
    private static System.Random rng = new System.Random();

    public static (string equation, int answer) GenerateEasy()
    {
        int answer = UnityEngine.Random.Range(5, 20);

        int b = UnityEngine.Random.Range(1, 10);
        int a = answer + b;

        string equation = $"{a} - {b} = ?";
        return (equation, answer);
    }

    public static (string equation, int answer) GenerateMedium()
    {
        int answer = UnityEngine.Random.Range(10, 50);

        int b = UnityEngine.Random.Range(2, 9);
        int a = answer * b;

        string equation = $"{a} / {b} = ?";
        return (equation, answer);
    }
}