using System.Runtime.InteropServices;
namespace Mogre.Demo.BezierPatch
{
    using System;

    using Mogre;
    // Event handler to add ability to alter subdivision
    public class BezierApplication : Demo.ExampleApplication.Example
    {
        #region Fields
        
        protected PatchVertex[] patchCtlPoints;
        protected VertexDeclaration patchDecl;
        
        // Hack struct for test
        PatchMeshPtr patch;
        Pass patchPass;
        
        #endregion Fields
        
        #region Methods
        
        // Just override the mandatory create scene method
        public override void CreateScene()
        {
            // Set ambient light
            sceneMgr.AmbientLight = new ColourValue(0.2f, 0.2f, 0.2f);

            // Create a point light
            Light l = sceneMgr.CreateLight("MainLight");
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Type = Light.LightTypes.LT_DIRECTIONAL;
            l.SetDirection(-0.5f, -0.5f, 0);
            
            // Create patch
            patchDecl = HardwareBufferManager.Singleton.CreateVertexDeclaration();
            patchDecl.AddElement(0, 0, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_POSITION);
            patchDecl.AddElement(0, sizeof(float)*3, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_NORMAL);
            patchDecl.AddElement(0, sizeof(float)*6, VertexElementType.VET_FLOAT2, VertexElementSemantic.VES_TEXTURE_COORDINATES, 0);
            
            // Make a 3x3 patch for test
            patchCtlPoints = new PatchVertex[9];
            
            // Patch data
            patchCtlPoints[0] = new PatchVertex();
            patchCtlPoints[0].x = -500.0f; patchCtlPoints[0].y = 200.0f; patchCtlPoints[0].z = -500.0f;
            patchCtlPoints[0].nx = -0.5f; patchCtlPoints[0].ny = 0.5f; patchCtlPoints[0].nz = 0.0f;
            patchCtlPoints[0].u = 0.0f; patchCtlPoints[0].v = 0.0f;
            
            patchCtlPoints[1] = new PatchVertex();
            patchCtlPoints[1].x = 0.0f; patchCtlPoints[1].y = 500.0f; patchCtlPoints[1].z = -750.0f;
            patchCtlPoints[1].nx = 0.0f; patchCtlPoints[1].ny = 0.5f; patchCtlPoints[1].nz = 0.0f;
            patchCtlPoints[1].u = 0.5f; patchCtlPoints[1].v = 0.0f;
            
            patchCtlPoints[2] = new PatchVertex();
            patchCtlPoints[2].x = 500.0f; patchCtlPoints[2].y = 1000.0f; patchCtlPoints[2].z = -500.0f;
            patchCtlPoints[2].nx = 0.5f; patchCtlPoints[2].ny = 0.5f; patchCtlPoints[2].nz = 0.0f;
            patchCtlPoints[2].u = 1.0f; patchCtlPoints[2].v = 0.0f;
            
            patchCtlPoints[3] = new PatchVertex();
            patchCtlPoints[3].x = -500.0f; patchCtlPoints[3].y = 0.0f; patchCtlPoints[3].z = 0.0f;
            patchCtlPoints[3].nx = -0.5f; patchCtlPoints[3].ny = 0.5f; patchCtlPoints[3].nz = 0.0f;
            patchCtlPoints[3].u = 0.0f; patchCtlPoints[3].v = 0.5f;
            
            patchCtlPoints[4] = new PatchVertex();
            patchCtlPoints[4].x = 0.0f; patchCtlPoints[4].y = 500.0f; patchCtlPoints[4].z = 0.0f;
            patchCtlPoints[4].nx = 0.0f; patchCtlPoints[4].ny = 0.5f; patchCtlPoints[4].nz = 0.0f;
            patchCtlPoints[4].u = 0.5f; patchCtlPoints[4].v = 0.5f;
            
            patchCtlPoints[5] = new PatchVertex();
            patchCtlPoints[5].x = 500.0f; patchCtlPoints[5].y = -50.0f; patchCtlPoints[5].z = 0.0f;
            patchCtlPoints[5].nx = 0.5f; patchCtlPoints[5].ny = 0.5f; patchCtlPoints[5].nz = 0.0f;
            patchCtlPoints[5].u = 1.0f; patchCtlPoints[5].v = 0.5f;
            
            patchCtlPoints[6] = new PatchVertex();
            patchCtlPoints[6].x = -500.0f; patchCtlPoints[6].y = 0.0f; patchCtlPoints[6].z = 500.0f;
            patchCtlPoints[6].nx = -0.5f; patchCtlPoints[6].ny = 0.5f; patchCtlPoints[6].nz = 0.0f;
            patchCtlPoints[6].u = 0.0f; patchCtlPoints[6].v = 1.0f;
            
            patchCtlPoints[7] = new PatchVertex();
            patchCtlPoints[7].x = 0.0f; patchCtlPoints[7].y = 500.0f; patchCtlPoints[7].z = 500.0f;
            patchCtlPoints[7].nx = 0.0f; patchCtlPoints[7].ny = 0.5f; patchCtlPoints[7].nz = 0.0f;
            patchCtlPoints[7].u = 0.5f; patchCtlPoints[7].v = 1.0f;
            
            patchCtlPoints[8] = new PatchVertex();
            patchCtlPoints[8].x = 500.0f; patchCtlPoints[8].y = 200.0f; patchCtlPoints[8].z = 800.0f;
            patchCtlPoints[8].nx = 0.5f; patchCtlPoints[8].ny = 0.5f; patchCtlPoints[8].nz = 0.0f;
            patchCtlPoints[8].u = 1.0f; patchCtlPoints[8].v = 1.0f;
            
            patch = MeshManager.Singleton.CreateBezierPatch(
                "Bezier1", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                patchCtlPoints, patchDecl,
                3, 3, 5, 5, PatchSurface.VisibleSide.VS_BOTH,
                HardwareVertexBuffer.Usage.HBU_STATIC_WRITE_ONLY,
                HardwareIndexBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY,
                true,
                true);
            
            // Start patch at 0 detail
            patch.SetSubdivision(0.0f);
            // Create entity based on patch
            Entity patchEntity = sceneMgr.CreateEntity("Entity1", "Bezier1");
            
            MaterialPtr pMat = MaterialManager.Singleton.Create("TextMat",
                                                                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            pMat.GetTechnique(0).GetPass(0).CreateTextureUnitState( "BumpyMetal.jpg" );
            patchEntity.SetMaterialName("TextMat");
            patchPass = pMat.GetTechnique(0).GetPass(0);
            
            // Attach the entity to the root of the scene
            sceneMgr.RootSceneNode.AttachObject(patchEntity);
            
            camera.SetPosition(500,500, 1500);
            camera.LookAt(0,200,-300);
        }
        
        public override void DestroyScene()
        {
            base.DestroyScene();
            // free up the pointer before we shut down OGRE
            patch.Dispose();
            patch = null;
        }
        float factor = 0.0f;
        float timeLapse = 0.0f;
        
        bool wireframe = false;
        
        protected override bool ExampleApp_FrameStarted(FrameEvent evt)
        {
            if(!base.ExampleApp_FrameStarted(evt))
                return false;
            

            
            timeLapse += evt.timeSinceLastFrame;
            
            // Prgressively grow the patch
            if (timeLapse > 1.0f)
            {
                factor += 0.2f;
                
                if (factor > 1.0f)
                {
                    wireframe = !wireframe;
                    //  camera.PolygonMode = wireframe ? PolygonMode.PM_WIREFRAME : PolygonMode.PM_SOLID;
                    patchPass.PolygonMode = wireframe ? PolygonMode.PM_WIREFRAME : PolygonMode.PM_SOLID;
                    factor = 0.0f;
                }
                
                patch.SetSubdivision(factor);
                mDebugText = "Bezier subdivision factor: " + factor;
                timeLapse = 0.0f;
                
            }
            
            // Call default
            return true;
        }
        
        #endregion Methods
        
        #region Nested Types
        
        [StructLayout(LayoutKind.Explicit)]
        protected struct PatchVertex
        {
            #region Fields
            
            [FieldOffset(0)]
            public float x;
            [FieldOffset(4)]
            public float y;
            [FieldOffset(8)]
            public float z;
            [FieldOffset(12)]
            public float nx;
            [FieldOffset(16)]
            public float ny;
            [FieldOffset(20)]
            public float nz;
            [FieldOffset(24)]
            public float u;
            [FieldOffset(28)]
            public float v;
            
            #endregion Fields
        }
        
        #endregion Nested Types
    }
}
