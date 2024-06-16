﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using IBX_Engine.Graphics.Internal;

namespace IBX_Engine
{
    public class GameInstance(int width, int height, string title) : GameWindow(new GameWindowSettings(), new NativeWindowSettings()
    {
        ClientSize = new Vector2i(width, height),
        Title = title,
    })
    {

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            // We need to update the aspect ratio once the window has been resized.
            camera.AspectRatio = Size.X / (float)Size.Y;
        }

        /// <summary>
        /// Change the field of view of the camera based on the mouse wheel input.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (KeyboardState.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)args.Time; // Left
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)args.Time; // Right
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)args.Time; // Up
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)args.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (camera.FirstMove) // This bool variable is initially set to true.
            {
                camera.LastPosition = new Vector2(mouse.X, mouse.Y);
                camera.FirstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - camera.LastPosition.X;
                var deltaY = mouse.Y - camera.LastPosition.Y;
                camera.LastPosition = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        private readonly float[] vertices =
        [
            // Position          // Normal        // Texture coordinates
            -1.0f, -1.0f, 1.0f, 0.0f,  0.0f, 1.0f, 0.0f, 0.0f,
             1.0f, -1.0f, 1.0f, 0.0f,  0.0f, 1.0f, 1.0f, 0.0f,
             1.0f,  1.0f, 1.0f, 0.0f,  0.0f, 1.0f, 1.0f, 1.0f,
            -1.0f,  1.0f, 1.0f, 0.0f,  0.0f, 1.0f, 0.0f, 1.0f,
            // Back face
             1.0f,  1.0f, -1.0f, 0.0f,  0.0f, -1.0f, 1.0f, 1.0f,
             1.0f, -1.0f, -1.0f, 0.0f,  0.0f, -1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, -1.0f, 0.0f,  0.0f, -1.0f, 0.0f, 0.0f,
            -1.0f,  1.0f, -1.0f, 0.0f,  0.0f, -1.0f, 0.0f, 1.0f,
            // Left face
            -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
            -1.0f,  1.0f, -1.0f, -1.0f,  0.0f,  0.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, 0.0f, 0.0f,
            -1.0f, -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, 0.0f, 1.0f,
            // Right face
             1.0f, -1.0f, -1.0f, 1.0f,  0.0f,  0.0f, 0.0f, 0.0f,
             1.0f,  1.0f, -1.0f, 1.0f,  0.0f,  0.0f, 1.0f, 0.0f,
             1.0f,  1.0f,  1.0f, 1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
             1.0f, -1.0f,  1.0f, 1.0f,  0.0f,  0.0f, 0.0f, 1.0f,
            // Top face
             1.0f,  1.0f,  1.0f, 0.0f,  1.0f,  0.0f, 1.0f, 1.0f,
             1.0f,  1.0f, -1.0f, 0.0f,  1.0f,  0.0f, 1.0f, 0.0f,
            -1.0f,  1.0f, -1.0f, 0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -1.0f,  1.0f,  1.0f, 0.0f,  1.0f,  0.0f, 0.0f, 1.0f,
            // Bottom face
            -1.0f, -1.0f, -1.0f, 0.0f,  -1.0f,  0.0f, 0.0f, 0.0f,
             1.0f, -1.0f, -1.0f, 0.0f,  -1.0f,  0.0f, 1.0f, 0.0f,
             1.0f, -1.0f,  1.0f, 0.0f,  -1.0f,  0.0f, 1.0f, 1.0f,
            -1.0f, -1.0f,  1.0f, 0.0f,  -1.0f,  0.0f, 0.0f, 1.0f,
        ];

        private readonly uint[] indices = 
        [
            // Front face
            0, 1, 2,
            2, 3, 0,
            // Back face
            4, 5, 6,
            6, 7, 4,
            // Left face
            8, 9, 10,
            10, 11, 8,
            // Right face
            12, 13, 14,
            14, 15, 12,
            // Top face
            16, 17, 18,
            18, 19, 16,
            // Bottom face
            20, 21, 22,
            22, 23, 20,
        ];

        private VertexBufferObject vbo;
        private VertexArrayObject vao;
        private ElementBufferObject ebo;

        private ShaderProgram shader;

        private Texture mainTexture;
        private Texture secondaryTexture;

        private Camera camera;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            vao = new();
            vbo = new();
            ebo = new();

            mainTexture = new("Assets/trees.png");
            secondaryTexture = new("Assets/pfp.png");

            shader = new("Assets/shader.vert", "Assets/shader.frag");

            shader.Use();

            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            mainTexture.Use(TextureUnit.Texture0);
            secondaryTexture.Use(TextureUnit.Texture1);

            vbo.SetData(vertices, BufferUsageHint.StaticDraw);

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

            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            camera.Position = new Vector3(-1.5f, 0.5f, 2.0f);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {

            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.Identity;

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            shader.SetVector3("lightPos", new Vector3(2f, 3 * MathF.Abs(MathF.Sin((float)DateTime.Now.TimeOfDay.TotalSeconds)), -1.5f));
            shader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

            shader.SetVector3("viewPos", camera.Position);


            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            ebo.Dispose();
            vao.Dispose();
            vbo.Dispose();
            shader.Dispose();

            mainTexture.Dispose();
        }
    }
}
