using System;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
namespace MmxVnEngine
{
	 public interface IReadMmxVnData
	{  Image GetImage (int number);
	   Scene GetScene (int number);
		SoundFile GetSound( int number);

	}
	public class DataFileWorker : IReadMmxVnData 
	{   private FileStream fs;
	  private BinaryFormatter bin;
		BinaryWriter wr;
		BinaryReader rd;
		private List<long> ImageOffsets;
		private List<long> SceneOffsets;
		private List<long> SoundOffsets;
		private bool wrMode;
		GlobalObject go;
		public DataFileWorker ( GlobalObject glo)
		{
			go=glo;
			bin = new BinaryFormatter ();


		}

		public bool Open (string FileName, bool isToSave)
		{ ImageOffsets= new List<long>();
			SceneOffsets = new List<long>();
			SoundOffsets= new List<long> (); 
			FileMode mode = FileMode.Open;
			FileAccess acs = FileAccess.Read;
			wrMode = isToSave;
			if (isToSave) {
				mode = FileMode.Create; 
				acs = FileAccess.Write;
			}
			try {
				fs = new FileStream (FileName, mode, acs);
			} catch (FileNotFoundException e) {
				go.msg.Message(MessageEnum.FileNotFound, FileName);  
				return false;

			} catch (Exception e) {
				go.msg.Message(MessageEnum.FileLoadError, FileName);  
				return false;
			}


			if (wrMode == false) {
				LoadCatolog();
				rd = new BinaryReader (fs);
			}
			else
				wr= new BinaryWriter(fs);
			 
			return true;
		}

		public void Close()
		{ if( wrMode)
			SaveCatolog();
			fs.Close ();
			if( wr != null)
			wr.Close ();
			if( rd != null)
			rd.Close ();

		}

		private void SaveCatolog ()
		{ long StartCatologPos= fs.Position; 
			wr.Write(ImageOffsets.Count);
			foreach( long off in ImageOffsets)
				wr.Write(off);
			wr.Write( SceneOffsets.Count);   
			foreach( long off in SceneOffsets)
				wr.Write(off);
			wr.Write( SoundOffsets.Count);   
			foreach( long off in SoundOffsets)
				wr.Write(off);
			wr.Write(StartCatologPos);  
		}

		private void LoadCatolog ()
		{ fs.Seek (-sizeof(long), SeekOrigin.End);
			long StartCatologPos= rd.ReadInt64();
			fs.Seek( StartCatologPos, SeekOrigin.Begin);  
			int imageCount=rd.ReadInt32 (); 
			for( int i=0; i< imageCount; i++)
				ImageOffsets.Add ( rd.ReadInt64());
            int sceneCount=rd.ReadInt32 (); 
			for( int i=0; i< sceneCount; i++)
				SceneOffsets.Add ( rd.ReadInt64());
			int soundCount=rd.ReadInt32 (); 
			for( int i=0; i< soundCount; i++)
				SoundOffsets.Add ( rd.ReadInt64());
				
		}
		public Image GetImage (int number)
		{   
			if (number<0 || number>= ImageOffsets.Count ) {
				go.msg.Message(MessageEnum.BinaryFileFormatError ,"");
				System.Windows.Forms.Application.Exit();
				return null;
			}
			fs.Seek(ImageOffsets [number],  SeekOrigin.Begin);
			return (Image) bin.Deserialize(fs);


		}
		public Scene GetScene (int number)
		{ 	

			if (number < 0 || number >= SceneOffsets.Count)
			{
				go.msg.Message (MessageEnum.BinaryFileFormatError, ""); 
				System.Windows.Forms.Application.Exit(); 
				return null;
			}
			fs.Seek (SceneOffsets [number], SeekOrigin.Begin);
			Scene res = new Scene ();
			if (! res.ReadFromBinary (rd))
			{
				go.msg.Message (MessageEnum.BinaryFileFormatError, ""); 
				System.Windows.Forms.Application.Exit(); 
				return null;
			}

			return res;

		}

		public void WriteImage (string path)
		{   
			if( !File.Exists( go.CreatePath(path))) 
			   return;

			ImageOffsets.Add(fs.Position);
			DirectoryWorker wrk;
			bin.Serialize(fs, wrk.LoadImage(path));

		}

		public void WriteImage (Image img)
		{ 	ImageOffsets.Add(fs.Position);
			bin.Serialize(fs, img);

		}

		public void WriteScene (string name, Scene scene)
		{ 
			SceneOffsets.Add (fs.Position);  
			scene.WriteToBinary(wr); 

		}



		 public SoundFile GetSound (int number)
		{ if (number<0 || number>= SoundOffsets.Count ) {
				go.msg.Message(MessageEnum.BinaryFileFormatError ,"");
				System.Windows.Forms.Application.Exit();
				return null;
			}
			fs.Seek(SoundOffsets [number],  SeekOrigin.Begin);
			SoundFile res= new SoundFile();
			res.FromBinaryFile(rd);
			return  res;

		}


		public void WriteSound( string name, string path)
		{ byte[] bar= File.ReadAllBytes(path);
			SoundOffsets.Add(fs.Position); 
			wr.Write (bar.Length);
			wr.Write (bar);

		}





	}
}

