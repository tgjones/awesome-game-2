using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sun : DrawableGameComponent
    {
        private float BlurIntensity;

        private SpriteBatch spriteBatch;
        private Model SunModel;
        private Effect fxAlbedo;
        private Effect fxLayers;
        private Effect fxHDR;
        private Effect fxLinearFilter;
        private Effect fxGaussianFilter;
        private RenderTarget2D rtColor;
        private RenderTarget2D rtLayers;
        private RenderTarget2D rtHDR;
        private RenderTarget2D rtGaussian;
        private ChainedRenderTarget rtcHDR;
        private TextureCube texSun;
        private TextureCube texLayer1;
        private TextureCube texLayer2;
        private Texture2D texGradient;

        private Matrix worldMatrix;
        private Vector3 vecRotationAxis;
        private Vector3 vecLayer1Axis;
        private Vector3 vecLayer2Axis;
        private float fRotationAngle;
        private float fLayer1Angle;
        private float fLayer2Angle;

        public Sun(Game game, float xOffset, float blurIntensity) : base(game)
        {
            worldMatrix = Matrix.CreateTranslation(xOffset, 0.0f, 0.0f);
            BlurIntensity = blurIntensity;
        }

        // -----------------------------------------------------------------
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related this.Game.Content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize( )
        {
            // arbitrary, but different axes
            vecRotationAxis = Vector3.Normalize( new Vector3( 1f, 2f, 1f ) );
            fRotationAngle = 0f;
            vecLayer1Axis = Vector3.Normalize( new Vector3( 3f, 1f, 1f ) );
            fLayer1Angle = 0f;
            vecLayer2Axis = Vector3.Normalize( new Vector3( 1f, 5f, 2f ) );
            fLayer2Angle = 0f;            

            base.Initialize( );
        }
        // -----------------------------------------------------------------
        /// <summary>
        /// Loadthis.Game.Content will be called once per game and is the place to load
        /// all of your this.Game.Content.
        /// </summary>
        protected override void LoadContent( )
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );

            // all effects we use
            fxAlbedo = this.Game.Content.Load<Effect>( "Effects\\DiffuseColor" );
            fxLayers = this.Game.Content.Load<Effect>( "Effects\\SunLayerCube" );
            fxHDR = this.Game.Content.Load<Effect>( "Effects\\SunLayerCombine" );
            fxLinearFilter = this.Game.Content.Load<Effect>( "Effects\\LinearFilter" );
            fxGaussianFilter = this.Game.Content.Load<Effect>( "Effects\\GaussianFilter" );

            // the only model we use
            SunModel = this.Game.Content.Load<Model>("Models\\sphere" );

            // textures as explained in the tutorial
            texSun = this.Game.Content.Load<TextureCube>( "Textures\\SunTexture" );
            texLayer1 = this.Game.Content.Load<TextureCube>( "Textures\\SunLayer1" );
            texLayer2 = this.Game.Content.Load<TextureCube>( "Textures\\SunLayer2" );
            texGradient = this.Game.Content.Load<Texture2D>( "Textures\\FireGradient" );

            // set up all rendertargets we use here
            CreateRenderTargets( );
        }
        // -----------------------------------------------------------------
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update( GameTime gameTime )
        {
            KeyboardState ks = Keyboard.GetState( );
            if( ks.IsKeyDown( Keys.W ) )
            {
                BlurIntensity += 0.25f;
            }
            if( ks.IsKeyDown( Keys.S ) )
            {
                BlurIntensity -= 0.25f;
            }

            // angles are increased with different timings
            // in order to create some "random" effect
            if ( fRotationAngle < MathHelper.Pi ) 
                fRotationAngle += 0.0005f;
            else 
                fRotationAngle = 0f;

            if ( fLayer1Angle < MathHelper.Pi ) 
                fLayer1Angle += 0.0005f;
            else 
                fLayer1Angle = 0f;

            if ( fLayer2Angle < MathHelper.Pi ) 
                fLayer2Angle += 0.0008f;
            else 
                fLayer2Angle = 0f;

            base.Update( gameTime );
        }
        // -----------------------------------------------------------------
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw( GameTime gameTime )
        {
            DrawSun();         // start by drawing diffuse color!
            DrawSunLayers( );   // then draw layers as described in tutorial
            ExtractHDR( );      // extract luminance values above threshold
            BlurScreen();      // blur only these very bright values

            // now we additively combine all rendertargets
            GraphicsDevice.SetRenderTarget( 0, null );
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin( 
                SpriteBlendMode.Additive,
                SpriteSortMode.Immediate,
                SaveStateMode.SaveState );

            spriteBatch.Draw( rtColor.GetTexture( ),
                Vector2.Zero,
                Color.White );

            spriteBatch.Draw( rtHDR.GetTexture( ),
                new Rectangle( 0, 0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                 ),
                Color.White );

            spriteBatch.End( );                       
      
            base.Draw( gameTime );
        }
        // -----------------------------------------------------------------
        private void BlurScreen( )
        {
            // this stuff is done with my rendertarget class
            rtcHDR.GenerateMipMapLevels( );            
            rtcHDR.ApplyBlur( ref fxGaussianFilter );
            rtcHDR.AdditiveBlend( );
        }
        // -----------------------------------------------------------------
        private void ExtractHDR( )
        {
            GraphicsDevice.SetRenderTarget(0, rtHDR);
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Textures[0] = rtLayers.GetTexture();
            fxHDR.CurrentTechnique = fxHDR.Techniques[ 0 ];
            fxHDR.Parameters[ "gfThreshold" ].SetValue( 1.7f );
            fxHDR.Parameters[ "gfBrightness" ].SetValue( BlurIntensity );
            fxHDR.Parameters[ "gGradientTexture" ].SetValue( texGradient );
            fxHDR.Begin( );
            foreach ( EffectPass pass in fxHDR.CurrentTechnique.Passes )
            {
                spriteBatch.Begin(
                    SpriteBlendMode.None,
                    SpriteSortMode.Immediate,
                    SaveStateMode.SaveState );
                pass.Begin( );
                spriteBatch.Draw( rtLayers.GetTexture( ),
                    new Rectangle( 0, 0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                ),
                    Color.White );
                pass.End( );
                spriteBatch.End( );
            } // foreach
            fxHDR.End( );
            GraphicsDevice.SetRenderTarget(0, null);
        }
        // -----------------------------------------------------------------
        private void DrawSunLayers( )
        {
            ICameraService camera = this.Game.Services.GetService<ICameraService>();

            GraphicsDevice.SetRenderTarget(0, rtLayers);
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RenderState.DepthBufferEnable = false;
            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.SourceBlend = Blend.One;
            GraphicsDevice.RenderState.DestinationBlend = Blend.One;

            foreach ( ModelMesh mesh in SunModel.Meshes )
            {
                foreach ( ModelMeshPart part in mesh.MeshParts )
                {
                    part.Effect = fxLayers;
                } // foreach
            } // foreach            
            
            Matrix World = Matrix.CreateFromAxisAngle(vecLayer1Axis, fLayer1Angle) * worldMatrix;

            foreach ( ModelMesh mesh in SunModel.Meshes )
            {
                foreach ( Effect effect in mesh.Effects )
                {
                    effect.CurrentTechnique = effect.Techniques[ 0 ];
                    effect.Parameters[ "gTexture" ].SetValue( texLayer1 );
                    effect.Parameters[ "World" ].SetValue( World );
                    effect.Parameters[ "View" ].SetValue( camera.View );
                    effect.Parameters[ "Projection" ].SetValue( camera.Projection );
                    effect.Parameters[ "WorldIT" ].SetValue( Matrix.Invert( World ) );
                } // foreach
                mesh.Draw( );
            } // foreach

            World = Matrix.CreateFromAxisAngle(vecLayer2Axis, fLayer2Angle) * worldMatrix;

            foreach ( ModelMesh mesh in SunModel.Meshes )
            {
                foreach ( Effect effect in mesh.Effects )
                {
                    effect.CurrentTechnique = effect.Techniques[ 0 ];
                    effect.Parameters[ "gTexture" ].SetValue( texLayer2 );
                    effect.Parameters[ "World" ].SetValue( World );
                    effect.Parameters[ "View" ].SetValue( camera.View );
                    effect.Parameters[ "Projection" ].SetValue( camera.Projection );
                    effect.Parameters[ "WorldIT" ].SetValue( Matrix.Invert( World ) );
                } // foreach
                mesh.Draw( );
            } // foreach

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
            GraphicsDevice.SetRenderTarget(0, null);
        }        
        // -----------------------------------------------------------------
        private void DrawSun( )
        {
            ICameraService camera = this.Game.Services.GetService<ICameraService>();
            
            GraphicsDevice.SetRenderTarget(0, rtColor);
            GraphicsDevice.Clear(Color.Black);

            foreach ( ModelMesh mesh in SunModel.Meshes )
            {
                foreach ( ModelMeshPart part in mesh.MeshParts )
                {
                    part.Effect = fxAlbedo;
                } // foreach
            } // foreach

            Matrix World = Matrix.CreateFromAxisAngle(vecRotationAxis, fRotationAngle) * worldMatrix;

            foreach ( ModelMesh mesh in SunModel.Meshes )
            {
                foreach ( Effect effect in mesh.Effects )
                {
                    effect.CurrentTechnique = effect.Techniques[ 0 ];
                    effect.Parameters[ "gAlbedoMap" ].SetValue( texSun );
                    effect.Parameters[ "gWorldXf" ].SetValue( World );
                    effect.Parameters[ "gViewXf" ].SetValue( camera.View );
                    effect.Parameters[ "gProjectionXf" ].SetValue( camera.Projection );
                    effect.Parameters[ "gWorldITXf" ].SetValue( Matrix.Invert( World ) );
                } // foreach
                mesh.Draw( );
            } // foreach

            GraphicsDevice.SetRenderTarget(0, null);
        }
        // -----------------------------------------------------------------
        private void CreateRenderTargets( )
        {
            rtColor = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                1,
                SurfaceFormat.HalfVector4 );

            rtLayers = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                1,
                SurfaceFormat.HalfVector4 );

            rtHDR = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                1,
                SurfaceFormat.HalfVector4 );

            rtGaussian = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                1,
                SurfaceFormat.HalfVector4 );

            // this is my own rendertarget class
            // it simply extends the XNA class by linear filtering
            rtcHDR = new ChainedRenderTarget(
                spriteBatch,
                fxLinearFilter,
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            // pointer to the rendertarget to create the blur
            rtcHDR.RenderTarget = rtHDR;
        }
        // -----------------------------------------------------------------
    }
}