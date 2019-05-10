using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Tree.Scripts.Geometry;
using Assets.Tree.Scripts.Geometry.Meshes;
using Assets.Tree.Scripts.Geometry.CustomExtensions;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.Tree.Scripts
{

    public class Tupel<T1, T2>
    {

        public T1 First;
        public T2 Second;

    }

    public class StochasticTupel<T>
    {

        public float Probability;
        public T Value;
        public float PropapilityBottom;
        public float PropapilityTop;

        public StochasticTupel(T value, float probability)
        {
            Probability = probability;
            Value = value;
        }

    }

    namespace Abstract
    {
        
        namespace Lindenmayer
        {
            
            public struct LindenmayerConfiguration
            {
                public Dictionary<char, Token> SymbolMeanings;
                public Dictionary<char, List<ConditionalStochasticList<string>>> ReplaceRules;
                public string StartValue;
                public Func<ParameterIn, IParameterOut>[] StaticFunctions;
                public Dictionary<char, IParameterOut> DefaultInstructionValues;
                public int Iterations;
                public int Seed;
                public TurtleState InitialState;
            }

            [Serializable]
            public class NamedFloat
            {
                [HideInInspector]
                public string Name;
                public float Value;

            }

            public class NamedFloatCoverter
            {

                public static Dictionary<string, float> ToDictionary(NamedFloat[] data)
                {
                    Dictionary<string, float> newData = new Dictionary<string, float>();
                    for (int i = 0; i < data.Length; i++) newData.Add(data[i].Name, data[i].Value);
                    return newData;
                }

            }

            public abstract class ILindenmayerConfigurationProvider
            {

                public abstract LindenmayerConfiguration Get(int Quality, int Seed, int Iterations, Dictionary<string, float> Options);

                public abstract NamedFloat[] PreConfig();

            }

            public class LindenmayerException : Exception 
            {

                public LindenmayerException(string message) : base(message) { }

            }

            public class ConditionalStochasticList<T> : IEnumerable<StochasticTupel<T>>
            {

                public List<StochasticTupel<T>> ValueList;
                public Func<ConditionParameter, bool> Condition;

                public ConditionalStochasticList(Func<ConditionParameter, bool> condition)
                {
                    ValueList = new List<StochasticTupel<T>>();
                    Condition = condition;
                }

                public void Add(StochasticTupel<T> data)
                {
                    ValueList.Add(data);
                }

                public IEnumerator<StochasticTupel<T>> GetEnumerator()
                {
                    return ValueList.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }

                public T GetRandom(System.Random random)
                {
                    float pivot = 0;
                    for (int i = 0; i < ValueList.Count; i++)
                    {
                        ValueList[i].PropapilityBottom = pivot;
                        ValueList[i].PropapilityTop = ValueList[i].Probability + pivot;
                        pivot += ValueList[i].Probability;
                    }
                    float randomNum = (float)random.NextDouble();
                    for (int i = 0; i < ValueList.Count; i++)
                    {
                        if (ValueList[i].PropapilityBottom <= randomNum && ValueList[i].PropapilityTop >= randomNum) return ValueList[i].Value;
                    }
                    return default(T);
                }

            }

            public class Token
            {

                public enum TokenType : int
                {
                    None = 0,
                    Variable = 1,
                    Rotate = 2,
                    Plot = 3,
                    OpenBranch = 4,
                    CloseBranch = 5
                }

                public TokenType Type;
                public int Parameter = -1;
                public int CurrentIteration;
                public char Symbol;

                public Token() { }

                public Token(TokenType type)
                {
                    Type = type;
                }

            }

            public class PreProcessor
            {

                public Dictionary<char, Token> SymbolMeanings;
                public Dictionary<char, List<ConditionalStochasticList<string>>> ReplaceRules;
                public char BraceOpen;
                public System.Random Random;

                public PreProcessor(Dictionary<char, Token> symbolMeanings, Dictionary<char, List<ConditionalStochasticList<string>>> replaceRules, System.Random random, char braceOpen = '(')
                {
                    SymbolMeanings = symbolMeanings;
                    ReplaceRules = replaceRules;
                    BraceOpen = braceOpen;
                    Random = random;
                }

                public List<Token> ExpandTokenize(string input, int maxIterations, StaticParameterIn staticParameterIn, int recusionCount = 0)
                {
                    List<Token> tokens = new List<Token>();
                    Token lastToken = new Token();
                    for (int i = 0; i < input.Length; i++)
                    {
                        char symbol = input[i];
                        if (SymbolMeanings.ContainsKey(symbol))
                        {
                            ConditionParameter conditionParameter = new ConditionParameter(recusionCount, staticParameterIn);
                            if (SymbolMeanings[symbol].Type != Token.TokenType.Variable)
                            {
                                lastToken = new Token(SymbolMeanings[symbol].Type);
                                lastToken.Symbol = symbol;
                                lastToken.CurrentIteration = recusionCount;
                                tokens.Add(lastToken);
                            }
                            else
                            {
                                if (ReplaceRules.ContainsKey(symbol))
                                {
                                    if (recusionCount < maxIterations)
                                    {
                                        for (int j = 0; j < ReplaceRules[symbol].Count; j++)
                                        {
                                            if (ReplaceRules[symbol][j].Condition(conditionParameter))
                                            {
                                                tokens.AddRange(ExpandTokenize(ReplaceRules[symbol][j].GetRandom(Random), maxIterations, staticParameterIn, recusionCount + 1));
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lastToken = new Token(Token.TokenType.None);
                                        lastToken.Symbol = symbol;
                                        lastToken.CurrentIteration = recusionCount;
                                        tokens.Add(lastToken);
                                    }
                                }
                                else
                                {
                                    lastToken = new Token(Token.TokenType.Plot);
                                    lastToken.Symbol = symbol;
                                    lastToken.CurrentIteration = recusionCount;
                                    tokens.Add(lastToken);
                                }
                            }
                        }
                        else if (symbol == BraceOpen)
                        {
                            string param = "";
                            while (++i < input.Length && "0123456789".Contains(input[i].ToString())) param += input[i];
                            lastToken.Parameter = Convert.ToInt32(param);
                        }
                        else
                        {
                            throw new LindenmayerException("Unknown symbol: " + symbol);
                        }
                    }
                    return tokens;
                }

            }

            public abstract class IInstruction
            {

                public Func<ParameterIn, IParameterOut> ModFunction;

                abstract public void ApplyTo(Turtle turtle, ParameterIn param);

            }

            public class RotateInstruction : IInstruction
            {

                public override void ApplyTo(Turtle turtle, ParameterIn param)
                {
                    RotateParameterOut result = (RotateParameterOut)param.Defaults;
                    if (ModFunction != null) result = (RotateParameterOut)ModFunction(param);
                    turtle.State.Rotation = Quaternion.Euler(result.Rotation) * turtle.State.Rotation;
                }

            }

            public class PlotInstruction : IInstruction
            {

                public override void ApplyTo(Turtle turtle, ParameterIn param)
                {
                    IParameterOut rawResult = (IParameterOut)param.Defaults;
                    if (ModFunction != null) rawResult = ModFunction(param);
                    if(rawResult.HasGeometry && !turtle.Meshes.ContainsKey(param.Symbol)) turtle.Meshes.Add(param.Symbol, new List<Mesh>());
                    if(param.Defaults.GetType() == typeof(TubeParameterOut))
                    {
                        TubeParameterOut result = (TubeParameterOut)rawResult;
                        Vector3 oldPosition = turtle.State.Position;
                        turtle.State.Position += (turtle.State.Rotation * Vector3.up) * result.Length;
                        turtle.State.Radius = result.Radius2;
                        if (result.HasGeometry)
                        {
                            Mesh mesh = null;
                            if (result.Radius1 != 0)
                            {
                                if (result.Radius2 != 0)
                                {
                                    mesh = turtle.MeshGenerator.Tube(result.Quality, result.Radius1, result.Radius2, oldPosition, turtle.State.Position);
                                }
                                else
                                {
                                    mesh = turtle.MeshGenerator.Cone(result.Quality, result.Radius1, oldPosition, turtle.State.Position);
                                }
                            }
                            else if (result.Radius2 != 0)
                            {
                                mesh = turtle.MeshGenerator.Cone(result.Quality, result.Radius2, oldPosition, turtle.State.Position, Quaternion.Euler(180, 0, 0), new Vector3(1, 1, 1));
                            }
                            if(mesh != null)
                            {
                                if (turtle.State.LastTubeMesh != null)
                                {
                                    Vector3[] meshVertices = mesh.vertices;
                                    Vector3[] meshNormals = mesh.normals;
                                    for (int i = 0; i < result.Quality + 1; i++)
                                    {
                                        meshVertices[i] = turtle.State.LastTubeMesh.vertices[result.Quality + 1 + i];
                                        meshNormals[i] = turtle.State.LastTubeMesh.normals[result.Quality + 1 + i];
                                    }
                                    mesh.vertices = meshVertices;
                                    mesh.normals = meshNormals;
                                }
                                turtle.State.LastTubeMesh = null;
                                if (mesh.vertexCount == (result.Quality + 1) * 2) turtle.State.LastTubeMesh = mesh;
                                turtle.Meshes[param.Symbol].Add(mesh);
                            }
                        }
                    }
                    else if (param.Defaults.GetType() == typeof(SphereParameterOut))
                    {
                        SphereParameterOut result = (SphereParameterOut)rawResult;
                        turtle.State.Radius = result.Radius;
                        if (result.HasGeometry) turtle.Meshes[param.Symbol].Add(turtle.MeshGenerator.Sphere(result.Quality, result.Radius, turtle.State.Position, turtle.State.Rotation, new Vector3(1, 1, 1)));
                    }
                    else if (param.Defaults.GetType() == typeof(QuadParameterOut))
                    {
                        QuadParameterOut result = (QuadParameterOut)rawResult;
                        if (result.HasGeometry) turtle.Meshes[param.Symbol].Add(turtle.MeshGenerator.DoubleQuad(result.Width, result.Height, turtle.State.Position + (turtle.State.Rotation * Vector3.up) * (result.Height / 2f), turtle.State.Rotation, new Vector3(1, 1, 1)));
                    }
                }

            }

            public class StaticParameterIn
            {

                public int MaxIterations;
                public System.Random Random;

                public StaticParameterIn(int maxIterations, System.Random random)
                {
                    MaxIterations = maxIterations;
                    Random = random;
                }

            }

            public struct ParameterIn
            {

                public int CurrentIteration;
                public float Radius;
                public char Symbol;
                public IParameterOut Defaults;
                public StaticParameterIn Statics;

            }

            public class ConditionParameter
            {

                public int CurrentIteration;
                public StaticParameterIn Statics;

                public ConditionParameter(int currentIteration, StaticParameterIn statics)
                {
                    CurrentIteration = currentIteration;
                    Statics = statics;
                }

            }

            public abstract class IParameterOut 
            {

                public bool HasGeometry;

            }

            public class RotateParameterOut : IParameterOut
            {

                public Vector3 Rotation;

                public RotateParameterOut(Vector3 rotation)
                {
                    Rotation = rotation;
                }

            }

            public class TubeParameterOut : IParameterOut
            {

                public int Quality;
                public float Radius1;
                public float Radius2;
                public float Length;

                public TubeParameterOut(bool hasGeometry, int quality, float r1, float r2, float length)
                {
                    HasGeometry = hasGeometry;
                    Quality = quality;
                    Radius1 = r1;
                    Radius2 = r2;
                    Length = length;
                }

            }

            public class SphereParameterOut : IParameterOut
            {

                public int Quality;
                public float Radius;

                public SphereParameterOut(bool hasGeometry, int quality, float r)
                {
                    HasGeometry = hasGeometry;
                    Quality = quality;
                    Radius = r;
                }

            }

            public class QuadParameterOut : IParameterOut
            {

                public float Width;
                public float Height;

                public QuadParameterOut(bool hasGeometry, float width, float height)
                {
                    HasGeometry = hasGeometry;
                    Width = width;
                    Height = height;
                }

            }

            public class Turtle
            {

                public TurtleState State;
                public Stack<TurtleState> StateStack;
                public SortedDictionary<char, List<Mesh>> Meshes;
                public Generator MeshGenerator;

                public Turtle(TurtleState state)
                {
                    State = state;
                    StateStack = new Stack<TurtleState>();
                    Meshes = new SortedDictionary<char, List<Mesh>>();
                    MeshGenerator = new Generator();
                }

            }

            public class TurtleState : ICloneable
            {

                public Vector3 Position;
                public Quaternion Rotation;
                public Mesh LastTubeMesh;
                public float Radius;

                public TurtleState(Vector3 position, Quaternion rotation, Mesh lastTubeMesh, float radius)
                {
                    Position = position;
                    Rotation = rotation;
                    LastTubeMesh = lastTubeMesh;
                    Radius = radius;
                }

                public object Clone()
                {
                    return new TurtleState(Position, Rotation, LastTubeMesh, Radius);
                }

            }

            public class Interpreter
            {

                public Func<ParameterIn, IParameterOut>[] StaticFunctions;
                public Dictionary<char, IParameterOut> DefaultInstructionValues;

                public Interpreter(Func<ParameterIn, IParameterOut>[] staticFunctions, Dictionary<char, IParameterOut> defaultInstructionValues)
                {
                    StaticFunctions = staticFunctions;
                    DefaultInstructionValues = defaultInstructionValues;
                }

                public Mesh Generate(List<Token> tokens, TurtleState initialState, StaticParameterIn staticParameterIn)
                {
                    Turtle turtle = new Turtle(initialState);
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        IInstruction instruction = null;
                        switch (tokens[i].Type)
                        {
                            case Token.TokenType.Plot:
                                instruction = new PlotInstruction();
                                break;

                            case Token.TokenType.Rotate:
                                instruction = new RotateInstruction();
                                break;
                        }
                        if (instruction != null)
                        {
                            if (tokens[i].Parameter < StaticFunctions.Length && tokens[i].Parameter >= 0) instruction.ModFunction = StaticFunctions[tokens[i].Parameter];
                            ParameterIn param = new ParameterIn();
                            if (DefaultInstructionValues.ContainsKey(tokens[i].Symbol))
                            {
                                param.Defaults = DefaultInstructionValues[tokens[i].Symbol];
                                param.Radius = turtle.State.Radius;
                                param.Symbol = tokens[i].Symbol;
                                param.CurrentIteration = tokens[i].CurrentIteration;
                                param.Statics = staticParameterIn;
                                instruction.ApplyTo(turtle, param);
                            }
                            else
                            {
                                throw new LindenmayerException("Missing default instruction value for: " + tokens[i].Symbol);
                            }
                            continue;
                        }
                        switch (tokens[i].Type)
                        {
                            case Token.TokenType.OpenBranch:
                                turtle.StateStack.Push((TurtleState)turtle.State.Clone());
                                break;

                            case Token.TokenType.CloseBranch:
                                turtle.State = turtle.StateStack.Pop();
                                break;
                        }
                    }
                    List<Mesh> subMeshes = new List<Mesh>();
                    foreach (KeyValuePair<char, List<Mesh>> meshes in turtle.Meshes)
                    {
                        CombineInstance[] combine = new CombineInstance[meshes.Value.Count];
                        for (int i = 0; i < meshes.Value.Count; i++) combine[i].mesh = meshes.Value[i];
                        Mesh subMesh = new Mesh();
                        subMesh.CombineMeshes(combine, true, false);
                        subMesh.RecalculateBounds();
                        subMeshes.Add(subMesh);
                    }
                    CombineInstance[] masterCombine = new CombineInstance[subMeshes.Count];
                    for (int i = 0; i < subMeshes.Count; i++) masterCombine[i].mesh = subMeshes[i];
                    Mesh result = new Mesh();
                    result.CombineMeshes(masterCombine, false, false);
                    result.RecalculateBounds();
                    result.name = " LProcedual";
                    Debug.Log("LTree: Generating completed");
                    return result;
                }

            }

        }

    }

}