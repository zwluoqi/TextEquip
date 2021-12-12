// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:XorInt.cs
// * Created:2019/3/21
// * Author:  lucy.yijian
// * Alert:
// * 代码千万行
// * 注释第一行
// * 命名不规范
// * 同事两行泪
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Xor
{

	public class XorInt2
	{
		List<int> vals ;
		RandomUtil randomUtil;
		public int val{
			get{
				var ret = this.vals [1] ^ this.vals [2];
				if (ret != this.vals [0]) {

				}
				var _tmps = new List<int> ();
				_tmps.Add (vals[0]);
				_tmps.Add (randomUtil.NextInt ());
				_tmps.Add (_tmps[0] ^ _tmps [1]);
				this.vals = _tmps;
				return ret;
			}
		}

		public XorInt2(int _val,RandomUtil _randomUtil){
			this.randomUtil = _randomUtil;
			this.vals = new List<int>();

			this.vals.Add (_val);
			this.vals.Add (randomUtil.NextInt ());
			this.vals.Add (this.vals [0] ^ this.vals [1]);
		}
	}

	public class XorInt3
	{
//		int _sourceVal;

		int _codeVal;
		int _code;


//		int _sumVal;
//		int _addVal;
		public int val{
			get{
				var ret = _codeVal ^ _code;
//				if (ret != _sourceVal) {
//					var s =new StackTrace() ; 
//					Grow.GrowNet.SendYaMieDie ("modify dict data:"+s.ToString());
//					return 0;
//				}
//				if (this._sumVal != ret + this._addVal) {
//					var s =new StackTrace() ; 
//					Grow.GrowNet.SendYaMieDie ("多重加密 dict data:"+s.ToString());
//					return 0;
//				}
				return ret;
			}
		}

		public XorInt3(int val,RandomUtil randomUtil){
//			this._sourceVal = val;
//
//			this._addVal = randomUtil.NextInt();
//			this._sumVal = this._sourceVal + this._addVal;
//
			this._code = randomUtil.NextInt();
			this._codeVal = this._code ^ val;
		}
	}

	public class XorInt
    {
		int _sourceVal;

		int _codeVal;
		int _code;


		int _sumVal;
		int _addVal;
		public int val{
			get{
				var ret = _codeVal ^ _code;
				if (ret != _sourceVal) {
					var s =new StackTrace() ;
					throw new Exception("modify dict data");
					// Grow.GrowNet.SendYaMieDie ("modify dict data:"+s.ToString());
					return 0;
				}
				if (this._sumVal != ret + this._addVal) {
					var s =new StackTrace() ;
					throw new Exception("多重加密 dict data");
					// Grow.GrowNet.SendYaMieDie ("多重加密 dict data:"+s.ToString());
					return 0;
				}
				return ret;
			}
		}

		public XorInt(int val,RandomUtil randomUtil){
			this._sourceVal = val;

			this._addVal = randomUtil.NextInt();
			this._sumVal = this._sourceVal + this._addVal;

			this._code = randomUtil.NextInt();
			this._codeVal = this._code ^ val;
		}
    }

	public class XorLong
	{
		long _sourceVal;
		long _codeVal;
		long _code;


		long _sumVal;
		long _addVal;

		public long val{
			get{
				var ret = _codeVal ^ _code;
				if (ret != _sourceVal) {
					var s =new StackTrace() ; 
					// Grow.GrowNet.SendYaMieDie ("modify dict data:"+s.ToString());
					throw new Exception("modify dict data");

					return 0;
				}
				if (this._sumVal != ret + this._addVal) {
					var s =new StackTrace() ; 
					// Grow.GrowNet.SendYaMieDie ("多重加密 dict data:"+s.ToString());
					throw new Exception("多重加密 dict data");

					return 0;
				}
				return ret;
			}
		}

		public XorLong(long val,RandomUtil randomUtil){
			this._sourceVal = val;

			this._addVal = randomUtil.NextInt();
			this._sumVal = this._sourceVal + this._addVal;

			this._code = randomUtil.NextLong();
			this._codeVal = this._code ^ val;
		}
	}

	public class XorDouble{
		byte[] _codeVal;

		public double val{
			get{
				return BitConverter.ToDouble (_codeVal, 0);
			}
		}

		public XorDouble(double val,RandomUtil randomUtil){
			this._codeVal = BitConverter.GetBytes(val);
		}
	}

	public class XorFloat{
		byte[] _codeVal;

		public double val{
			get{
				return BitConverter.ToSingle (_codeVal, 0);
			}
		}
		public XorFloat(float val,RandomUtil randomUtil){
			this._codeVal = BitConverter.GetBytes(val);
		}
	}
}

