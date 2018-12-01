using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private TransformComponent cubeTransform;
        private TransformComponent cubeTransform2;
        private TransformComponent cubeTransform3;

        private ShaderEffectComponent cubeShader;

        private float colorGrade = 0;
        private float colorGradeLimit = 0;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to light green (intensities in R, G, B, A).
            RC.ClearColor = new float4(0.7f, 1.0f, 0.5f, 1.0f);

            // Create a scene with 3 cubes
            // The three components: one XForm, one Material and the Mesh
            cubeTransform = new TransformComponent {Scale = new float3(0.5f, 0.5f, 0.5f), Translation = new float3(0, 0, 0)};
            cubeShader = new ShaderEffectComponent
            { 
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (0, 0, 1), new float3 (1, 1, 1),  4)
            };
            
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            cubeTransform2 = new TransformComponent {Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 0)};
            var cubeShader2 = new ShaderEffectComponent
            { 
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (0, 0, 1), new float3 (1, 1, 1),  4)
            };
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            cubeTransform3 = new TransformComponent {Scale = new float3(1.5f, 1.5f, 1.5f), Translation = new float3(0, 0, 0)};
            var cubeShader3 = new ShaderEffectComponent
            { 
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (0, 0, 1), new float3 (1, 1, 1),  4)
            };
            var cubeMesh3 = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            // Assemble the cubes node containing the three components
            var cubeNode = new SceneNodeContainer();
            cubeNode.Components = new List<SceneComponentContainer>();
            cubeNode.Components.Add(cubeTransform);
            cubeNode.Components.Add(cubeShader);
            cubeNode.Components.Add(cubeMesh);

            var cubeNode2 = new SceneNodeContainer();
            cubeNode2.Components = new List<SceneComponentContainer>();
            cubeNode2.Components.Add(cubeTransform2);
            cubeNode2.Components.Add(cubeShader2);
            cubeNode2.Components.Add(cubeMesh2);

            var cubeNode3 = new SceneNodeContainer();
            cubeNode3.Components = new List<SceneComponentContainer>();
            cubeNode3.Components.Add(cubeTransform3);
            cubeNode3.Components.Add(cubeShader3);
            cubeNode3.Components.Add(cubeMesh3);

            // Create the scene containing the cubest
            _scene = new SceneContainer();
            _scene.Children = new List<SceneNodeContainer>();
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(cubeNode2);
            _scene.Children.Add(cubeNode3);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            // Animate the camera angle
            _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime/10 ;
            
            // Animate the cube
            cubeTransform.Translation = new float3(15, 9 * M.Sin(3 * TimeSinceStart), 0);
            cubeTransform2.Translation = new float3(0, 6 * M.Sin(3 * TimeSinceStart), 0);
            cubeTransform3.Translation = new float3(-20, 3 * M.Sin(3 * TimeSinceStart), 0);
            cubeTransform3.Scale = new float3(M.Sin(2 * TimeSinceStart), M.Sin(2 * TimeSinceStart), M.Sin(2 * TimeSinceStart));

            //color animation Blue/Red
            if(colorGrade >= 1){
                colorGradeLimit = -0.1f;
            }
            if(colorGrade <= 0){
                colorGradeLimit = 0.1f;
            }
            colorGrade =colorGrade+colorGradeLimit;
            cubeShader.Effect = SimpleMeshes.MakeShaderEffect(new float3(colorGrade,0,1.0f-colorGrade),new float3(0,0,0),0);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);


            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}