using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using IBX_Engine.VoxelSystem;
using IBX_Engine.Mathematics;
using IBX_Engine.Graphics;

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

            camera.FOV -= e.OffsetY;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Check to see if the window is focused before processing input
            if (!IsFocused) return;

            // Check to see if the escape key is pressed and close the window if it is
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

            // Process the camera input
            camera.ProcessCameraInput(KeyboardState, MouseState, args);
        }

        private ShaderProgram shader;

        private Camera camera;

        private readonly List<Voxel> VoxelsToRender = [];

        private Vec3 LightPosition = new(2f, 3f, -1.5f);

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.3f, 0f, 0.3f, 1.0f);

            shader = new ShaderProgram("Assets/shader.vert", "Assets/shader.frag");
            shader.Use();

            VoxelFaceFlags all = VoxelFaceFlags.TOP ^ VoxelFaceFlags.BOTTOM ^ VoxelFaceFlags.SIDE0 ^ VoxelFaceFlags.SIDE2 ^ VoxelFaceFlags.SIDE4;

            VoxelsToRender.Add(new Voxel(Vec3.Zero, VoxelFaceFlags.TOP ^ VoxelFaceFlags.BOTTOM, VoxelPropretiesFLags.NONE));
            VoxelsToRender.Add(new Voxel(LightPosition, all, VoxelPropretiesFLags.NONE));

            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            camera = new(Vector3.UnitZ * 3, Size.X / (float)Size.Y)
            {
                Position = new Vector3(-1.5f, 0.5f, 2.0f)
            };

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            shader.SetVector3("lightPos", LightPosition.AsOpenTKVector());
            shader.SetVector3("lightColor", new Vector3(1f, 1f, 1f));
            shader.SetVector3("viewPos", camera.Position);

            foreach(Voxel voxel in VoxelsToRender)
            {
                voxel.RenderVoxel();
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }
    }
}