/*
 * Dot3Bump demo
 * Contributor: Iscu 'linkerro' Radu
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mogre;

namespace Mogre.Demo.Dot3Bump
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            try
            {
                Dot3Bump App = new Dot3Bump();
                App.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
		}
	}
	
	class Dot3Bump : Demo.ExampleApplication.Example
	{
		#region Variable declarations
		private const int NUM_ENTITIES=3;
		private const int NUM_LIGHTS=3;
		
		private Entity[] mEntities=new Entity[NUM_ENTITIES];
		private string[] mEntityMeshes=new string[NUM_ENTITIES]
		{
			"athene.mesh",
			"knot.mesh",//"LightArmour.mesh",
			"ogrehead.mesh"
		};
		
		private int mCurrentEntity=0;
		private Light[] mLights=new Light[NUM_LIGHTS];
		private BillboardSet[] mLightFlareSets=new BillboardSet[NUM_LIGHTS];
		private Billboard[] mLightFlares=new Billboard[NUM_LIGHTS];
		private Vector3[] mLightPositions=new Vector3[NUM_LIGHTS]
		{
			new Vector3(300,0,0),
			new Vector3(-300,50,0),
			new Vector3(0,-300,-100)
		};
		private Radian[] mLightRotationAngles=new Radian[NUM_LIGHTS]
		{
			new Degree(0),
			new Degree(30),
			new Degree(75)
		};
		private Vector3[] mLightRotationAxes=new Vector3[NUM_LIGHTS]
		{
			Vector3.UNIT_X,
			Vector3.UNIT_Y,
			Vector3.UNIT_Z
		};
		private float[] mLightSpeeds=new float[NUM_LIGHTS]{30,10,50};
		private ColourValue[] mDiffuseLightColours=new ColourValue[]
		{
			new ColourValue(1f,1f,1f),
			new ColourValue(1f,0f,0f),
			new ColourValue(1f,1f,0.5f)
		};
		private ColourValue[] mSpecularLightColours=new ColourValue[NUM_LIGHTS]
		{
			new ColourValue(1f,1f,1f),
			new ColourValue(1f,0.8f,0.8f),
			new ColourValue(1f,1f,0.8f)
		};
		private bool[] mLightState=new bool[NUM_LIGHTS]
		{
			true,
			true,
			false
		};
		private const int NUM_MATERIALS=4;
		private string[][] mMaterialNames=new string[NUM_ENTITIES][]
		{
			new string[]
			{
			    "Examples/Athene/Basic",
			    "Examples/Athene/NormalMapped",
			    "Examples/Athene/NormalMappedSpecular",
			    "Examples/Athene/NormalMapped"
			},
			new string[]
			{
			    "Examples/BumpMapping/SingleLight",
			    "Examples/BumpMapping/MultiLight",
			    "Examples/BumpMapping/MultiLightSpecular",
			    "Examples/OffsetMapping/Specular"
			},
			new string[]
			{
			    "Examples/BumpMapping/SingleLight",
			    "Examples/BumpMapping/MultiLight",
			    "Examples/BumpMapping/MultiLightSpecular",
			    "Examples/OffsetMapping/Specular"
			}
		};
		private int mCurrentMaterial=1;
		private SceneNode mMainNode;
		private SceneNode[] mLightNodes=new SceneNode[NUM_LIGHTS];
		private SceneNode[] mLightPivots=new SceneNode[NUM_LIGHTS];
		
		private OverlayElement mObjectInfo;
		private OverlayElement mMaterialInfo;
		private OverlayElement mInfo;
		private float timeDelay=0;
		private System.DateTime timeo,timem,time1,time2,time3=DateTime.Now;
		//number of ticks between two timestamps wellow wich a key action shold not occur
		//this is a completly arbitrary number; not shure if this is machine independent
		private const int KEY_TIME=5000000;

		
		//protected SceneNode mpObjsNode; //the node wich will hold our entities
		#endregion
		
		public Dot3Bump(){}
				
		void flipLightState(int i)
		{
			//mLightState[i]=!mLightState[i];
			mLights[i].Visible=!mLights[i].Visible;
			//mLightFlareSets[i].Visible=mLights[i].Visible;
		}
				
		public override void CreateFrameListener()
		{
			base.CreateFrameListener();
			root.FrameStarted+= new FrameListener.FrameStartedHandler(frameStarted);
		}
				
		bool frameStarted(FrameEvent ev)
		{
			//if(!ExampleApp_FrameStarted(ev))
			//	return false;
			timeDelay-=ev.timeSinceLastFrame;
			

			//switch meshes
			if(inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_O) && TimeCheck(ref timeo))
			{
				timeDelay=1;
				mEntities[mCurrentEntity].Visible=false;
				mCurrentEntity=(++mCurrentEntity)%NUM_ENTITIES;
				mEntities[mCurrentEntity].Visible=true;
				mEntities[mCurrentEntity].SetMaterialName(mMaterialNames[mCurrentEntity][mCurrentMaterial]);
				mObjectInfo.Caption="Current: "+mEntityMeshes[mCurrentEntity];
				mMaterialInfo.Caption="Current: "+mMaterialNames[mCurrentEntity][mCurrentMaterial];
			}
			
			//switch materials
            if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_M) && TimeCheck(ref timem))
			{
				timeDelay=1;
				mCurrentMaterial=(++mCurrentMaterial)%NUM_MATERIALS;
				mEntities[mCurrentEntity].SetMaterialName(mMaterialNames[mCurrentEntity][mCurrentMaterial]);
				mMaterialInfo.Caption="Current: "+mMaterialNames[mCurrentEntity][mCurrentMaterial];
			}
			
			//enable/disable lights
			//TODO: move this into a per key press event trigger
            if ((inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_NUMPAD1) || inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_1)) && TimeCheck(ref time1))
			{
				timeDelay=1;
				flipLightState(0);
			}
            if ((inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_NUMPAD2) || inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_2)) && TimeCheck(ref time2))
			{
				timeDelay=1;
				flipLightState(1);
			}
            if ((inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_NUMPAD3) || inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_3)) && TimeCheck(ref time3))
			{
				timeDelay=1;
				flipLightState(2);
			}
			
			
			//animate the lights
			for(int i=0;i<NUM_LIGHTS;++i)
				mLightPivots[i].Rotate(Vector3.UNIT_Z,new Degree(mLightSpeeds[i]*ev.timeSinceLastFrame));
			return true;
		}
		
		//work arround for not being able to use events to tell key presses
		//slows down mesh/material/light changes to acceptable levels
		//without this on a fast card the meshes/materials just flicker 'till you get your finger
		//of the key, no control on what you end up seing
		private bool TimeCheck(ref DateTime last)
		{
			if((DateTime.Now-last).Ticks>KEY_TIME)
			{
				last=DateTime.Now;
				return true;
			}
			else
				return false;
		}
		
		public override void CreateScene()
		{
			//Check to see if vertex and fragment programs are supported
			if(!Root.Singleton.RenderSystem.Capabilities.HasCapability(Capabilities.RSC_VERTEX_PROGRAM))
				MessageBox.Show("Your graphics card does not support vertex shaders and cannot run this demo.\nDot3Bump.CreateScene");
			else if(!Root.Singleton.RenderSystem.Capabilities.HasCapability(Capabilities.RSC_FRAGMENT_PROGRAM)||
			        !Root.Singleton.RenderSystem.Capabilities.HasCapability(Capabilities.RSC_DOT3))
				MessageBox.Show("Your graphics card does not support dot3 blending or fragment programs and cannot run this demo.\nDot3Bump.CreatScene");
			else
			{
				//set ambient light
				sceneMgr.AmbientLight=new ColourValue(0f,0f,0f);
				
				mMainNode=sceneMgr.RootSceneNode.CreateChildSceneNode();
				
				//Load the meshes with nond default HBU options
				for(int mn=0;mn<NUM_ENTITIES;mn++)
				{
					MeshPtr pMesh=MeshManager.Singleton.Load(mEntityMeshes[mn],
					                                         ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
					                                         HardwareBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY,
					                                         HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY,
					                                         true,
					                                         true);//can still read it
					ushort src, dest;
					if(!pMesh.SuggestTangentVectorBuildParams(VertexElementSemantic.VES_TANGENT, out src,out dest))
						pMesh.BuildTangentVectors(VertexElementSemantic.VES_TANGENT, src,dest);
					
					//create entity
					mEntities[mn]=sceneMgr.CreateEntity("Ent"+mn.ToString(),mEntityMeshes[mn]);
					//attach to main node
					mMainNode.AttachObject(mEntities[mn]);
					//make invisible except for index 0
					if(mn==0)
						mEntities[mn].SetMaterialName(mMaterialNames[mCurrentEntity][mCurrentMaterial]);
					else
						mEntities[mn].Visible=false;
				}
				for(int i=0;i<NUM_LIGHTS;i++)
				{
					mLightPivots[i]=sceneMgr.RootSceneNode.CreateChildSceneNode();
					mLightPivots[i].Rotate(mLightRotationAxes[i],mLightRotationAngles[i]);
					//create a light, use default parameters
					mLights[i]=sceneMgr.CreateLight("Light"+i.ToString());
					mLights[i].Position=mLightPositions[i];
					mLights[i].DiffuseColour=mDiffuseLightColours[i];
					mLights[i].SpecularColour=mSpecularLightColours[i];
					mLights[i].Visible=mLightState[i];
					//attach light to pivot
					mLightPivots[i].AttachObject(mLights[i]);
					//create billboard for the light
					mLightFlareSets[i]=sceneMgr.CreateBillboardSet("Flare"+i.ToString());
					mLightFlareSets[i].MaterialName="Examples/Flare";
					mLightPivots[i].AttachObject(mLightFlareSets[i]);
					mLightFlares[i]=mLightFlareSets[i].CreateBillboard(mLightPositions[i]);
					mLightFlares[i].Colour=mDiffuseLightColours[i];
					mLightFlareSets[i].Visible=true;
				}
				//move the camera to the right and make it look at the mesh
				camera.MoveRelative(new Vector3(50f,0f,20f));
				camera.LookAt(0f,0f,0f);
				//show overlay
				Overlay Over=OverlayManager.Singleton.GetByName("Example/DP3Overlay");
				mObjectInfo=OverlayManager.Singleton.GetOverlayElement("Example/DP3/ObjectInfo");
				mMaterialInfo=OverlayManager.Singleton.GetOverlayElement("Example/DP3/MaterialInfo");
				mInfo=OverlayManager.Singleton.GetOverlayElement("Example/DP3/Info");
				
				mObjectInfo.Caption="Current"+mEntityMeshes[mCurrentEntity];
				mMaterialInfo.Caption="Current"+mMaterialNames[mCurrentEntity][mCurrentMaterial];
				if(!Root.Singleton.RenderSystem.Capabilities.HasCapability(Capabilities.RSC_FRAGMENT_PROGRAM))
					mInfo.Caption="NOTE: Light colours and specular highlights are not supported by your card";
				Over.Show();
				
				//register key event monitor

			}
			
		}
	}
}
