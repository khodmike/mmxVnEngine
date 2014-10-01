using System;
using System.Collections.Generic; 
using System.IO;
namespace MmxVnEngine
{
	public class ChooseCommand:CommandSP
	{
		

		public  override byte GetArgCount ()
		{
			return 1;
		}
		protected  override string[] GetNames ()
		{
			return new string [] {"CHOOSE"};
		}

		public  override string GetID ()
		{
			return "CH";
		}
		public ChooseCommand( GlobalObject glo)
		{ 
			go=glo;

		}
	


		public  override  bool Parse (string str, TextReader rd)
		{   List<Opt> opts= new List<Opt>();
			string token; 
			string title, scene, logic;
			int sceneIndex;
			LogicParser lp;
			while ((token=ParseGetToken(ref str, rd)) !=null)
			{ 
				if (token.ToUpper () != "OPT")
					return false;

				title = str;
				if (title.Trim () == "")
					return false;  
				str = "";

				token = ParseGetToken (ref str, rd);

				if (token == null || token.ToUpper () != "SCENE")
					return false;
				scene = str;
				if (scene.Trim () == "")
					return false;  
				str = "";
				sceneIndex = ((DirectoryWorker)(go.read)).RegisterScene (scene);     
					
				token = ParseGetToken (ref str, rd);
		
				if (token == null)
				{
					opts.Add (new Opt (title, sceneIndex, null));
					break;
				}  
				if (token.ToUpper () == "OPT")
				{
					opts.Add (new Opt (title, sceneIndex, null));
					str = "opt " + str;
					continue;
				}
           
				if (token.ToUpper () != "LOGIC")
					return false;
				logic = str;
	
				lp = new LogicParser (go.vars);

				if (logic == null || logic.Trim () == "")
					return false;  
				if( ! lp.Parse (logic))
				{ go.msg.Message(MessageEnum.ErrorLogicParser, logic);
				return false;

				}
				str = "";
				opts.Add (new Opt (title, sceneIndex, lp));

			}


			if (opts.Count == 0)
				return false;
			else
			{  new ChooseEx(go,opts); 
				return true;
			}
		}
	
		private string ParseGetToken(  ref string  str, TextReader rd)
		{ do
			{ str=str.Trim ();
				if( str =="" || str.StartsWith(go.p.SceneCommentary))
				continue;
			int i= str.IndexOf(go.p.SceneParameterSeparator);
            if(i==-1)
					return str;
			string os=str;  
				str= str.Substring(i+1);
		   return os.Substring(0,i);  
		  }
			while((str=rd.ReadLine())  != null);

			return null;

		}


	}


	public  class Opt
		{ public Opt( string pTitle, int pSceneIndex, LogicParser pIsShown=null)
			{ Title= pTitle;
				SceneIndex= pSceneIndex;
			isShown= pIsShown;  
			}

			private string Title;
			private int SceneIndex;
		private LogicParser isShown; 

		   public string GetTitle ()
		{ return Title; 
		}

		public int GetSceneIndex()
		{
			return SceneIndex; 
		}

		public bool CanShow()
		{ if( isShown==null)
			return true;
			else
				return isShown.Execute();  

		}

		}


}


 

