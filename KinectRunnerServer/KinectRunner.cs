using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

using xn;
using xnv;
namespace KinectRunnerServer
{
	public class KinectRunner
	{
		public KinectRunner ()
		{
			Init();
			Setup();
		}
		
		void Init(){
			
			Setup();
		}
		
		public void Run(){
			while(true){
				try{
					this.Context.WaitAndUpdateAll();
					this.SeshManager.Update(Context);
				}catch(SystemException){
				}
			}
		}
		
		/// <summary>
		/// Setup builds an XN Context, Session Manager and all the detectors. 
		/// It also adds the callbacks for the SessionManager and adds the listeners on the Broadcaster. 
		/// </summary>
		private void Setup(){
			//build the context
			Context = new Context(CONFIG);
			//build session manager
			SeshManager = new SessionManager(Context,"RaiseHand","RaiseHand");
			SeshManager.SetQuickRefocusTimeout(15000);
			
			//build the detectors
			Pushy = new PushDetector();
			Swipy = new SwipeDetector();
			//setup all the callbacks
			SetupCallbacks();
			SeshManager.SessionStart += SessionStarted;
			//add the flow router to the session
			SeshManager.AddListener(Pushy);
			SeshManager.AddListener(Swipy);
			
		}
		
		void SessionStarted (ref Point3D point)
		{
			Console.WriteLine("Session Started");
		}

		/// <summary>
		/// Add the callbacks 
		/// </summary>
		
		private void SetupCallbacks(){
			Pushy.Push += new PushDetector.PushHandler(OnPush);
			//swipe detectors
			Swipy.SwipeUp += new SwipeDetector.SwipeUpHandler(SwipeUp);
			Swipy.SwipeRight += new SwipeDetector.SwipeRightHandler(SwipeRight);
			Swipy.SwipeDown += new SwipeDetector.SwipeDownHandler(SwipeDown);
			Swipy.SwipeLeft += new SwipeDetector.SwipeLeftHandler(SwipeLeft);
		}
		
		

		
		/// <summary>
		/// The following are all Methods that detectors will call. THESE are not finalized versions simply prototypes
		/// </summary>
		
		//push detectors
		private void OnPush(float velocity, float angle){
			byte[] msg = Encoding.ASCII.GetBytes(KinectMessages[0]);
			publisher.Send(msg,KinectMessages[0].Length);
			Console.WriteLine("push sent");
		}
		
		//swipe detectors clockwise (up, right,down,left)
		private void SwipeUp(float left, float right){
			byte[] msg = Encoding.ASCII.GetBytes(KinectMessages[1]);
			publisher.Send(msg,KinectMessages[1].Length);
			Console.WriteLine("up sent");
		}
		
		private void SwipeRight(float left, float right){
			byte[] msg = Encoding.ASCII.GetBytes(KinectMessages[4]);
			publisher.Send(msg,KinectMessages[4].Length);
			Console.WriteLine("right sent");
		}
		
		private void SwipeDown(float left, float right){
			byte[] msg = Encoding.ASCII.GetBytes(KinectMessages[2]);
			publisher.Send(msg,KinectMessages[2].Length);
			Console.WriteLine("down sent");
		}
		
		private void SwipeLeft(float left, float right){
			byte[] msg = Encoding.ASCII.GetBytes(KinectMessages[3]);
			publisher.Send(msg,KinectMessages[3].Length);
			Console.WriteLine("left sent");
		}
		
		
		
		
		//initializations
		private readonly string CONFIG = @"../../config.xml";
		//a nice list of readonly strings
		private readonly string[] KinectMessages={"push","up","down","left","right"};
		UdpClient publisher = new UdpClient("localhost",8899);
		
		Context Context;
		
		//Managers
		SessionManager SeshManager;
		//Detectors
		PushDetector Pushy;
		SwipeDetector Swipy;
		
	}
}
