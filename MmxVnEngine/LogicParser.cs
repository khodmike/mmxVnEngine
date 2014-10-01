using System;
using System.Collections.Generic;
namespace MmxVnEngine
{
	public class LogicParser
	{ private LogicOperathion op;
		private LogicParser left, right;
		private int rightNum;
		private bool rightLog; 
		private string leftVarName;
		private bool LeftIsLogic;
		VarDispetcher d;
		public LogicParser ( VarDispetcher dis)
		{ LeftIsLogic=false;
			d=dis;
		}
		public bool Parse (string str)
		{

		
			List<DimigPair> lst;
			if( ! ScanDimig( ref str,  out lst))
				return false;
				
			if( lst.Count !=0  )
				 return ParseDimig (str,  lst);
			if( str.IndexOf("AND")!=-1 || str.IndexOf("OR")!=-1 || str.IndexOf("&&")!=-1  || str.IndexOf("||")!=-1 )
				return ParseAndOr( str);


			string [] tokens_raw=str.Split(' ');
			System.Collections.Generic.List<string> tokens= new System.Collections.Generic.List<string>();
			foreach( string t in tokens_raw)
				if( t.Trim()!="")
					tokens.Add(t);
		
			leftVarName=tokens[0].Trim();
			if( d.isExistLogic(leftVarName))
			{LeftIsLogic=true;
			 if( tokens.Count==1)
				{ op= LogicOperathion.VarOp; return true; } 
			}
			else if ( d.isExistNum(leftVarName)==false)
				return false;
			if( tokens.Count != 3)
				return false;
			string  optoken=tokens[1], righttok=tokens[2];

			if(LeftIsLogic &&  ! ParseBool(righttok) )
				return false;
			 if(!LeftIsLogic &&   ! Int32.TryParse(righttok.Trim (), out rightNum))
				return false;
			return ParseOp( optoken);
		}


		private bool  ParseDimig (string str, List< DimigPair> lst)
		{ //Индекс последнего токена  перед самой правой открывающей скобкой
			 
			int index = str.Substring (0, lst [lst.Count - 1].open).Trim ().LastIndexOf (" ") + 1;
			int k=-1;;
			if (index == 0) {
				index = lst [0].close + 1;
				string str2= str.Substring (lst [0].close+1);
				int a= str2.Length;
				str2= str2.Trim ();
				int b= str2.Length; 
				k=str2.IndexOf (" ") + lst[0].close+  (a-b)+1;
					

			}
			else
				k =lst[lst.Count-1].open;
			left= new LogicParser(d);

			bool r1= left.Parse( str.Substring (0, index));
			right= new LogicParser(d);

			bool r2= right.Parse ( str.Substring(k));

			return r1&&r2&&ParseOp(str.Substring (index, k-index).Trim ());
		}


		private class DimigPair
		{  public int open;
			public int close=-1;
		};

		private bool  ScanDimig (ref string effStr, out List<DimigPair> lst)
		{
			int level = 0;
			int cn = -1;
			lst = new List<DimigPair> ();
			effStr = effStr.Trim ().ToUpper();
			for (int i=0; i < effStr.Length; i++) {
				if (effStr [i] == '(') {
					level++;
					if (level == 1) {
						cn++;
						lst.Add (new DimigPair ());
						lst [cn].open = i; 

					}   

				}
				if (effStr [i] == ')') {
					level--;
					if (level == 0) { 
						lst [cn].close = i; 

					}   

				}

			}

	
			if (level != 0)
				return false;
			if (cn > -1 && lst [0].open == 0 && lst [0].close == effStr.Length-1) {
				effStr= effStr.Substring(1, effStr.Length-2).Trim (); 
				lst.RemoveAt(0); 
				ScanDimig(ref effStr, out lst);
			}
			return true;
           

		}

		private bool ParseAndOr ( string str)
		{ 
			int li1, li2,li3,li4;
			li1= str.LastIndexOf("&&");
			li2=str.LastIndexOf("||");
			li3= str.LastIndexOf ("AND");
			li4= str.LastIndexOf("OR");
			int trueLastIndex= Math.Max ( li1, Math.Max (li2, Math.Max (li3,li4)));
			left= new LogicParser(d); 
			 bool r1= left.Parse (str.Substring (0, trueLastIndex-1));
			string str2= str.Substring(trueLastIndex);
			int k;
			if( str2.StartsWith("AND"))
				k=3;
			else
				k=2;
			string op=str.Substring(trueLastIndex,k);
			right= new LogicParser(d);
			bool r2= right.Parse(str.Substring(trueLastIndex+k)); 
			return r1&&r2&&ParseOp(op) ;

		}

		private bool ParseOp (string str)
		{   
			string WorkStr = str.Trim () ;
			switch (WorkStr) {
			case"&&": { op= LogicOperathion.And ; return true ;}
			case"||": { op= LogicOperathion.Or ; return true ;}
			case "AND":{ op= LogicOperathion.And ; return true ;} 
			case "OR":{ op= LogicOperathion.Or ; return true ;}
			case "GT": { op= LogicOperathion.Gt; return !LeftIsLogic ;}
            case ">": { op= LogicOperathion.Gt; return ! LeftIsLogic ;}
            case "SM": { op= LogicOperathion.Sm ; return ! LeftIsLogic ;}
            case "<": { op= LogicOperathion.Sm; return ! LeftIsLogic ;}
            case "GTEQ": { op= LogicOperathion.GtEq; return ! LeftIsLogic ;}
            case ">=": { op= LogicOperathion.GtEq; return ! LeftIsLogic ;}
            case "=>": { op= LogicOperathion.GtEq; return ! LeftIsLogic ;}
            case "SMEQ": { op= LogicOperathion.SmEq; return !LeftIsLogic ;}
			case "<=": { op= LogicOperathion.SmEq; return ! LeftIsLogic ;}
			case "=<": { op= LogicOperathion.SmEq; return !LeftIsLogic ;}
			case "EQ": { op= LogicOperathion.Eq; return true;}
			case "==": { op= LogicOperathion.Eq; return true;}
			case "=": { op= LogicOperathion.Eq; return true;}
			case "NOTEQ": { op= LogicOperathion.NotEq; return true;}
			case "NEQ": { op= LogicOperathion.NotEq; return true;}
			case "!=": { op= LogicOperathion.NotEq; return true;}
			case "=!": { op= LogicOperathion.NotEq; return true;}
            case "<>": { op= LogicOperathion.NotEq; return true;}
			default: return false;

			}

		}

		private bool ParseBool (string str)
		{
			switch (str.Trim ()) {
			case "TRUE": {rightLog=true; return true;}
			case "T": {rightLog=true; return true;}
			case "1": {rightLog=true; return true;}
            case "FALSE": {rightLog=false; return true;}
			case "F": {rightLog=false; return true;}
			case "0": {rightLog=false; return true;}
			default: return false;
			}

		}

		public bool Execute ()
		{  
			switch (op) {
			case LogicOperathion.And :return left.Execute() && right.Execute();
			case LogicOperathion.Or : return left.Execute () || right.Execute ();
			case LogicOperathion.Eq:  return ExecuteEq(); 
			case LogicOperathion.NotEq:  return ExecuteNotEq(); 
			case LogicOperathion.Gt:  return d.GetNumVar( leftVarName) > rightNum;  
            case LogicOperathion.GtEq :  return d.GetNumVar( leftVarName) >= rightNum; 
            case LogicOperathion.Sm:  return d.GetNumVar( leftVarName) < rightNum; 
			case LogicOperathion.SmEq :  return d.GetNumVar( leftVarName) <= rightNum;   
			case LogicOperathion.VarOp: return d.GetLogVar(leftVarName);  
			default: return false; 
			}

		}
		private enum LogicOperathion 
	{ Gt, GtEq, Sm, SmEq, Eq, NotEq, VarOp, And, Or
	}

		private bool ExecuteEq ()
		{ if( LeftIsLogic)
			return d.GetLogVar( leftVarName) == rightLog;
			else
				return d.GetNumVar (leftVarName)== rightNum;
         
		}

		private bool ExecuteNotEq ()
		{ if( LeftIsLogic)
			return d.GetLogVar( leftVarName) != rightLog;
			else
				return d.GetNumVar (leftVarName)!= rightNum;
         
		}


	}


}

