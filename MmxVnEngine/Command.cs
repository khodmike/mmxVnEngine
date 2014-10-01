using System;
using System.IO;
using System.Drawing;  

namespace MmxVnEngine
{
	 abstract public class Command
	{ 

		protected GlobalObject go;
		public  abstract byte GetArgCount ();
		public  abstract string GetID ();
		public bool isName( string foundStr)
		{  string effSt= foundStr.Trim ().ToUpper(); 
			foreach( string str in GetNames ())
				if( str==effSt ) 
					return true;
			return false;
		}
		public abstract bool Parse( string[] str);

		protected abstract string[] GetNames();
	}


	abstract public class CommandSP: Command
	{ 



		public abstract bool Parse(string str, TextReader rd);
		public override bool Parse (string[] str)
		{
			return false;
		}

	}


	 public class CommandShow: Command
	{ 




		public  override byte GetArgCount ()
		{
			return 1;
		}

		public  override string GetID ()
		{
			return "SHOW";
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"SHOW", "SH"};
		}

		public CommandShow (GlobalObject glo)
		{
			go=glo;
		}


		public  override  bool Parse(string[] str)
		{
		 new CommandShowEx(go, str[0]);
			return true;
		}


	}


	 public class CommandBackground: Command
	{ 

	


		public  override byte GetArgCount ()
		{
			return 1;
		}

		public  override string GetID ()
		{
			return "BG";
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"BG", "CHBG"};
		}

		public CommandBackground ( GlobalObject glo)
		{ 
			go=glo;
		}


		public  override  bool Parse (string[] str)
		{ DirectoryWorker wr= ( DirectoryWorker) go.read; 
			int imgIndex = wr.RegisterImage (str [0]); 
			if (imgIndex != -1)
			{
				new CommandBackgroundEx (go, imgIndex);
				return true;
			}
			else
				return false;
		}

	}


	 public class CommandShowPersona: Command
	{ 

	

		protected  override string[] GetNames ()
		{
			return new string [] {"PERS", "SHPERS", "SHOW"};
		}

		public  override byte GetArgCount ()
		{
			return 2;
		}

		public  override string GetID ()
		{
			return "SHPRS";
		}
		public CommandShowPersona( GlobalObject glo)
		{ 
			go=glo;
		}






		public  override  bool Parse (string[] str)
		{    byte number=0;
			DirectoryWorker wr= ( DirectoryWorker) go.read; 
			int imgIndex = wr.RegisterImage (str [0]); 
			if (imgIndex != -1 && Byte.TryParse (str [1], out number) && number < go.p.NumOfParts)
			{
				new CommandShowPersonaEx (go, imgIndex,number );
				return true;
			}
			else
				return false;

		}



	}

	 public class CommandHidePersona : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 1;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"HIDEPERS", "HIDE", "HDPERS"};
		}

		public  override string GetID ()
		{
			return "HDPRS";
		}

		public CommandHidePersona ( GlobalObject glo)
		{ 
			go=glo;
		}

 
		public  override  bool Parse (string[] str)
		{ byte number=0;
			if (Byte.TryParse (str [0], out number) && number < go.p.NumOfParts)
			{
				new CommandHidePersonaEx (go, number);
				return true;
			}
			else
				return false;
		}


	}


	 public class CommandHideAll : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 0;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"HIDEALL"};
		}

		public  override string GetID ()
		{
			return "HDALL";
		}
		public CommandHideAll( GlobalObject glo)
		{ 
			go=glo;
		}
	
		public  override  bool Parse(string[] str)
		{ 
			new CommandHideAllEx(go);
			return true; 
		}


	}


	 public class CommandScene : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 1;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"SCENE", "NEXTSCENE"};
		}

		public  override string GetID ()
		{
			return "SCHENE";
		}
		public CommandScene( GlobalObject glo)
		{ 
			go=glo;
		}
	

	
		public  override  bool Parse(string[] str)
		{ int	SceneIndex = ((DirectoryWorker ) (go.read)).RegisterScene (str[0]); 
		 	new CommandSceneEx(go, SceneIndex );
			return true;
		}


	}


	 public class CommandSetVar : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 3;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"VAR"};
		}

		public  override string GetID ()
		{
			return "SETVAR";
		}
		public CommandSetVar( GlobalObject glo)
		{ 
			go=glo;
		}
	



		public  override  bool Parse (string[] str)
		{

			string type, value, Name;
		    bool isLogic;
            int nValue=0;
            bool lValue=false;
			type = str [0].Trim ().ToUpper ();
			Name = str [1];
			value = str [2].Trim ().ToUpper ();
			if (type != "LOGIC" && type != "NUM")
			{
				go.msg.Message (MessageEnum.ErrorVarType, type);
				return false;

			}
			if (type == "LOGIC")
				isLogic = true;
			else
				isLogic = false;
			if (isLogic)
			{
				if (value != "T" && value != "TRUE" && value != "1" && value != "F" && value != "FALSE" && value != "0")
				{
					go.msg.Message (MessageEnum.ErrorLogicVarValue, value);  
					return false;

				}
				if (value == "T" || value == "TRUE" || value == "1")
					lValue = true;
				else
					lValue = false; 


			} else
			{ if( ! Int32.TryParse(value,  out nValue))
				{ go.msg.Message(MessageEnum.ErrorNumVarValue, value);
					return false;

				}

			}

			//Регистрация переменой для контроля разбора лог. выражений
			if (isLogic)
			{
				go.vars.SetLogVar (Name, lValue);  
			} else
			{
				go.vars.SetNumVar(Name, nValue);  
			}

			 new CommandSetVarEx(go, Name, isLogic, nValue, lValue);
			return true;


		}


	}


	public class CommandChangeVar : Command
	{ 

	
		public  override byte GetArgCount ()
		{
			return 2;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"CHVAR"};
		}

		public  override string GetID ()
		{
			return "CHVAR";
		}
		public CommandChangeVar( GlobalObject glo)
		{ 
			go=glo;
		}
	



		public  override  bool Parse (string[] str)
		{

			string  value;
		    string Name;
		    bool isLogic;
		    int nValue=0;
		    bool lValue=false;

			Name = str [0];
			value = str [1].Trim ().ToUpper ();
			if (go.vars.isExistLogic (Name)) 
				isLogic = true;
			else
				if (go.vars.isExistNum (Name)) 
				isLogic = false;
				else
				{ go.msg.Message(MessageEnum.UndefinaidVareble, Name) ; 
				  return false;
				}

			if (isLogic)
			{
				if (value != "T" && value != "TRUE" && value != "1" && value != "F" && value != "FALSE" && value != "0")
				{
					go.msg.Message (MessageEnum.ErrorLogicVarValue, value);  
					return false;

				}
				if (value == "T" || value == "TRUE" || value == "1")
					lValue = true;
				else
					lValue = false; 


			} else
			{ if( ! Int32.TryParse(value, out  nValue))
				{ go.msg.Message(MessageEnum.ErrorNumVarValue, value);
					return false;

				}

			}

		  new CommandChangeVarEx(go, Name, isLogic, nValue, lValue);
			return true;


		}


	}


	public class CommandINC : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 2;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"INC"};
		}

		public  override string GetID ()
		{
			return "INC";
		}
		public CommandINC( GlobalObject glo)
		{ 
			go=glo;
		}
	


		public  override  bool Parse (string[] str)
		{

			string  value;
		   string Name;
	       int Value;

			Name = str [0];
			value = str [1].Trim ().ToUpper ();
			if ( ! go.vars.isExistNum (Name)) 
				{ go.msg.Message(MessageEnum.UndefinaidVareble, Name) ; 
				  return false;
			    }

			 if( ! Int32.TryParse(value,  out Value))
				{ go.msg.Message(MessageEnum.ErrorNumVarValue, value);
					return false;

				}



		new CommandINCEx(go, Name, Value);

			return true;


		}


	}


	public class CommandDEC : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 2;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"DEC"};
		}

		public  override string GetID ()
		{
			return "DEC";
		}
		public CommandDEC( GlobalObject glo)
		{ 
			go=glo;
		}
	


		public  override  bool Parse (string[] str)
		{

			string  value;
		    string Name;
             int Value;

			Name = str [0];
			value = str [1].Trim ().ToUpper ();
			if ( ! go.vars.isExistNum (Name)) 
				{ go.msg.Message(MessageEnum.UndefinaidVareble, Name) ; 
				  return false;
			    }

			 if( ! Int32.TryParse(value,  out Value))
				{ go.msg.Message(MessageEnum.ErrorNumVarValue, value);
					return false;

				}



	       new CommandDECEx(go, Name, Value);
			return true;


		}


	}



	public class CommandMusic : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 1;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"MUSIC", "STARTMUSIC"};
		}

		public  override string GetID ()
		{
			return "STARTMUSIC";
		}
		public CommandMusic( GlobalObject glo)
		{ 
			go=glo;
		}
	


		public  override  bool Parse (string[] str)
		{ int num= ((DirectoryWorker) go.read).RegisterSound(str[0]);
			new CommandMusicEx(go,num);
			return true;


		}


	}


	public class CommandMusicStop : Command
	{ 


		public  override byte GetArgCount ()
		{
			return 0;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"STOPMUSIC"};
		}

		public  override string GetID ()
		{
			return "STOPMUSIC";
		}
		public CommandMusicStop( GlobalObject glo)
		{ 
			go=glo;
		}
	


		public  override  bool Parse (string[] str)
		{ 
			new CommandMusicStopEx(go);
			return true;


		}


	}
}



