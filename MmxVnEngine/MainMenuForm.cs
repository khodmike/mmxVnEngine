using System;
using System.Windows.Forms; 
using System.Drawing; 

namespace MmxVnEngine
{
	public class MainMenuForm: Form 
	{  GlobalObject go;
		Button btnResume;
		static bool initStatus=true;
		public MainMenuForm ()
		{





			initStatus= GlobalInit(); 
			  
			int maxX = SystemInformation.VirtualScreen.Width;
			int maxY = SystemInformation.VirtualScreen.Height;
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;
			this.Text=" Mmx Vn Engine";

			Button btn = new Button ();
			btn.Text = "Выход";
			btn.Location = new Point (maxX / 2 - 100, 90 * maxY / 100); 
			btn.Width = 200;
			btn.Height = 30;
			btn.Click += ClickExit;
			this.Controls.Add (btn);


		    btn = new Button ();
			btn.Text = "Новая Игра";
			btn.Location = new Point (maxX / 2 - 100, 40 * maxY / 100); 
			btn.Width = 200;
			btn.Height = 30;
			btn.Click += ClickNewGame;
			this.Controls.Add (btn);


			btn = new Button ();
			btn.Text = "Возобновить";
			btn.Location = new Point (maxX / 2 - 100, 30 * maxY / 100); 
			btn.Width = 200;
			btn.Height = 30;
			btn.Click += ClickResume;
			btn.Visible=false; 
			this.Controls.Add (btn);
			btnResume=btn; 



		}

		static public void  Main()
		{  


			MainMenuForm frm= new MainMenuForm();
			if( ! initStatus)
				return;


			Application.Run(frm);
	

		}

			    private bool  GlobalInit ()
		{   go= new GlobalObject();
			go.read= new DirectoryWorker(go);  
		    go.frm= new WorkForm(go);
			return  true;
			

		}

		private void ClickExit( object s, EventArgs arg)
		{
			Application.Exit();
		}

		private void ClickNewGame( object s, EventArgs arg)
		{   
		//УСТОНАВЛИВАЕМ ТЕКУЩУЮ СЦЕНУ НА ПЕРВУЮ	
			go.curScene=new Scene();
			((DirectoryWorker ) (go.read)).RegisterScene ("scene.txt"); 
			go.read.GetScene(0); 


			btnResume.Visible=true; 
			go.frm.ClearScene(); 
			go.frm.Show ();


		}

		private void ClickResume( object s, EventArgs arg)
		{   
		
			go.frm.Show ();

		}
	}
}

