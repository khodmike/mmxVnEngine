using System;
using System.Windows.Forms;

namespace MmxVnEngine
{
	public class MmxMessanger
	{  const int msg_string_count=20;
		string[]  msgs;



	  public MmxMessanger ()
		{ msgs= new string[msg_string_count];
			DefaultInit();

		}

		private  void DefaultInit()
		{


	
	 msgs[0]="Неверный формат файдла данных.";
	 msgs[1]= "Фаил ";
	  msgs[2]= " не найден.";
 	  msgs[3]= "При загрузке файла ";
	  msgs[4]= "произошла неопределяемая ошибка.";
	  msgs[5]= "При сохранении в файл  ";
	   msgs[6]= "произошла неопределяемая ошибка.";
	   msgs[7]="Ошибка при разборе файла ";
	   msgs[8]= "Нероспознаная комманда ";
	   msgs[9]=" в строке  ";
       msgs[10]= "Неверное число аргументов в комманде  ";
       msgs[11]= "Неверные аргументы в комманде ";
	   msgs[12]= "Ошибка синтаксиса  логического выражения ";
	   msgs[13]= " Неверный тип переменой ";
	   msgs[14]= " Может быть только logic или num";
	   msgs[15]= "Недопустимое значение логической переменой ";
	   msgs[16]= "Недопустимое значение числовой переменой ";
	   msgs[17]= "Попытка работы с необьявленой переменой "; 
       msgs[18]= "Неверный формат  звуковово файла  ";  
       msgs[19]= " Допустимы только файлы .wav"; 
		}


		private string mTitle;

		public void SetTitle (string Title)
		{
			mTitle=Title;
		}

		public void  Message (MessageEnum m, string param, string param2="", string param3="")
		{ string msg="";
			switch (m) {
			case MessageEnum.BinaryFileFormatError:  msg=msgs[0]; break; 
            case MessageEnum.FileNotFound : msg=msgs[1]+param+msgs[2]; break; 
			case MessageEnum.FileLoadError : msg=msg=msgs[3]+param+msgs[4]+Environment.NewLine + param2; break; 
			case MessageEnum.FileSaveError : msg=msgs[5]+param+msgs[6]; break;
			case MessageEnum.UnkhowCommand: msg=msgs[7]+ param+Environment.NewLine+msgs[8]+param2+msgs[9]+param3; break; 
            case MessageEnum.ErrorArgsCount  : msg=msgs[7]+param+Environment.NewLine+msgs[10]+param2; break; 
			case MessageEnum.ErrorArgs : msg=msgs[7]+param+Environment.NewLine+msgs[11]+param2; break; 
			case MessageEnum.ErrorLogicParser: msg=msgs[12]+param; break;
			case MessageEnum.ErrorVarType: msg=msgs[13]+param+ Environment.NewLine+ msgs[14]; break;
			case MessageEnum.ErrorLogicVarValue: msg= msgs[15]+param; break;
            case MessageEnum.ErrorNumVarValue : msg= msgs[16]+param; break;
            case MessageEnum.UndefinaidVareble : msg= msgs[17]+param; break;
			case MessageEnum.WrongSoundFormat: msg=msgs[18]+param+ Environment.NewLine+ msgs[19]; break; 
			}
			MessageBox.Show( msg, mTitle, MessageBoxButtons.OK,MessageBoxIcon.Error);

		}

	}

	public enum MessageEnum
		{ BinaryFileFormatError,
          FileNotFound,
		  FileLoadError,
		  FileSaveError,
		  UnkhowCommand,
		 ErrorArgsCount,
		 ErrorArgs,
		 ErrorLogicParser,
		ErrorVarType,
		ErrorLogicVarValue,
		ErrorNumVarValue,
		UndefinaidVareble,
		WrongSoundFormat

		}
}

