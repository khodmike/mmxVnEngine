using System;
using System.IO;
namespace MmxVnEngine
{
	public class TextFileParser
	{  private string WorkFileName;
		private GlobalObject go;
		private Command[] cmds;
		private string CommentString;
		private bool isCommandPrefix;
		private string CommandPrefix="";
		private Command DefaultCommand;
		private char ArgumentSeparotor=' ';
		private char CommandArgumentSeparator= ' ';
		public TextFileParser (string FileName, GlobalObject glo, Command[] pcmds, string comment, string pcommandprefix, 
		                       Command DefCom=null, char pArgumentSeparator= ' ', char pCommandArgumentSeparator=' ' )
		{
			WorkFileName =glo.CreatePath(FileName);
			go = glo;
			cmds = pcmds;
			CommentString = comment;
			if (pcommandprefix != "")
			{ isCommandPrefix=true;
				CommandPrefix= pcommandprefix;  
			}
			else
				isCommandPrefix=false;
			DefaultCommand=DefCom;
			ArgumentSeparotor= pCommandArgumentSeparator;
			CommandArgumentSeparator= pCommandArgumentSeparator;  


		}

		public bool Parse ()
		{
			if (! File.Exists (WorkFileName))
			{
				go.msg.Message (MessageEnum.FileNotFound, WorkFileName);  
				return false; 
			}

			FileStream fs = new FileStream (WorkFileName, FileMode.Open, FileAccess.Read);
			StreamReader rd = new StreamReader (fs);
			string str = "", str_orig="", command_name;
			Command  cmd;
			while ((str=rd.ReadLine())  != null)
			{
				str_orig=str;

				if(str=="" || str.StartsWith(CommentString))
					continue;
			


                  cmd=null;


				//ПОИСК КОММАНДЫ
				if( isCommandPrefix && ! str.Trim ().StartsWith( CommandPrefix))
				{  
					cmd=DefaultCommand; 
					command_name="";
						
				}
			    else
				{

				str= str.Trim ();
                str= str.Remove(0, CommandPrefix.Length);  
			    command_name= str.Split(CommandArgumentSeparator)[0]; 
				

					foreach( Command cm in cmds)
						if( cm.isName(command_name))
							{cmd=cm;
							 break;}

				

					

			    if(cmd==null)
				{ 
				go.msg.Message(MessageEnum.UnkhowCommand, WorkFileName , command_name, str_orig );  
				return false;
				}

				}


				//АРГУМЕНТЫ


				//получение строки аргументов
             str=str.Remove (0, command_name.Length);

				//спец обработка
				if( cmd is CommandSP)  
				{ 	
					if( ! ((CommandSP) cmd).Parse(str,rd)  )
					{  go.msg.Message(MessageEnum.ErrorArgs, WorkFileName, command_name  );  return false;}
					continue; 
				}

				//0
				if(cmd.GetArgCount()==0)
				{ cmd.Parse (null);
					continue; 

				}
			    

				string [] args= new string [cmd.GetArgCount()];
				//1
				if( cmd.GetArgCount() ==  1)
				{
					args[0]=str;
					
				}
				else // > 1
				{
				System.Collections .Generic .List< string> tokens= new System.Collections.Generic.List<string>();
					foreach( string st in str.Split(ArgumentSeparotor))
						if( st.Trim ()!= "")
							tokens.Add (st);

					if( tokens.Count !=  cmd.GetArgCount())
					{   go.msg.Message(MessageEnum.ErrorArgsCount, WorkFileName, str_orig );  
						return false;
					}
				

					args=tokens.ToArray() ;
						

				}
				
				if( ! cmd.Parse (args))
				{  go.msg.Message(MessageEnum.ErrorArgs, WorkFileName, str_orig );  
					return false;

				}

				 



					



			}


			return true;

		}





	}
}

