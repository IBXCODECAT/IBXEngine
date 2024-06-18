using IBX_Engine.Graphics;
using IBX_Engine.Graphics.Internal;
using IBX_Engine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static IBX_Engine.Mathematics.HexMath;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IBX_Engine.VoxelSystem
{
    /// <summary>
    /// 128 Side5 | 64 Side4 | 32 Side3 | 16 Side2 | 8 Side1 | 4 Side0 | 2 Top | 1 Bottom | 0 None
    /// </summary>
    [Flags]
    public enum VoxelFaceFlags : byte
    {
        //    = 0b_543210_TB
        NONE = 0b_000000_00,

        //    =  0b_543210_TB
        TOP = 0b_000000_01,
        BOTTOM = 0b_000000_10,

        //    = 0b_543210_TB
        SIDE0 = 0b_000001_00,
        SIDE1 = 0b_000010_00,
        SIDE2 = 0b_000100_00,
        SIDE3 = 0b_001000_00,
        SIDE4 = 0b_010000_00,
        SIDE5 = 0b_100000_00,
    }

    /// <summary>
    /// 128 (reserved) | 64 (reserved) | 32 (reserved) | 
    /// 16 FLAMABLE | 8 HAS_PHYSICS | 4 NO_COLLISION | 
    /// 2 TRANSPARENT | 1 UNBREAKABLE | 0 NONE
    /// </summary>
    [Flags]
    public enum VoxelPropretiesFLags : byte
    {
        NONE =           0b_00_000000,
        UNBREAKABLE =    0b_00_000001,
        TRANSPARENT =    0b_00_000010,
        NO_COLLISION =   0b_00_000100,
        HAS_PHYSICS =    0b_00_001000,
        FLAMMABLE =      0b_00_010000,
        
        // Reserved for future use
        Unused1 = 0b_00_100000,
        Unused2 = 0b_01_000000,
        Unused3 = 0b_10_000000,
    }

    public class Voxel
    {
        public bool IsActive { get; private set; } = true;

        public Vec3 VoxelCoordinate { get; private set; }

        /// <summary>
        /// Defines the faces that are visible on the voxel
        /// </summary>
        public VoxelFaceFlags FaceBitFlags { get; private set; }

        /// <summary>
        /// Defines the properties of the voxel
        /// </summary>
        public VoxelPropretiesFLags PropBitFlags { get; private set; }

        private readonly float[] VoxelVertices =
        [
            // Positions                                        Normals             Texture coords
            //====================BOTTOM HEXAGON FACES====================//

            //Triangle 1
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            HexPoints[1].X, -VOXEL_THICKNESS, HexPoints[1].Y,    0f, -1.0f, 0f,      0f, 1f,
            HexPoints[2].X, -VOXEL_THICKNESS, HexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,
            
            //Triangle 2
            HexPoints[2].X, -VOXEL_THICKNESS, HexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,
            HexPoints[3].X, -VOXEL_THICKNESS, HexPoints[3].Y,    0f, -1.0f, 0f,      1f, 0f,
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triangle 3
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            HexPoints[3].X, -VOXEL_THICKNESS, HexPoints[3].Y,    0f, -1.0f, 0f,      0f, 1f,
            HexPoints[4].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 4
            HexPoints[4].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,
            HexPoints[5].X, -VOXEL_THICKNESS, HexPoints[5].Y,    0f, -1.0f, 0f,      1f, 0f,
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triagnle 5
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, -1.0f, 0f,      0f, 0f,
            HexPoints[5].X, -VOXEL_THICKNESS, HexPoints[5].Y,    0f, -1.0f, 0f,      0f, 1f,
            HexPoints[6].X, -VOXEL_THICKNESS, HexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 6
            HexPoints[6].X, -VOXEL_THICKNESS, HexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,
            HexPoints[1].X, -VOXEL_THICKNESS, HexPoints[1].Y,    0f, -1.0f, 0f,      1f, 0f,
            HexPoints[0].X, -VOXEL_THICKNESS, HexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //====================TOP HEXAGON FACES====================//

            //Triangle 1
            HexPoints[2].X,  VOXEL_THICKNESS, HexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,
            HexPoints[1].X,  VOXEL_THICKNESS, HexPoints[1].Y,    0f,  1.0f, 0f,      0f, 1f,
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 2
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            HexPoints[3].X,  VOXEL_THICKNESS, HexPoints[3].Y,    0f,  1.0f, 0f,      1f, 0f,
            HexPoints[2].X,  VOXEL_THICKNESS, HexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 3
            HexPoints[4].X,  VOXEL_THICKNESS, HexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,
            HexPoints[3].X,  VOXEL_THICKNESS, HexPoints[3].Y,    0f,  1.0f, 0f,      0f, 1f,
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 4
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            HexPoints[5].X,  VOXEL_THICKNESS, HexPoints[5].Y,    0f,  1.0f, 0f,      1f, 0f,
            HexPoints[4].X,  VOXEL_THICKNESS, HexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 5
            HexPoints[6].X,  VOXEL_THICKNESS, HexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,
            HexPoints[5].X,  VOXEL_THICKNESS, HexPoints[5].Y,    0f,  1.0f, 0f,      0f, 1f,
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 6
            HexPoints[0].X,  VOXEL_THICKNESS, HexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            HexPoints[1].X,  VOXEL_THICKNESS, HexPoints[1].Y,    0f,  1.0f, 0f,      1f, 0f,
            HexPoints[6].X,  VOXEL_THICKNESS, HexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,

            //====================SIDE QUAD FACES====================//

            //SIDE 1 - Triangle 1
            HexPoints[1].X, -VOXEL_THICKNESS, HexPoints[1].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[1].X,  VOXEL_THICKNESS, HexPoints[1].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[2].X, -VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 1 - Triangle 2
            HexPoints[1].X, VOXEL_THICKNESS, HexPoints[1].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[2].X, VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[2].X, -VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 2 - Triangle 1
            HexPoints[2].X, -VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[2].X,  VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[3].X, -VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 2 - Triangle 2
            HexPoints[2].X, VOXEL_THICKNESS, HexPoints[2].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[3].X, VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[3].X, -VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 3 - Triangle 1
            HexPoints[3].X, -VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[3].X,  VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[4].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 3 - Triangle 2
            HexPoints[3].X, VOXEL_THICKNESS, HexPoints[3].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[4].X, VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[4].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 4 - Triangle 1
            HexPoints[4].X, -VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[4].X,  VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[5].X, -VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 4 - Triangle 2
            HexPoints[4].X, VOXEL_THICKNESS, HexPoints[4].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[5].X, VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[5].X, -VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 5 - Triangle 1
            HexPoints[5].X, -VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[5].X,  VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[6].X, -VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 5 - Triangle 2
            HexPoints[5].X, VOXEL_THICKNESS, HexPoints[5].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[6].X, VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[6].X, -VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 6 - Triangle 1
            HexPoints[6].X, -VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,         0f, 0f,
            HexPoints[6].X,  VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,         0f, 1f,
            HexPoints[1].X, -VOXEL_THICKNESS, HexPoints[1].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 6 - Triangle 2
            HexPoints[6].X, VOXEL_THICKNESS, HexPoints[6].Y,    0f, 0f, 0f,          0f, 1f,
            HexPoints[1].X, VOXEL_THICKNESS, HexPoints[1].Y,    0f, 0f, 0f,          1f, 1f,
            HexPoints[1].X, -VOXEL_THICKNESS, HexPoints[1].Y,   0f, 0f, 0f,         1f, 0f,
        ];
        
        // ====================BOTTOM HEXAGON FACES====================//
        private readonly uint[] BottomFaceIndices = [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 ];

        // =====================TOP HEXAGON FACES=====================//
        private readonly uint[] TopFaceIndices = [19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 ];

        // =====================SIDE HEXAGON FACES====================//
        private readonly uint[] Side0Indices = [36, 37, 38, 39, 40, 41];
        private readonly uint[] Side1Indices = [42, 43, 44, 45, 46, 47];
        private readonly uint[] Side2Indices = [48, 49, 50, 51, 52, 53];
        private readonly uint[] Side3Indices = [54, 55, 56, 57, 58, 59];
        private readonly uint[] Side4Indices = [60, 61, 62, 63, 64, 65];
        private readonly uint[] Side5Indices = [66, 67, 68, 69, 70, 71];

        private readonly List<uint> drawElementsList = [];
        public readonly uint[] DrawElements;

        /// <summary>
        /// This <see cref="VertexBufferObject"/> will contain all possible data required 
        /// for each <see cref="Voxel"/>, and will be shared between all voxels.
        /// </summary>
        private static readonly VertexBufferObject CONSTANT_VBO = new();

        /// <summary>
        /// This <see cref="VertexArrayObject"/> will be used to format the data from
        /// <see cref="CONSTANT_VBO"/> and will be shared between all voxels.
        /// </summary>
        private static readonly VertexArrayObject CONSTANT_VAO = new();

        private readonly ElementBufferObject ebo;

        public Voxel(Vec3 voxelCoordinate, VoxelFaceFlags faceFlags, VoxelPropretiesFLags propFlags)
        {
            VoxelCoordinate = voxelCoordinate;
            FaceBitFlags = faceFlags;
            PropBitFlags = propFlags;

            ebo = new();

            if (FaceBitFlags.Equals(VoxelFaceFlags.NONE))
            {
                IsActive = false;
                DrawElements = [];
            }
            else
            {
                // Add bottom and top faces
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.BOTTOM)) drawElementsList.AddRange(BottomFaceIndices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.TOP)) drawElementsList.AddRange(TopFaceIndices);

                // Add side faces
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE0)) drawElementsList.AddRange(Side0Indices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE1)) drawElementsList.AddRange(Side1Indices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE2)) drawElementsList.AddRange(Side2Indices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE3)) drawElementsList.AddRange(Side3Indices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE4)) drawElementsList.AddRange(Side4Indices);
                if (FaceBitFlags.HasFlag(VoxelFaceFlags.SIDE5)) drawElementsList.AddRange(Side5Indices);

                DrawElements = [.. drawElementsList];

                PrepareRender();
            }
        }

        private void PrepareRender()
        {
            Logger.Log("Creating Mesh");

            Texture mainTexture = new("Assets/trees.png");
            Texture secondaryTexture = new("Assets/pfp.png");

            ShaderProgram shader = ShaderProgram.CURRENT_BOUND_PROGRAM;

            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            mainTexture.Use(TextureUnit.Texture0);
            secondaryTexture.Use(TextureUnit.Texture1);

            CONSTANT_VBO.SetData(VoxelVertices, BufferUsageHint.StaticDraw);

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            int vertexLocation = shader.GetAttributeLocation("aPosition");
            CONSTANT_VAO.EnableVertexAttribute(vertexLocation);
            CONSTANT_VAO.VertexAttributePointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float));

            int normalLocation = shader.GetAttributeLocation("aNormal");
            CONSTANT_VAO.EnableVertexAttribute(normalLocation);
            CONSTANT_VAO.VertexAttributePointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            int texCoordLocation = shader.GetAttributeLocation("aUVCoord");
            CONSTANT_VAO.EnableVertexAttribute(texCoordLocation);
            CONSTANT_VAO.VertexAttributePointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            ebo.BufferData(DrawElements, BufferUsageHint.StaticDraw);
        }

        internal void RenderVoxel()
        {
            ShaderProgram shader = ShaderProgram.CURRENT_BOUND_PROGRAM;

            // Ensure the VBO and VAO are bound before rendering
            CONSTANT_VBO.Bind();
            CONSTANT_VAO.Bind();

            // Bind the specific EBO for this voxel
            ebo.Bind();

            // Set the model matrix for this voxel
            Matrix4 model = Matrix4.CreateTranslation(VoxelCoordinate.X, VoxelCoordinate.Y, VoxelCoordinate.Z);
            shader.SetMatrix4("model", model);

            // Draw the voxel
            GL.DrawElements(PrimitiveType.Triangles, DrawElements.Length, DrawElementsType.UnsignedInt, 0);
        }

        private const float VOXEL_THICKNESS = 0.5f;
    }
}
