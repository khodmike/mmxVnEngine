using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace MmxVnEngine
{ 
	public class DirectoryWorker : IReadMmxVnData
	{  GlobalObject go;
		List<string> ImagePaths;
		List<string> ScenePaths;
		List<string> SoundPaths;

		public DirectoryWorker ( GlobalObject glo)
		{ go=glo;
			ImagePaths= new List<string>();
			ScenePaths= new List<string>();
			SoundPaths= new List<string>();
		}

		  public Image GetImage (int number)
		{
			Image img = null;
			string name= ImagePaths[number]; 
			try {
				img = Image.FromFile (go.CreatePath (name), false);
			} catch (FileNotFoundException e) {
				go.msg.Message(MessageEnum.FileNotFound, name);  
				System.Windows.Forms.Application.Exit ();  
				return null;

			} catch(Exception e){
				go.msg.Message(MessageEnum.FileLoadError , name,  e.Message );  
				System.Windows.Forms.Application.Exit ();
				return null;
			}
			return img;
		}

		 public Image LoadImage (string path)
		{
			Image img = null;
			try {
				img = Image.FromFile (go.CreatePath (path), false);
			} catch (FileNotFoundException e) {
				go.msg.Message(MessageEnum.FileNotFound, path);  
				return null;

			} catch(Exception e){
				go.msg.Message(MessageEnum.FileLoadError , path,  e.Message );  
				return null;
			}
			return img;
		}
	    public Scene GetScene (int number)
		{
			Command [] cmds = new Command[] {
				new CommandShow (go),
				new CommandBackground (go),
				new CommandShowPersona (go),
				new CommandHidePersona (go),
				new CommandHideAll (go),
				new CommandScene (go),
				new ChooseCommand( go),
				new CommandSetVar(go),
				new CommandChangeVar(go),
				new CommandINC(go),
				new CommandDEC(go) ,
				new CommandMusic(go),
				new CommandMusicStop(go) 
			};       
			TextFileParser txtp = new TextFileParser (ScenePaths [number], go, cmds, 
			                                          go.p.SceneCommentary , go.p.SceneCommandPrefex ,
			                                          new CommandShow (go),
			                                          go.p.SceneParameterSeparator, go.p.SceneCommandParameterSeparator  ); 
			 
			if (!  txtp.Parse ())
			{
				System.Windows.Forms.Application.Exit ();
				return null;
			}
			return go.curScene; 
		}

		public int RegisterImage (string path)
		{ int i=ImagePaths.IndexOf(path);
			if(i != -1) 
			return i;
			if( LoadImage(path)==null)
			return -1; 
			ImagePaths.Add(path);
			return ImagePaths.Count-1; 

		}


		public int RegisterScene (string path)
		{ int i=ScenePaths.IndexOf(path);
			if(i != -1) 
			return i;
			ScenePaths.Add(path);
			return ScenePaths.Count-1; 


		}

		public SoundFile GetSound (int number)
		{
			string name = go.CreatePath (SoundPaths [number]); 
		
			if (! File.Exists (name))
			{
				go.msg.Message (MessageEnum.FileNotFound, name);  
				System.Windows.Forms.Application.Exit ();  
				return null;
			}
			;

			if (new FileInfo (name).Extension.ToLower () != ".wav")
			{ go.msg.Message (MessageEnum.WrongSoundFormat , name);  
				System.Windows.Forms.Application.Exit ();  
				return null;
			};
			SoundFile res=new SoundFile();
			res.FromMusicFile(name);
			return res; 




		}


		public int RegisterSound (string path)
		{ int i=SoundPaths.IndexOf(path);
			if(i != -1) 
			return i;
			SoundPaths.Add(path);
			return SoundPaths.Count-1; 


		}
	}
}

