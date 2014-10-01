using System;
using System.Collections.Generic; 

namespace MmxVnEngine
{
	public class VarDispetcher
	{ private List< VarDespetcherLogicElem> listLogic;
      private List< VarDespetcherNumElem> listNum;  
		public VarDispetcher ()
		{ listLogic= new List<VarDespetcherLogicElem>();
			listNum= new List<VarDespetcherNumElem>(); 
		}


		private int FindNum( string name)
		{ name=name.ToUpper(); 
			for( int i=0; i< listNum.Count; i++) 
			if( listNum[i].name== name)
				return i;
			return -1;

		}

		private int FindLog( string name)
		{  name=name.ToUpper(); 
			for( int i=0; i< listLogic.Count; i++) 
			if( listLogic[i].name== name)
				return i;
			return -1;

		}

		private class VarDespetcherLogicElem
		{ public string name;
		   public bool value;

			public VarDespetcherLogicElem( string VarName, bool val)
			{ name=VarName;
				value=val;

			}

		}

		private class VarDespetcherNumElem
		{ public string name;
		   public int value;
			public VarDespetcherNumElem( string VarName, int val)
			{ name=VarName;
				value=val;

			}

		}


		public int GetNumVar( string VarName)
		{ int i=FindNum ( VarName);
			if( i!= -1)
				return listNum[i].value;
			else
			return 0;
		}

		public bool GetLogVar (string VarName)
		{ 
			int i=FindLog ( VarName);
			if( i!= -1)
				return listLogic[i].value;
			else
			return false;
		}

		public bool isExistLogic (string VarName)
		{  return FindLog( VarName) != -1; 

		}

		public bool isExistNum (string VarName)
		{  return FindNum( VarName) != -1; 

		}

		public void SetLogVar (string VarName, bool Value)
		{ int index= FindLog( VarName);
			if( index==-1)
				listLogic.Add( new VarDespetcherLogicElem(VarName.ToUpper() , Value));
			else
				listLogic[index].value=Value; 

		}

		public void SetNumVar (string VarName, int Value)
		{ int index= FindNum( VarName);
			if( index==-1)
				listNum.Add( new VarDespetcherNumElem(VarName.ToUpper() , Value));
			else
				listNum[index].value=Value; 

		}


	}
}

