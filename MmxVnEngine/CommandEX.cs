using System;
using System.Collections.Generic;

namespace MmxVnEngine
{
	 abstract public class CommandEX
	{ protected GlobalObject go;
		private const  byte  binary_number=0;
		public abstract bool Execute();

		public CommandEX ()
		{
		}
	}



 public class CommandShowEx: CommandEX
	{ 

		private const  byte  binary_number=1;


		




		public CommandShowEx ( GlobalObject glo, string pText)
		{ 
			go=glo;
			text=pText;
			go.curScene.AddCommand(this); 
		}

		private string text="";


		public  override  bool Execute ()
		{ go.frm.SetOutputText(text);
			return false;

		}
	}





	 public class CommandBackgroundEx: CommandEX
	{ 

		private const  byte  binary_number=2;





		public CommandBackgroundEx ( GlobalObject glo, int pImgIndex)
		{ 
			go=glo;
			imgIndex=pImgIndex;
			go.curScene.AddCommand(this); 

		}
		private int imgIndex=-1; 


		public  override  bool Execute ()
		{ go.frm.SetBg(go.read.GetImage(imgIndex)); 
			return true;

		}
	}


	 public class CommandShowPersonaEx: CommandEX
	{ 

		private const  byte  binary_number=3;



			public CommandShowPersonaEx( GlobalObject glo, int pImgIndex, byte pNumber)
		{ 
			go=glo;
			imgIndex=pImgIndex;
			number=pNumber;
			go.curScene.AddCommand(this); 
		}


		private int imgIndex;
		private byte number=0;
	


		public  override  bool Execute ()
		{ go.frm.ShowPersonaPicture(number, go.read.GetImage(imgIndex)  ); 
			return true;

		}
	}

	 public class CommandHidePersonaEx : CommandEX
	{ 

		private const  byte  binary_number=4;



		public CommandHidePersonaEx ( GlobalObject glo, byte pNumber)
		{ 
			go=glo;
			number= pNumber;
			go.curScene.AddCommand(this); 
		}
		private byte number=0;


		public  override  bool Execute ()
		{ go.frm.HidePersonaPicture(number); 
			return true;
		}
	}


	 public class CommandHideAllEx : CommandEX
	{ 

		private const  byte  binary_number=5;
	
		
		public CommandHideAllEx( GlobalObject glo)
		{ 
			go=glo;
			go.curScene.AddCommand(this); 
		}
	
	

		public  override  bool Execute ()
		{ go.frm.HideAll(); 
			return true;
		}
	}


	 public class CommandSceneEx : CommandEX
	{ 

		private const  byte  binary_number=6;
	
	
		public CommandSceneEx( GlobalObject glo, int pSceneIndex)
		{ 
			go=glo;
			SceneIndex=pSceneIndex;
			go.curScene.AddCommand(this); 
		}

		private  int SceneIndex;


		public  override  bool Execute ()
		{ go.curScene.ClearOnTransfer ();  
			go.read.GetScene(SceneIndex);   
			 
			return true;
		}
	}


	public class ChooseEx: CommandEX
	{ private const  byte  binary_number=7;
      public ChooseEx (GlobalObject glo, List<Opt> op)
		{ go=glo;
			glo.curScene.AddCommand(this); 
			opts=op;

		}
		private List<Opt> opts;
		public  override  bool Execute ()
		{ int i=0;
			while( i< opts.Count) 
				if (! opts[i].CanShow ())
					opts.RemoveAt(i); 
			else
				i++;

			if (opts.Count == 0)
				return false;
			if (opts.Count == 1)
			{
				new CommandSceneEx (go, opts [0].GetSceneIndex());
				return true;
			}
			go.frm.DoChoose(opts);  
			return false;
		}


	}

	 public class CommandSetVarEx : CommandEX
	{ 

		private const  byte  binary_number=8;
	
	
		public CommandSetVarEx( GlobalObject glo, string pName, bool pIsLogic, int pnValue, bool plValue)
		{ 
			go=glo;
			Name=pName;
			isLogic=pIsLogic;
			nValue= pnValue;
			lValue=plValue; 
			go.curScene.AddCommand(this); 
		}

		private string Name;
		private bool isLogic;
		private int nValue;
		private bool lValue;


		public  override  bool Execute ()
		{
			if (isLogic)
			{
				go.vars.SetLogVar (Name, lValue);  
			} else
			{
				go.vars.SetNumVar(Name, nValue);  
			}
			 
			return true;
		}
	}


	public class CommandChangeVarEx : CommandEX
	{ 

		private const  byte  binary_number=9;

		
		public CommandChangeVarEx( GlobalObject glo, string pName, bool pIsLogic, int pnValue, bool plValue)
		{ 
			go=glo;
			Name=pName;
			isLogic=pIsLogic;
			nValue= pnValue;
			lValue=plValue; 
			go.curScene.AddCommand(this); 
		}

		private string Name;
		private bool isLogic;
		private int nValue;
		private bool lValue;
	

		public  override  bool Execute ()
		{
			if (isLogic)
			{
				go.vars.SetLogVar (Name, lValue);  
			} else
			{
				go.vars.SetNumVar(Name, nValue);  
			}
			 
			return true;
		}
	}


	public class CommandINCEx : CommandEX
	{ 

		private const  byte  binary_number=10;

	
		public CommandINCEx( GlobalObject glo, string pName,  int pValue)
		{ 
			go=glo;
			Name=pName;
			Value= pValue;
			go.curScene.AddCommand(this); 

		}

		private string Name;
		private int Value;


		public  override  bool Execute ()
		{


				go.vars.SetNumVar(Name, go.vars.GetNumVar(Name) + Value);  

			 
			return true;
		}
	}


	public class CommandDECEx : CommandEX
	{ 

		private const  byte  binary_number=11;

	
		public CommandDECEx( GlobalObject glo, string pName,  int pValue)
		{ 
			go=glo;
			Name=pName;
			Value= pValue;
			go.curScene.AddCommand(this); 

		}

		private string Name;
		private int Value;


		public  override  bool Execute ()
		{


				go.vars.SetNumVar(Name, go.vars.GetNumVar(Name) - Value);  

			 
			return true;
		}
	}


	public class CommandMusicEx : CommandEX
	{ 

		private const  byte  binary_number=12;

	
		public CommandMusicEx( GlobalObject glo, int pNumber)
		{ 
			go=glo;
			Number=pNumber;
			go.curScene.AddCommand(this); 

		}

	
		private int Number;


		public  override  bool Execute ()
		{ if(go.music != null)
				go.music.Stop();

			SoundFile res= go.read.GetSound(Number);
			if( res == null)
				return false;
			go.music=res;
			res.Play ();
			return true;
		}
	}


	public class CommandMusicStopEx : CommandEX
	{ 

		private const  byte  binary_number=13;

	
		public CommandMusicStopEx( GlobalObject glo)
		{ 
			go=glo;
			go.curScene.AddCommand(this); 

		}

	



		public  override  bool Execute ()
		{  if( go.music != null)
				go.music.Stop();
			return true;
		}
	}

}