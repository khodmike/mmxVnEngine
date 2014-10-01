using System;
using System.Windows.Forms;
using System.Drawing; 
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace MmxVnEngine
{
	public class WorkForm: Form
	{ 

		Image [] Personas;
		Image BgImage=null;
		List<Button> ButtonChoise; 
		int maxX = SystemInformation.VirtualScreen.Width;
		int maxY = SystemInformation.VirtualScreen.Height;
	
		string TextToOut, NextTextToOut, controlText ;
		GlobalObject go;


		SoundFile sound;
		public WorkForm ( GlobalObject glo)
		{ go=glo;
			NextTextToOut="";



		
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;
			this.Click += PicClick;



			CreatePbPers(); 
			this.Paint+=PaintForm;  
		


		
		



		}




		public void PicClick (Object sender, EventArgs arg)
		{ 

			MouseEventArgs arg1 = (MouseEventArgs)arg;

			if (arg1.Button == MouseButtons.Left)
			{ if( NextTextToOut != "")
				{
					TextToOut=NextTextToOut;
					DivideTextY();
					this.Invalidate();

				}
				else
					go.curScene.Execute ();
			} else if (arg1.Button == MouseButtons.Right)
			{
				this.Hide(); 
			}

		  

		}


		public void ChooseClick (Object sender, EventArgs arg)
		{ 
			new CommandSceneEx(go, (int) ((Button) sender).Tag);
			EndChoose();
			go.curScene.Execute(); 
		  

		}

		private void EndChoose ()
		{ foreach( Button btn in ButtonChoise)
			this.Controls.Remove(btn);
			ButtonChoise.Clear();
			ButtonChoise=null;
			go.curScene.Execute(); 

		}

		private void CreatePbPers ()
		{   Personas= new  Image[ go.p.NumOfParts];
			for( byte i=0; i< go.p.NumOfParts;  i++)
				Personas[i]= null;

						

		}

		public void ShowPersonaPicture (int contnum, Image img)
		{  


				
				  Personas[contnum]=img;
					this.Invalidate();  
					

			   
			  





		}

		public void HidePersonaPicture( int contnum)
		{  
			        Personas[contnum]=null;
					this.Invalidate();  
		
		}


		public void HideAll()
		{
			for( byte i=0; i< go.p.NumOfParts; i++)
				Personas[i]=null;
					this.Invalidate();  
		
		}


 

		public void SetOutputText (string outText)
		{		
			outText=DivideTextX (outText);
			TextToOut+= Environment.NewLine + outText; 
			DivideTextY ();
			this.Invalidate(); 

		}

		private string DivideTextX (string text)
		{
			string other = "";
			Font f = new Font (go.p.textFamily, go.p.textSize, GraphicsUnit.Point);
			int ind;
			while (TextRenderer.MeasureText(text, f).Width > maxX)
			{
				ind = text.LastIndexOf (" ");
				other = text.Substring (ind) + other;
				text = text.Substring (0, ind);  

			}

			if (other != "")
			{ other=other.Substring(1, other.Length-1);  
				text= text+ Environment.NewLine+ DivideTextX(other);     
			}

			return text;

		}

		private  void DivideTextY ()
		{
			NextTextToOut = ""; 

			Font f = new Font (go.p.textFamily, go.p.textSize, GraphicsUnit.Point);
			int ind;
			int textFieldHeight = (int)(maxY * (0.95f - go.p.textYKoef));
			while (TextRenderer.MeasureText(TextToOut, f).Height > textFieldHeight)
			{
				ind = TextToOut.LastIndexOf (Environment.NewLine);
				NextTextToOut = TextToOut.Substring (ind) + NextTextToOut;
				TextToOut = TextToOut .Substring (0, ind);  

			}

			if (controlText == TextToOut)
			{
				TextToOut = NextTextToOut;
				DivideTextY ();
			}

			controlText=TextToOut;  



		}

		public void SetBg (Image img)
		{ BgImage= img;
		  this.Invalidate(); 

		}
		private  void PaintForm (Object sender, PaintEventArgs e)
		{ 


			int part= maxX/ go.p.NumOfParts;
			int textY=(int)  (maxY * go.p.textYKoef); 
			if( BgImage != null) 
				e.Graphics.DrawImage (BgImage, new Rectangle (0, 0, maxX, maxY)); 
			for (byte i=0; i< go.p.NumOfParts; i++)
			{ if( Personas[i] != null)
				 e.Graphics.DrawImage (Personas[i], new Rectangle (i* part  , 0, i+1* part, maxY)); 
					 


			}
			e.Graphics.DrawRectangle( new Pen(go.p.penColor,go.p.penSize), 0,  textY, maxX,maxY);  
			e.Graphics.FillRectangle ( new SolidBrush( Color.FromArgb(128,go.p.textBgColor.R,go.p.textBgColor.G,go.p.textBgColor.B)) , 0,  textY, maxX,maxY); 
			//e.Graphics.DrawString(TextToOut,new Font(go.p.textFamily, go.p.textSize,GraphicsUnit.Point), new SolidBrush(go.p.textColor),0,textY,StringFormat.GenericDefault);  
			TextRenderer.DrawText(e.Graphics,TextToOut,new Font(go.p.textFamily, go.p.textSize,GraphicsUnit.Point), new Point(0, textY), go.p.textColor); 


		}


		public void ClearScene ()
		{
			TextToOut = "";
			NextTextToOut="";
			BgImage = null;
			for( byte i=0; i< go.p.NumOfParts;  i++)
				Personas[i]= null;
			go.curScene.Execute(); 
	}


		public void DoChoose (List<Opt> opts)
		{
			ButtonChoise = new List<Button>(); 
			int i=0;
			foreach (Opt o in opts)
			{ 

				Button btn= new Button();
				btn.Tag=o.GetSceneIndex();  
				btn.Text=o.GetTitle();
				int Y= (int) (maxY*( go.p.chooseStartY+ i* (go.p.chooseIntervalY+ go.p.chooseHeight)));
				btn.Location= new Point((int)(maxX*go.p.chooseStartX)  , Y   );
				btn.Click+= ChooseClick;
				btn.Width= (int) ( maxX* go.p.chooseWitdh);
				btn.Height= (int) ( maxY* go.p.chooseHeight); 
				if( go.p.chooseIsBold)
					btn.Font= new System.Drawing.Font(go.p.chooseFontFamily,go.p.chooseFontSize, FontStyle.Bold );
				else
					btn.Font= new System.Drawing.Font(go.p.chooseFontFamily,go.p.chooseFontSize);
				ButtonChoise.Add(btn);	 
				this.Controls.Add (btn);
				i++;
					
			}

		}



}

}