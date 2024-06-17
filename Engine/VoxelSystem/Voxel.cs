using IBX_Engine.Graphics;
using IBX_Engine.Graphics.Internal;
using IBX_Engine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Defines the faces that are visible on the voxel
        /// </summary>
        public VoxelFaceFlags FaceBitFlags { get; private set; }

        /// <summary>
        /// Defines the properties of the voxel
        /// </summary>
        public VoxelPropretiesFLags PropBitFlags { get; private set; }

        private readonly float[] Vertices =
        [
            // Positions                                        Normals             Texture coords
            //====================BOTTOM HEXAGON FACES====================//

            //Triangle 1
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,
            
            //Triangle 2
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triangle 3
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 4
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //Triagnle 5
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, -1.0f, 0f,      0f, 0f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, -1.0f, 0f,      0f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,

            //Triangle 6
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, -1.0f, 0f,      1f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, -1.0f, 0f,      1f, 0f,
            hexPoints[0].X, -CELL_THICKNESS, hexPoints[0].Y,    0f, -1.0f, 0f,      0f, 0f,

            //====================TOP HEXAGON FACES====================//

            //Triangle 1
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 2
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 3
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 4
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f,  1.0f, 0f,      0f, 0f,

            //Triangle 5
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f,  1.0f, 0f,      0f, 1f,
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,

            //Triangle 6
            hexPoints[0].X,  CELL_THICKNESS, hexPoints[0].Y,    0f,  1.0f, 0f,      1f, 1f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f,  1.0f, 0f,      1f, 0f,
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f,  1.0f, 0f,      0f, 0f,

            //====================SIDE QUAD FACES====================//

            //SIDE 1 - Triangle 1
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[1].X,  CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 1 - Triangle 2
            hexPoints[1].X, CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[2].X, CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 2 - Triangle 1
            hexPoints[2].X, -CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[2].X,  CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 2 - Triangle 2
            hexPoints[2].X, CELL_THICKNESS, hexPoints[2].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[3].X, CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 3 - Triangle 1
            hexPoints[3].X, -CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[3].X,  CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 3 - Triangle 2
            hexPoints[3].X, CELL_THICKNESS, hexPoints[3].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[4].X, CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 4 - Triangle 1
            hexPoints[4].X, -CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[4].X,  CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 4 - Triangle 2
            hexPoints[4].X, CELL_THICKNESS, hexPoints[4].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[5].X, CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 5 - Triangle 1
            hexPoints[5].X, -CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[5].X,  CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 5 - Triangle 2
            hexPoints[5].X, CELL_THICKNESS, hexPoints[5].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[6].X, CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         1f, 0f,

            //SIDE 6 - Triangle 1
            hexPoints[6].X, -CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         0f, 0f,
            hexPoints[6].X,  CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,         0f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,         1f, 0f,

            //Side 6 - Triangle 2
            hexPoints[6].X, CELL_THICKNESS, hexPoints[6].Y,    0f, 0f, 0f,          0f, 1f,
            hexPoints[1].X, CELL_THICKNESS, hexPoints[1].Y,    0f, 0f, 0f,          1f, 1f,
            hexPoints[1].X, -CELL_THICKNESS, hexPoints[1].Y,   0f, 0f, 0f,         1f, 0f,
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

        private List<uint> drawElementsList = new();
        public readonly uint[] DrawElements;

        public Mesh VoxelMesh { get; private set; }

        public Voxel(VoxelFaceFlags faceFlags, VoxelPropretiesFLags propFlags)
        {
            FaceBitFlags = faceFlags;
            PropBitFlags = propFlags;

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

                DrawElements = drawElementsList.ToArray();
            }

            VoxelMesh = new(Vertices, DrawElements);
        }

        private static Vec2[] GetHexVertex(float radius)
        {
            // Define the angle between each vertex of the hexagon
            float angleIncrement = MathF.PI / 3.0f;

            // Initialize array to store vertices
            Vec2[] vertices = new Vec2[7];

            vertices[0] = new Vec2(0f, 0f);

            // Calculate each vertex
            for (int i = 0; i < 6; i++)
            {
                float angle = i * angleIncrement;
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices[i + 1] = new(x, y);
            }

            return vertices;
        }

        private static readonly Vec2[] hexPoints = GetHexVertex(1.0f);

        private const float CELL_THICKNESS = 0.5f;
    }
}
