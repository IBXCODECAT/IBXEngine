using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBX_Engine.Mathematics
{
    internal static class HexMath
    {
        /// <summary>
        /// Calculates the vertices of a hexagon with the given radius.
        /// </summary>
        /// <param name="radius">The radius of the hexagon</param>
        /// <returns>An array of <see cref="Vec2"/> representing the 2D vertex position</returns>
        internal static Vec2[] GetHexVertex(float radius)
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

        internal static readonly Vec2[] HexPoints = GetHexVertex(1.0f);
    }
}
