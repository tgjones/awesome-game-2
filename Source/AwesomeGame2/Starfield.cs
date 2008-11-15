using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2
{
    public class Starfield : DrawableGameComponent
    {
        private VertexDeclaration vertexDeclaration;
        private VertexBuffer vertexBuffer;
        private BasicEffect basicEffect;
        private Camera _camera;
        private int _count;
		
        public Starfield(Game game, Camera camera, int starCount)
            : base(game)
        {
            _camera = camera;
            _count = starCount;
        }

        protected void InitializeVertexBuffer(GraphicsDevice graphicsDevice)
        {
            // Init vertexList
            Random lRandom = new Random();
            VertexPositionColor[] vertexList = new VertexPositionColor[_count];

            // Create vertices
            for (int i = 0; i < _count; i++)
            {
                Vector3 lCoOrds = new Vector3(0.5f - (float)lRandom.NextDouble(), 0.5f - (float)lRandom.NextDouble(), 0.5f - (float)lRandom.NextDouble());
                lCoOrds.Normalize();
                lCoOrds *= 1000.0f;

                Vector3 lColor = new Vector3(0.2f + 0.7f * (float)lRandom.NextDouble());

                vertexList[i] = new VertexPositionColor(lCoOrds, new Color(lColor));
            }

            // Create vertex buffer
            this.vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColor.SizeInBytes * vertexList.Length, BufferUsage.None);
            this.vertexBuffer.SetData(vertexList);
        }

        protected override void LoadContent()
        {
            // Retrieve graphics device
            GraphicsDevice graphicsDevice = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;

            // Create declaration
            this.vertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);

            // Create effect
            this.basicEffect = new BasicEffect(graphicsDevice, null);
            this.basicEffect.VertexColorEnabled = true;

            // Init the vertex buffer
            this.InitializeVertexBuffer(graphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            // Retrieve graphicsdevice
            GraphicsDevice graphicsDevice = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
            {
                graphicsDevice.RenderState.DepthBufferEnable = true;
                graphicsDevice.RenderState.CullMode = CullMode.None;
            }

            // Set world, view and projection matrices
            basicEffect.World = Matrix.Identity;
            basicEffect.View = _camera.View;
            basicEffect.Projection = _camera.Projection;

            // Set the vertex declaration
            graphicsDevice.VertexDeclaration = this.vertexDeclaration;

            // Begin effects
            basicEffect.Begin();
            {
                // Loop through each pass
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    // Draw the primitives
                    graphicsDevice.Vertices[0].SetSource(this.vertexBuffer, 0, VertexPositionColor.SizeInBytes);
                    graphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, _count);

                    pass.End();
                }
            }
            basicEffect.End();

            base.Draw(gameTime);
        }
    } 
}
