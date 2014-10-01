using System;
using System.IO;
using System.Collections.Generic; 
namespace MmxVnEngine
{
	public class Scene
	{  private  List< CommandEX> Commands; 
		public Scene ()
		{ Commands= new List<CommandEX>();
		}

		public void  WriteToBinary( BinaryWriter  wr)
		{

		}


		public bool ReadFromBinary( BinaryReader rd)
		{ 
			return true;
		}


		public void  WriteToText( TextWriter   wr)
		{

		}

		public void AddCommand( CommandEX cmd)
		{
			Commands.Add( cmd);
		}


		public void Execute()
		{
			 if( Commands.Count == 0)
				return;
			bool cont;
			do
			{  cont= Commands[0].Execute();
				Commands.RemoveAt(0); 

			}
			while(cont);
		}

		public void ClearOnTransfer()
		
		{//Уничтогжаем все команды кроме текущей выполняющейся( команды перехода)
			if( Commands.Count >= 2)
			Commands.RemoveRange(1, Commands.Count-1); 

		}










	}
}

