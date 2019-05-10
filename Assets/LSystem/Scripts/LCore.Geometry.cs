using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Tree.Scripts.Abstract;
using Assets.Tree.Scripts.Abstract.Lindenmayer;

namespace Assets.Tree.Scripts
{

    namespace Geometry
    {

        namespace CustomExtensions
        {

            public static class Vector3Extension
            {

                public static string ToStringEx(this Vector3 vector, int digits = 3)
                {
                    return "(" + Math.Round((decimal)vector.x, digits) + ", " + Math.Round((decimal)vector.z, digits) + ", " + Math.Round((decimal)vector.y, digits) + ")";
                }

                public enum Plane
                {

                    XZ = 1,
                    XY = 2,
                    YZ = 3

                }

                public static Vector3 ProjectOnto(this Vector3 vector, Plane plane)
                {
                    return new Vector3(plane == Plane.YZ ? 0 : vector.x, plane == Plane.XZ ? 0 : vector.y, plane == Plane.XY ? 0 : vector.z);
                }

            }

        }

        public class Generator
        {

            public Meshes.Manager manager;

            public Generator()
            {
                manager = new Meshes.Manager();
            }

            public Mesh BoxHexUV()
            {
                return BoxHexUV(1, 1, 1);
            }

            public Mesh BoxHexUV(float width, float height, float depth)
            {
                return BoxHexUV(width, height, depth, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh BoxHexUV(float width, float height, float depth, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.TileHex(1, 1), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, -depth / 2f), Quaternion.identity, new Vector3(1, 1, 1)), //front
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(depth, height), Providers.Quad.UVCoordinates.TileHex(2, 1), Providers.Quad.Triangles.Simple(),
                        new Vector3(width / 2f, 0, 0), Quaternion.Euler(0, -90, 0), new Vector3(1, 1, 1)), //right
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(depth, height), Providers.Quad.UVCoordinates.TileHex(0, 1), Providers.Quad.Triangles.Simple(),
                        new Vector3(-width / 2f, 0, 0), Quaternion.Euler(0, 90, 0), new Vector3(1, 1, 1)), //left
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.TileHex(2, 0), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, depth / 2f), Quaternion.Euler(0, 180, 0), new Vector3(1, 1, 1)), //back
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, depth), Providers.Quad.UVCoordinates.TileHex(1, 0), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, height / 2f, 0), Quaternion.Euler(90, 0, 0), new Vector3(1, 1, 1)), //top
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, depth), Providers.Quad.UVCoordinates.TileHex(0, 0), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, -height / 2f, 0), Quaternion.Euler(-90, 0, 0), new Vector3(1, 1, 1)) //bottom
                }, false, position, rotation, scale);
            }

            public Mesh BoxOneUV()
            {
                return BoxOneUV(1, 1, 1);
            }

            public Mesh BoxOneUV(float width, float height, float depth)
            {
                return BoxOneUV(width, height, depth, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh BoxOneUV(float width, float height, float depth, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, -depth / 2f), Quaternion.identity, new Vector3(1, 1, 1)), //front
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(depth, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(width / 2f, 0, 0), Quaternion.Euler(0, -90, 0), new Vector3(1, 1, 1)), //right
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(depth, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(-width / 2f, 0, 0), Quaternion.Euler(0, 90, 0), new Vector3(1, 1, 1)), //left
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, depth / 2f), Quaternion.Euler(0, 180, 0), new Vector3(1, 1, 1)), //back
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, depth), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, height / 2f, 0), Quaternion.Euler(90, 0, 0), new Vector3(1, 1, 1)), //top
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, depth), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, -height / 2f, 0), Quaternion.Euler(-90, 0, 0), new Vector3(1, 1, 1)) //bottom
                }, false, position, rotation, scale);
            }

            public Mesh DoubleQuad()
            {
                return DoubleQuad(1, 1);
            }

            public Mesh DoubleQuad(float width, float height)
            {
                return DoubleQuad(width, height, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh DoubleQuad(float width, float height, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1)),
                    new Meshes.Meshpart(Providers.Quad.Vertices.Simple(width, height), Providers.Quad.UVCoordinates.Simple(), Providers.Quad.Triangles.Simple(),
                        new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0), new Vector3(1, 1, 1))
                }, false, position, rotation, scale);
            }

            public Mesh Tube(int quality)
            {
                return Tube(quality, 1, 1, 1);
            }

            public Mesh Tube(int quality, float radius1, float radius2, float height)
            {
                return Tube(quality, radius1, radius2, height, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Tube(int quality, float radius1, float radius2, Vector3 start, Vector3 end)
            {
                return Tube(quality, radius1, radius2, start, end, Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Tube(int quality, float radius1, float radius2, float height, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                Tupel<Vector3[], Vector3[]> verticesAndNormals = Providers.Tube.Vertices.Wrap(quality, radius1, radius2, height);
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(verticesAndNormals.First, verticesAndNormals.Second,
                        Providers.Tube.UVCoordinates.Wrap(quality), 
                        Providers.Tube.Triangles.Wrap(quality), 
                        new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1))
                }, true, position, rotation, scale);
            }

            public Mesh Tube(int quality, float radius1, float radius2, Vector3 start, Vector3 end, Quaternion rotation, Vector3 scale)
            {
                Vector3 direction = (end - start).normalized;
                return Tube(quality, radius1, radius2,
                    Vector3.Distance(start, end),
                    Vector3.Lerp(start, end, 0.5f),
                    Quaternion.FromToRotation(Vector3.up, direction) * rotation,
                    scale);
            }

            public Mesh Cone(int quality)
            {
                return Cone(quality, 1, 1);
            }

            public Mesh Cone(int quality, float radius, float height)
            {
                return Cone(quality, radius, height, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Cone(int quality, float radius, Vector3 start, Vector3 end)
            {
                return Cone(quality, radius, start, end, Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Cone(int quality, float radius, float height, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                Tupel<Vector3[], Vector3[]> verticesAndNormals = Providers.Cone.Vertices.Wrap(quality, radius, height);
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(verticesAndNormals.First, verticesAndNormals.Second,
                        Providers.Cone.UVCoordinates.Wrap(quality), 
                        Providers.Cone.Triangles.Wrap(quality), 
                        new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1))
                }, true, position, rotation, scale);
            }

            public Mesh Cone(int quality, float radius, Vector3 start, Vector3 end, Quaternion rotation, Vector3 scale)
            {
                Vector3 direction = (end - start).normalized;
                return Cone(quality, radius,
                    Vector3.Distance(start, end),
                    Vector3.Lerp(start, end, 0.5f),
                   Quaternion.FromToRotation(Vector3.up, direction) * rotation,
                    scale);
            }

            public Mesh Sphere(int quality)
            {
                return Sphere(quality, 1);
            }

            public Mesh Sphere(int quality, float radius)
            {
                return Sphere(quality, radius, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Sphere(int quality, float radius, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                Tupel<Vector3[], Vector3[]> verticesAndNormals = Providers.Sphere.Vertices.Wrap(quality, radius);
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(verticesAndNormals.First, verticesAndNormals.Second, 
                        Providers.Sphere.UVCoordinates.Wrap(quality), 
                        Providers.Sphere.Triangles.Wrap(quality), 
                        new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1))
                }, true, position, rotation, scale);
            }

            public Mesh Dome(int quality)
            {
                return Dome(quality, 1);
            }

            public Mesh Dome(int quality, float radius)
            {
                return Dome(quality, radius, new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
            }

            public Mesh Dome(int quality, float radius, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                Tupel<Vector3[], Vector3[]> verticesAndNormals = Providers.Dome.Vertices.Wrap(quality, radius);
                return manager.Merge(new Meshes.Meshpart[] { 
                    new Meshes.Meshpart(verticesAndNormals.First, verticesAndNormals.Second, 
                        Providers.Dome.UVCoordinates.Wrap(quality), 
                        Providers.Dome.Triangles.Wrap(quality), 
                        new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1))
                }, true, position, rotation, scale);
            }

            public Mesh LTree(LindenmayerConfiguration config)
            {
                System.Random random = new System.Random(config.Seed);
                StaticParameterIn staticParameterIn = new StaticParameterIn(config.Iterations, random);
                PreProcessor preProcessor = new PreProcessor(config.SymbolMeanings, config.ReplaceRules, random);
                List<Token> tokens = preProcessor.ExpandTokenize(config.StartValue, config.Iterations, staticParameterIn);
                Interpreter interpreter = new Interpreter(config.StaticFunctions, config.DefaultInstructionValues);
                return interpreter.Generate(tokens, config.InitialState, staticParameterIn);;
            }
        }

        namespace Providers
        {

            using Geometry.CustomExtensions;

            namespace Quad
            {

                public class Vertices
                {

                    public static Vector3[] Simple(float width, float height)
                    {
                        return new Vector3[] { 
                            new Vector3(-width / 2f, height / 2f, 0), 
                            new Vector3(width / 2f, height / 2f, 0), 
                            new Vector3(width / 2f, -height / 2f, 0), 
                            new Vector3(-width / 2f, -height / 2f, 0) 
                        };
                    }

                }

                public class UVCoordinates
                {

                    public static Vector2[] Simple()
                    {
                        return new Vector2[] { 
                            new Vector2(0, 1), 
                            new Vector2(1, 1), 
                            new Vector2(1, 0), 
                            new Vector2(0, 0)
                        };
                    }

                    public static Vector2[] Tile(int x, int y, int qualityX, int qualityY)
                    {
                        float sizeX = 1f / qualityX;
                        float sizeY = 1f / qualityY;
                        return new Vector2[] { 
                            new Vector2(x * sizeX, 1f - (y * sizeY)),
                            new Vector2((x + 1f) * sizeX, 1f - (y * sizeY)),
                            new Vector2((x + 1f) * sizeX, 1f - ((y + 1f) * sizeY)),
                            new Vector2(x * sizeX, 1f - ((y + 1f) * sizeY))
                        };
                    }

                    public static Vector2[] TileHex(int x, int y)
                    {
                        return Tile(x, y, 3, 2);
                    }

                }

                public class Triangles
                {

                    public static int[] Simple()
                    {
                        return new int[] { 0, 1, 2, 2, 3, 0 };
                    }

                }

            }

            namespace Tube
            {

                public class Vertices
                {

                    public static Tupel<Vector3[], Vector3[]> Wrap(int quality, float radius1, float radius2, float height)
                    {
                        Vector3[] data = new Vector3[(quality + 1) * 2];
                        Vector3[] dataNormals = new Vector3[(quality + 1) * 2];
                        float angleStep = Mathf.PI * 2 / quality;
                        int dataBufferPosition = 0;
                        for (int i = -1; i <= 1; i += 2)
                        {
                            float radius = (i == -1 ? radius1 : radius2);
                            for (int j = 0; j < quality; j++)
                            {
                                Vector3 newPoint = new Vector3(Mathf.Cos(j * angleStep) * radius, i * (height / 2f), Mathf.Sin(j * angleStep) * radius);
                                data[dataBufferPosition] = newPoint;
                                dataNormals[dataBufferPosition] = newPoint.ProjectOnto(Vector3Extension.Plane.XZ).normalized;
                                dataBufferPosition++;
                            }
                            data[dataBufferPosition] = data[dataBufferPosition - quality];
                            dataNormals[dataBufferPosition] = data[dataBufferPosition - quality].ProjectOnto(Vector3Extension.Plane.XZ).normalized;
                            dataBufferPosition++;
                        }
                        return new Tupel<Vector3[], Vector3[]>() { First = data, Second = dataNormals };
                    }

                }

                public class UVCoordinates
                {

                    public static Vector2[] Wrap(int quality)
                    {
                        Vector2[] data = new Vector2[(quality + 1) * 2];
                        float step = 1f / quality;
                        int dataBufferPosition = 0;
                        for (int i = 0; i <= 1; i++)
                        {
                            for (int j = 0; j < quality; j++)
                            {
                                data[dataBufferPosition] = new Vector2(j * step, i);
                                dataBufferPosition++;
                            }
                            data[dataBufferPosition] = new Vector2(quality * step, i);
                            dataBufferPosition++;
                        }
                        return data;
                    }


                }

                public class Triangles
                {

                    public static int[] Wrap(int quality)
                    {
                        int[] data = new int[quality * 6];
                        int dataBufferPosition = -1;
                        for (int i = 0; i < quality; i++)
                        {
                            int pivotIndex = quality + i + 1;
                            data[++dataBufferPosition] = i;
                            data[++dataBufferPosition] = pivotIndex;
                            data[++dataBufferPosition] = i + 1;
                            data[++dataBufferPosition] = i + 1;
                            data[++dataBufferPosition] = pivotIndex;
                            data[++dataBufferPosition] = pivotIndex + 1;
                        }
                        return data;
                    }

                }

            }

            namespace Cone
            {

                public class Vertices
                {

                    public static Tupel<Vector3[], Vector3[]> Wrap(int quality, float radius, float height)
                    {
                        Vector3[] data = new Vector3[quality + 2];
                        Vector3[] dataNormals = new Vector3[(quality + 1) + 1];
                        float angleStep = Mathf.PI * 2 / quality;
                        int dataBufferPosition = 0;
                        for (int j = 0; j < quality; j++)
                        {
                            Vector3 newPoint = new Vector3(Mathf.Cos(j * angleStep) * radius, -height / 2f, Mathf.Sin(j * angleStep) * radius);
                            data[dataBufferPosition] = newPoint;
                            dataNormals[dataBufferPosition] = newPoint.ProjectOnto(Vector3Extension.Plane.XZ).normalized;
                            dataBufferPosition++;
                        }
                        data[dataBufferPosition] = data[dataBufferPosition - quality];
                        dataNormals[dataBufferPosition] = data[dataBufferPosition - quality].ProjectOnto(Vector3Extension.Plane.XZ).normalized;
                        dataBufferPosition++;
                        data[dataBufferPosition] = new Vector3(0, height / 2f, 0);
                        dataNormals[dataBufferPosition] = Vector3.up;
                        return new Tupel<Vector3[], Vector3[]>() { First = data, Second = dataNormals };
                    }

                }

                public class UVCoordinates
                {

                    public static Vector2[] Wrap(int quality)
                    {
                        Vector2[] data = new Vector2[quality + 2];
                        float step = 1f / quality;
                        int dataBufferPosition = 0;
                        for (int j = 0; j < quality; j++)
                        {
                            data[dataBufferPosition] = new Vector2(j * step, 0);
                            dataBufferPosition++;
                        }
                        data[dataBufferPosition] = new Vector2(quality * step, 0);
                        dataBufferPosition++;
                        data[dataBufferPosition] = new Vector2(0.5f, 1);
                        return data;
                    }


                }

                public class Triangles
                {

                    public static int[] Wrap(int quality)
                    {
                        int[] data = new int[quality * 3];
                        int dataBufferPosition = -1;
                        for (int i = 0; i < quality; i++)
                        {
                            data[++dataBufferPosition] = i;
                            data[++dataBufferPosition] = quality + 1;
                            data[++dataBufferPosition] = i + 1;
                        }
                        return data;
                    }

                }

            }

            namespace Sphere
            {

                public class Vertices
                {

                    public static Tupel<Vector3[], Vector3[]> Wrap(int quality, float radius)
                    {
                        Vector3[] data = new Vector3[(quality + 1) * (quality + 1)];
                        Vector3[] dataNormals = new Vector3[(quality + 1) * (quality + 1)];
                        float angleStep = Mathf.PI * 2 / quality;
                        int dataBufferPosition = 0;
                        for (int i = 0; i < quality; i++)
                        {
                            for (int j = quality; j >= 0; j--)
                            {
                                Vector3 newPoint = new Vector3(
                                    Mathf.Cos(i * angleStep) * Mathf.Sin(j * angleStep * 0.5f) * radius,
                                    Mathf.Cos(j * angleStep * 0.5f) * radius,
                                    Mathf.Sin(i * angleStep) * Mathf.Sin(j * angleStep * 0.5f) * radius);
                                data[dataBufferPosition] = newPoint;
                                dataNormals[dataBufferPosition] = newPoint.normalized;
                                dataBufferPosition++;
                            }
                        }
                        for (int j = 0; j <= quality; j++)
                        {
                            data[dataBufferPosition] = data[j];
                            dataNormals[dataBufferPosition] = data[j].normalized;
                            dataBufferPosition++;
                        }
                        return new Tupel<Vector3[], Vector3[]>() { First = data, Second = dataNormals };
                    }

                }

                public class UVCoordinates
                {

                    public static Vector2[] Wrap(int quality)
                    {
                        Vector2[] data = new Vector2[(quality + 1) * (quality + 1)];
                        float step = 1f / quality;
                        int dataBufferPosition = 0;
                        for (int i = 0; i <= quality; i++)
                        {
                            for (int j = 0; j <= quality; j++)
                            {
                                data[dataBufferPosition] = new Vector2(i * step, j * step);
                                dataBufferPosition++;
                            }
                        }
                         return data;
                    }

                }

                public class Triangles
                {

                    public static int[] Wrap(int quality)
                    {
                        int[] data = new int[3 * ((quality - 2) * quality * 2 + 2 * quality)];
                        int dataBufferPosition = -1;
                        for (int i = 0; i < quality; i++)
                        {
                            int offset = i * (quality + 1);
                            for (int j = 1; j < quality - 1; j++)
                            {
                                int pivotIndex = quality + j + 1;
                                data[++dataBufferPosition] = j + offset;
                                data[++dataBufferPosition] = j + 1 + offset;
                                data[++dataBufferPosition] = pivotIndex + offset;
                                data[++dataBufferPosition] = pivotIndex + offset;
                                data[++dataBufferPosition] = j + 1 + offset;
                                data[++dataBufferPosition] = pivotIndex + 1 + offset;
                            }
                        }
                        for (int i = 0; i < quality; i++)
                        {
                            int offset = i * (quality + 1);
                            int pivotIndex = quality + 2;
                            data[++dataBufferPosition] = offset;
                            data[++dataBufferPosition] = 1 + offset;
                            data[++dataBufferPosition] = pivotIndex + offset;
                        }
                        for (int i = 0; i < quality; i++)
                        {
                            int offset = i * (quality + 1);
                            int pivotIndex = quality * 2;
                            data[++dataBufferPosition] = quality - 1 + offset;
                            data[++dataBufferPosition] = quality + offset;
                            data[++dataBufferPosition] = pivotIndex + offset;
                        }
                        return data;
                    }

                }

            }

            namespace Dome
            {

                public class Vertices
                {

                    public static Tupel<Vector3[], Vector3[]> Wrap(int quality, float radius)
                    {
                        Vector3[] data = new Vector3[(quality + 1) * ((quality / 2) + 1)];
                        Vector3[] dataNormals = new Vector3[(quality + 1) * ((quality / 2) + 1)];
                        float angleStep = Mathf.PI * 2 / quality;
                        int dataBufferPosition = 0;
                        for (int i = 0; i < quality; i++)
                        {
                            for (int j = quality / 2; j >= 0; j--)
                            {
                                Vector3 newPoint = new Vector3(
                                    Mathf.Cos(i * angleStep) * Mathf.Sin(j * angleStep * 0.5f) * radius,
                                    Mathf.Cos(j * angleStep * 0.5f) * radius,
                                    Mathf.Sin(i * angleStep) * Mathf.Sin(j * angleStep * 0.5f) * radius);
                                data[dataBufferPosition] = newPoint;
                                dataNormals[dataBufferPosition] = newPoint.normalized;
                                dataBufferPosition++;
                            }
                        }
                        for (int j = 0; j <= quality / 2; j++)
                        {
                            data[dataBufferPosition] = data[j]; 
                            dataNormals[dataBufferPosition] = data[j].normalized;
                            dataBufferPosition++;
                        }
                        return new Tupel<Vector3[], Vector3[]>() { First = data, Second = dataNormals };
                    }

                }

                public class UVCoordinates
                {

                    public static Vector2[] Wrap(int quality)
                    {
                        Vector2[] data = new Vector2[(quality + 1) * ((quality / 2) + 1)];
                        float step = 1f / quality;
                        int dataBufferPosition = 0;
                        for (int i = 0; i <= quality; i++)
                        {
                            for (int j = 0; j <= quality / 2; j++)
                            {
                                data[dataBufferPosition] = new Vector2(i * step, j * step * 2);
                                dataBufferPosition++;
                            }
                        }
                        return data;
                    }

                }

                public class Triangles
                {

                    public static int[] Wrap(int quality)
                    {
                        int[] data = new int[6 * (((quality / 2) - 1) * quality + 3 * quality)];
                        int dataBufferPosition = -1;
                        for (int i = 0; i < quality; i++)
                        {
                            int offset = i * ((quality / 2) + 1);
                            for (int j = 0; j < (quality / 2) - 1; j++)
                            {
                                int pivotIndex = (quality / 2) + j + 1;
                                data[++dataBufferPosition] = j + offset;
                                data[++dataBufferPosition] = j + 1 + offset;
                                data[++dataBufferPosition] = pivotIndex + offset;
                                data[++dataBufferPosition] = pivotIndex + offset;
                                data[++dataBufferPosition] = j + 1 + offset;
                                data[++dataBufferPosition] = pivotIndex + 1 + offset;
                            }
                        }
                        for (int i = 0; i < quality; i++)
                        {
                            int offset = i * ((quality / 2) + 1);
                            int pivotIndex = quality;
                            data[++dataBufferPosition] = (quality / 2) - 1 + offset;
                            data[++dataBufferPosition] = (quality / 2) + offset;
                            data[++dataBufferPosition] = pivotIndex + offset;
                        }
                        return data;
                    }

                }

            }

        }

        namespace Meshes
        {

            public class Meshpart
            {

                public Vector3[] Vertices;
                public Vector3[] Normals;
                public Vector2[] UVs;
                public int[] Triangles;

                public Meshpart(Vector3[] vertices, Vector2[] uvs, int[] triangles, Vector3 position, Quaternion rotation, Vector3 scale) : this(vertices, null, uvs, triangles, position, rotation, scale) { }

                public Meshpart(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] triangles, Vector3 position, Quaternion rotation, Vector3 scale)
                {
                    Vertices = vertices;
                    Normals = normals;
                    UVs = uvs;
                    Triangles = triangles;
                    for (int i = 0; i < Vertices.Length; i++)
                    {
                        Vertices[i].Scale(scale);
                        Vertices[i] = rotation * Vertices[i];
                        Vertices[i] = Vertices[i] + position;
                    }
                }
            }

            public class Manager
            {

                public Mesh Merge(Meshpart[] meshParts, bool hasNormals, Vector3 position, Quaternion rotation, Vector3 scale)
                {
                    int vertexCount = 0, TriangleCount = 0;
                    for (int i = 0; i < meshParts.Length; i++)
                    {
                        vertexCount += meshParts[i].Vertices.Length;
                        TriangleCount += meshParts[i].Triangles.Length;
                    }
                    Vector3[] newVertices = new Vector3[vertexCount];
                    Vector3[] newNormals = new Vector3[vertexCount];
                    Vector2[] newUVs = new Vector2[vertexCount];
                    int[] newTriangles = new int[TriangleCount];
                    int vertexBufferPosition = 0, TriangleBufferPosition = 0;
                    for (int i = 0; i < meshParts.Length; i++)
                    {
                        meshParts[i].Vertices.CopyTo(newVertices, vertexBufferPosition);
                        if (hasNormals) meshParts[i].Normals.CopyTo(newNormals, vertexBufferPosition);
                        meshParts[i].UVs.CopyTo(newUVs, vertexBufferPosition);
                        meshParts[i].Triangles.CopyTo(newTriangles, TriangleBufferPosition);
                        for (int j = TriangleBufferPosition; j < TriangleBufferPosition + meshParts[i].Triangles.Length; j++) newTriangles[j] += vertexBufferPosition;
                        vertexBufferPosition += meshParts[i].Vertices.Length;
                        TriangleBufferPosition += meshParts[i].Triangles.Length;
                    }
                    for (var i = 0; i < newVertices.Length; i++)
                    {
                        newVertices[i].Scale(scale);
                        newVertices[i] = rotation * newVertices[i];
                        newVertices[i] = newVertices[i] + position;
                    }
                    Mesh mesh = new Mesh();
                    mesh.vertices = newVertices;
                    mesh.uv = newUVs;
                    mesh.triangles = newTriangles;
                    mesh.RecalculateBounds();
                    if (hasNormals)
                    {
                        mesh.normals = newNormals;
                    }
                    else
                    {
                        mesh.RecalculateNormals();
                    }
                    return mesh;
                }

            }

        }

    }

}
