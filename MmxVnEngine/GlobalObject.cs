using System;

namespace MmxVnEngine
{
	public class GlobalObject
	{
		 public WorkForm frm;
		 public IReadMmxVnData read;
		public GlobalProparties p;
		 public MmxMessanger msg;
		public Scene curScene;
		public VarDispetcher vars;
		public SoundFile music;

		public GlobalObject()
		{ p= new GlobalProparties();
		  msg= new MmxMessanger();
			msg.SetTitle("MmxVnEngine"); 
			curScene= new Scene(); 
			vars= new VarDispetcher();





		}
		public  string CreatePath (string filename)
		{
			filename = filename.Trim ();
			if (filename [0] == '/' || filename [0] == '\\')
			{
				filename=filename.Remove(0,1); 

			};
			 filename=filename.Replace('\\', '/');
			filename=filename.Replace ('/', System.IO.Path.DirectorySeparatorChar); 

			return System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, filename);

		}




	}
}

