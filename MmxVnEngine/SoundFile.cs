using System;
using System.Media;
using System.IO;

namespace MmxVnEngine
{
	public class SoundFile
	{ private SoundPlayer pl=null;
		private MemoryStream st;
		public SoundFile ()
		{ 
		}

		public void  FromMusicFile( string path)
		{ pl= new SoundPlayer(path);
          
		}

		public void Play()
		{ if( pl == null)
			return;
			pl.Play ();

		}

		public void Stop ()
		{ pl.Stop(); 
		}

		public void FromBinaryFile (BinaryReader rd)
		{ int length= rd.ReadInt32();
			st= new MemoryStream(rd.ReadBytes(length),false);  
			pl= new SoundPlayer(st);
		}



	}
}

