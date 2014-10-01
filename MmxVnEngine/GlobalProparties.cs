using System;
using System.Drawing; 

namespace MmxVnEngine
{
	public class GlobalProparties
	{ public string Title;
      // цвет кисти границы
		public	Color penColor= Color.Blue;
			// Базисный цвет фона вывода текста
		 public Color textBgColor= Color.White;
			//Начала вывода текста по Y
		public	float textYKoef= (float)80/100;
		public 	int textSize=12;
		public 	Color textColor=Color.Black;
		public 	string textFamily="Arial";
		public 	int penSize=2;
		public  byte NumOfParts=5;


		//РАЗБОР СЦЕН
		public string SceneCommentary= "//";
		public string SceneCommandPrefex=".";
		public char SceneParameterSeparator=' ';
		public char SceneCommandParameterSeparator=' '; 



		//КНОПКИ ВЫБОРА
		public float chooseStartX=(float) 40/100;
		public float chooseWitdh=(float) 20/100;
		public float chooseStartY=(float) 40/100;
		public float chooseIntervalY=(float) 3/100;
		public float chooseHeight=(float) 3/100;
		public string chooseFontFamily=" Arial";
		public int chooseFontSize=12;
		public bool chooseIsBold=false;

	}
}

