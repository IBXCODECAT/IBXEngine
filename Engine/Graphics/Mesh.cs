using IBX_Engine.Mathematics;

namespace IBX_Engine.Graphics
{
    public class Mesh(Vec3[] vertices, uint[] indices, float[] uvs)
    {
        public Vec3[] Vertices { get; private set; } = vertices;
        public uint[] Indices { get; private set; } = indices;
        public float[] UVs { get; private set; } = uvs;
    }
}
