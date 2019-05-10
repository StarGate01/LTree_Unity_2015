using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Tree.Scripts.Abstract;
using Assets.Tree.Scripts.Abstract.Lindenmayer;

namespace Assets.Tree.Scripts.LindenmayerConfigurationProviders
{

    public class Default : ILindenmayerConfigurationProvider
    {

        public override NamedFloat[] PreConfig()
        {
            return new NamedFloat[] { 
                new NamedFloat() { Name = "TrunkAngleFactor", Value = 0.2f },
                new NamedFloat() { Name = "BranchAngle", Value = 30 }
            };
        }

        public override LindenmayerConfiguration Get(int Quality, int Seed, int Iterations, Dictionary<string, float> cOptions)
        {
            LindenmayerConfiguration config = new LindenmayerConfiguration();
            config.SymbolMeanings = new Dictionary<char, Token>() 
                {
                    { '+', new Token(Token.TokenType.Rotate) },
                    { '-', new Token(Token.TokenType.Rotate) },
                    { '~', new Token(Token.TokenType.Rotate) },
                    { '*', new Token(Token.TokenType.Rotate) },
                    { '#', new Token(Token.TokenType.Rotate) },
                    { '$', new Token(Token.TokenType.Rotate) },
                    { '&', new Token(Token.TokenType.Rotate) },
                    { '§', new Token(Token.TokenType.Rotate) },
                    { '[', new Token(Token.TokenType.OpenBranch) },
                    { ']', new Token(Token.TokenType.CloseBranch) },
                    { 'A', new Token(Token.TokenType.Plot) },
                    { 'B', new Token(Token.TokenType.Variable) },
                    { 'C', new Token(Token.TokenType.Variable) },
                    { 'D', new Token(Token.TokenType.Plot) },
                    { 'E', new Token(Token.TokenType.Variable) }
                };
            config.DefaultInstructionValues = new Dictionary<char, IParameterOut>()
                {
                    { '+', new RotateParameterOut(new Vector3(0, 0, -cOptions["BranchAngle"])) },
                    { '-', new RotateParameterOut(new Vector3(0, 0, cOptions["BranchAngle"])) },
                    { '~', new RotateParameterOut(new Vector3(cOptions["BranchAngle"], 0, 0)) },
                    { '*', new RotateParameterOut(new Vector3(-cOptions["BranchAngle"], 0, 0)) },
                    { '#', new RotateParameterOut(new Vector3(0, cOptions["BranchAngle"], 0)) },
                    { '$', new RotateParameterOut(new Vector3(0, -cOptions["BranchAngle"], 0)) },
                    { '&', new RotateParameterOut(new Vector3(60, 180, 60)) },
                    { '§', new RotateParameterOut(new Vector3(60, 180, 60)) },
                    { 'A', new TubeParameterOut(true, Quality, 1f, 1f, 25) },
                    { 'D', new QuadParameterOut(true, 120, 120) }
                };
            config.StartValue = "A(1)B";
            config.ReplaceRules = new Dictionary<char, List<ConditionalStochasticList<string>>>()
                {
                    { 'B', new List<ConditionalStochasticList<string>>() { 
                        new ConditionalStochasticList<string>(delegate(ConditionParameter param) {
                            return (param.CurrentIteration < param.Statics.MaxIterations - 3);
                        }) { 
                            new StochasticTupel<string>("+(0)A(1)B", 0.164f),
                            new StochasticTupel<string>("-(0)A(1)B", 0.164f),
                            new StochasticTupel<string>("~(0)A(1)B", 0.164f),
                            new StochasticTupel<string>("*(0)A(1)B", 0.164f),
                            new StochasticTupel<string>("A(1)B", 0.164f),
                            new StochasticTupel<string>("[+A(1)B]A(1)B", 0.045f),
                            new StochasticTupel<string>("[-A(1)B]A(1)B", 0.045f),
                            new StochasticTupel<string>("[*A(1)B]A(1)B", 0.045f),
                            new StochasticTupel<string>("[~A(1)B]A(1)B", 0.045f)
                        },
                        new ConditionalStochasticList<string>(delegate(ConditionParameter param) {
                            return (param.CurrentIteration == param.Statics.MaxIterations - 3);
                        }) { 
                            new StochasticTupel<string>("A(1)[CC]A(1)[CC]", 1)
                        } }
                    },
                    { 'C', new List<ConditionalStochasticList<string>>() { 
                        new ConditionalStochasticList<string>(delegate(ConditionParameter param) {
                            return (param.CurrentIteration == param.Statics.MaxIterations - 2);
                        }) { 
                            new StochasticTupel<string>("[&(4)EE]", 1f)
                        } }
                    },
                    { 'E', new List<ConditionalStochasticList<string>>() { 
                        new ConditionalStochasticList<string>(delegate(ConditionParameter param) {
                            return (param.CurrentIteration == param.Statics.MaxIterations - 1);
                        }) { 
                            new StochasticTupel<string>("[&(4)D(3)]", 1f)
                        } }
                    }
                };
            config.StaticFunctions = new Func<ParameterIn, IParameterOut>[] 
                {  
                    delegate(ParameterIn param)
                    {
                        return new RotateParameterOut(((RotateParameterOut)param.Defaults).Rotation * cOptions["TrunkAngleFactor"]);
                    },
                    delegate(ParameterIn param)
                    {
                        TubeParameterOut defaults = (TubeParameterOut)param.Defaults;
                        return new TubeParameterOut(defaults.HasGeometry, defaults.Quality, 
                            param.Radius, param.Radius - 1, defaults.Length);
                    },
                    delegate(ParameterIn param)
                    {
                        SphereParameterOut defaults = (SphereParameterOut)param.Defaults;
                        return new SphereParameterOut(defaults.HasGeometry, defaults.Quality,
                            defaults.Radius * ((float)param.Statics.Random.NextDouble() + 0.5f));
                    },
                    delegate(ParameterIn param)
                    {
                        QuadParameterOut defaults = (QuadParameterOut)param.Defaults;
                        return new QuadParameterOut(defaults.HasGeometry, defaults.Width, defaults.Height * (1f + (float)param.Statics.Random.NextDouble() * 0.5f));
                    },
                    delegate(ParameterIn param)
                    {
                        return new RotateParameterOut(((RotateParameterOut)param.Defaults).Rotation * ((float)param.Statics.Random.NextDouble() - 0.5f));
                    },
                    delegate(ParameterIn param)
                    {
                        TubeParameterOut defaults = (TubeParameterOut)param.Defaults;
                        return new TubeParameterOut(defaults.HasGeometry, defaults.Quality, 
                            param.Radius, param.Radius - 1, defaults.Length * 2);
                    }
                };
            config.Iterations = Iterations;
            config.Seed = Seed;
            config.InitialState = new TurtleState(new Vector3(0, 0, 0), Quaternion.identity, null, Iterations);
            return config;
        }

    }

}