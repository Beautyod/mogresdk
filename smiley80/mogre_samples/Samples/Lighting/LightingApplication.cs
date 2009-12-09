namespace Mogre.Demo.Lighting
{
	using System;
	using System.Collections.Generic;

	using Mogre;
	// Listener class for frame updates
	/** Application class */
	class LightingApplication : ExampleApplication.Example
	{
	    #region Fields
	
	    List<AnimationState> mAnimStateList = new List<AnimationState>();

	    #endregion Fields
	
	    #region Methods
	    
	    public override void CreateScene()
	    {
	        // Set a very low level of ambient lighting
	        sceneMgr.AmbientLight = new ColourValue(0.1f, 0.1f, 0.1f);
	
	        // Use the "Space" skybox
	        sceneMgr.SetSkyBox(true, "Examples/SpaceSkyBox");
	
	        // Load ogre head
	        Entity head = sceneMgr.CreateEntity("head", "ogrehead.mesh");
	
	        // Attach the head to the scene
	        sceneMgr.RootSceneNode.AttachObject(head);
	
	        // Note: removed Controller
	
	        SetupTrailLights();
	    }
	
	    protected override bool ExampleApp_FrameStarted(FrameEvent evt)
	    {
	        if(base.ExampleApp_FrameStarted(evt) == false )
	            return false;
	
	        foreach (AnimationState ani in this.mAnimStateList)
	        {
	            ani.AddTime(evt.timeSinceLastFrame);
	        }
	
	        return true;
	    }
	
	    void SetupTrailLights()
	    {
	        sceneMgr.AmbientLight = new ColourValue(0.5f, 0.5f, 0.5f);
	        Vector3 dir = new Vector3(-1, -1, 0.5f);
	        dir.Normalise();
	        Light l = sceneMgr.CreateLight("light1");
	        l.Type = Light.LightTypes.LT_DIRECTIONAL;
	        l.Direction = dir;
	
	        NameValuePairList pairList = new NameValuePairList();
	        pairList["numberOfChains"] = "2";
	        pairList["maxElements"] = "80";
	        RibbonTrail trail = (RibbonTrail)(
	            sceneMgr.CreateMovableObject("1", "RibbonTrail", pairList));
	        trail.MaterialName = "Examples/LightRibbonTrail";
	        trail.TrailLength = 400;
	
	        sceneMgr.RootSceneNode.CreateChildSceneNode().AttachObject(trail);
	
	        // Create 3 nodes for trail to follow
	        SceneNode animNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
	        animNode.Position = new Vector3(50,30,0);
	        Animation anim = sceneMgr.CreateAnimation("an1", 14);
	        anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);
	        NodeAnimationTrack track = anim.CreateNodeTrack(1, animNode);
	        TransformKeyFrame kf = track.CreateNodeKeyFrame(0);
	        kf.Translate = new Vector3(50,30,0);
	        kf = track.CreateNodeKeyFrame(2);
	        kf.Translate = new Vector3(100, -30, 0);
	        kf = track.CreateNodeKeyFrame(4);
	        kf.Translate = new Vector3(120, -100, 150);
	        kf = track.CreateNodeKeyFrame(6);
	        kf.Translate = new Vector3(30, -100, 50);
	        kf = track.CreateNodeKeyFrame(8);
	        kf.Translate = new Vector3(-50, 30, -50);
	        kf = track.CreateNodeKeyFrame(10);
	        kf.Translate = new Vector3(-150, -20, -100);
	        kf = track.CreateNodeKeyFrame(12);
	        kf.Translate = new Vector3(-50, -30, 0);
	        kf = track.CreateNodeKeyFrame(14);
	        kf.Translate = new Vector3(50,30,0);
	
	        AnimationState animState = sceneMgr.CreateAnimationState("an1");
	        animState.Enabled = true;
	        mAnimStateList.Add(animState);
	
	        trail.SetInitialColour(0, 1.0f, 0.8f, 0);
	        trail.SetColourChange(0, 0.5f, 0.5f, 0.5f, 0.5f);
	        trail.SetInitialWidth(0, 5);
	        trail.AddNode(animNode);
	
	        // Add light
	        Light l2 = sceneMgr.CreateLight("l2");
	        l2.DiffuseColour = (trail.GetInitialColour(0));
	        animNode.AttachObject(l2);
	
	        // Add billboard
	        BillboardSet bbs = sceneMgr.CreateBillboardSet("bb", 1);
	        bbs.CreateBillboard(Vector3.ZERO, trail.GetInitialColour(0));
	        bbs.MaterialName = "Examples/Flare";
	        animNode.AttachObject(bbs);
	
	        animNode = sceneMgr.RootSceneNode.CreateChildSceneNode();
	        animNode.Position = new Vector3(-50,100,0);
	        anim = sceneMgr.CreateAnimation("an2", 10);
	        anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);
	        track = anim.CreateNodeTrack(1, animNode);
	        kf = track.CreateNodeKeyFrame(0);
	        kf.Translate = new Vector3(-50,100,0);
	        kf = track.CreateNodeKeyFrame(2);
	        kf.Translate = new Vector3(-100, 150, -30);
	        kf = track.CreateNodeKeyFrame(4);
	        kf.Translate = new Vector3(-200, 0, 40);
	        kf = track.CreateNodeKeyFrame(6);
	        kf.Translate = new Vector3(0, -150, 70);
	        kf = track.CreateNodeKeyFrame(8);
	        kf.Translate = new Vector3(50, 0, 30);
	        kf = track.CreateNodeKeyFrame(10);
	        kf.Translate = new Vector3(-50,100,0);
	
	        animState = sceneMgr.CreateAnimationState("an2");
	        animState.Enabled = true;
	        mAnimStateList.Add(animState);
	
	        trail.SetInitialColour(1, 0.0f, 1.0f, 0.4f);
	        trail.SetColourChange(1, 0.5f, 0.5f, 0.5f, 0.5f);
	        trail.SetInitialWidth(1, 5);
	        trail.AddNode(animNode);
	
	        // Add light
	        l2 = sceneMgr.CreateLight("l3");
	        l2.DiffuseColour = trail.GetInitialColour(1);
	        animNode.AttachObject(l2);
	
	        // Add billboard
	        bbs = sceneMgr.CreateBillboardSet("bb2", 1);
	        bbs.CreateBillboard(Vector3.ZERO, trail.GetInitialColour(1));
	        bbs.MaterialName = "Examples/Flare";
	        animNode.AttachObject(bbs);
	    }
	
	    #endregion Methods
	}
}
