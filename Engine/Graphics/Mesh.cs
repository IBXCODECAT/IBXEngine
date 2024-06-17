using IBX_Engine.Graphics.Internal;
using IBX_Engine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;

namespace IBX_Engine.Graphics
{
    public class Mesh
    {
        public uint[] Indices { get; private set; }

        public Mesh(float[] data, uint[] indices)
        {
            Logger.Log("Creating Mesh");

            VertexBufferObject vbo = new();
            VertexArrayObject vao = new();
            ElementBufferObject ebo = new();

            this.Indices = indices;

            Texture mainTexture = new("Assets/trees.png");
            Texture secondaryTexture = new("Assets/pfp.png");

            ShaderProgram shader = ShaderProgram.CURRENT_BOUND_PROGRAM;
            
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            mainTexture.Use(TextureUnit.Texture0);
            secondaryTexture.Use(TextureUnit.Texture1);

            vbo.SetData(data, BufferUsageHint.StaticDraw);

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            int vertexLocation = shader.GetAttributeLocation("aPosition");
            vao.EnableVertexAttribute(vertexLocation);
            vao.VertexAttributePointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float));

            int normalLocation = shader.GetAttributeLocation("aNormal");
            vao.EnableVertexAttribute(normalLocation);
            vao.VertexAttributePointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            int texCoordLocation = shader.GetAttributeLocation("aUVCoord");
            vao.EnableVertexAttribute(texCoordLocation);
            vao.VertexAttributePointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            ebo.BufferData(indices, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            ShaderProgram shader = ShaderProgram.CURRENT_BOUND_PROGRAM;

            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);


            Matrix4 model = Matrix4.CreateRotationZ(MathF.Sin((float)DateTime.Now.TimeOfDay.TotalSeconds));

            shader.SetMatrix4("model", model);
        }
    }
}
