using System.Collections.Generic;
using UnityEngine;

namespace Game.View
{
	public class InputInteration
	{
		public bool a = false;
		public bool d = false;
		public List<int> opq = new List<int>();


		public KeyCode x;
		public KeyCode y;
		
		public InputInteration(KeyCode x, KeyCode y)
		{
			this.x = x;
			this.y = y;
		}
		
		
		private KeyCode CheckEndRotation()
		{
			if (opq.Count > 0)
			{
				if (opq[opq.Count - 1] == (int) x)
				{
					// display.pushMessage.D2LRotationBegin(true);
					return x;
				}
				else if (opq[opq.Count - 1] == (int) y)
				{
					// display.pushMessage.D2LRotationBegin(false);
					return y;
				}
				else
				{
					Debug.LogWarning("CheckEndRotation op None");
					return KeyCode.None;
				}
			}
			else
			{
				// display.pushMessage.D2LRotationEnd();
				return KeyCode.None;
			}
		}

		KeyCode CheckKeying()
		{
			bool valid = true;
			if (!Input.GetKey(x) && a)
			{

				RemoveA();
				valid = false;
			}

			if (!Input.GetKey(y) && d)
			{

				RemoveD();
				valid = false;
			}

			if (Input.GetKey(x) && !a)
			{

				AddA();
				valid = false;
			}

			if (Input.GetKey(y) && !d)
			{

				AddD();
				valid = false;
			}

			return CheckEndRotation();
		}

		void AddA()
		{

			opq.Remove((int) x);
			opq.Add((int) x);
			a = true;
		}

		void RemoveA()
		{
			opq.Remove((int) x);
			a = false;
		}

		void AddD()
		{

			opq.Remove((int) y);
			opq.Add((int) y);

			d = true;
		}

		void RemoveD()
		{
			opq.Remove((int) y);
			d = false;
		}

		public KeyCode GetCurCode(out bool operation)
		{
			operation
				= false;
			if (Input.GetKeyDown(x))
			{
				AddA();
				operation = true;
			}

			if (Input.GetKeyUp(x))
			{
				RemoveA();
				operation = true;
			}

			if (Input.GetKeyDown(y))
			{
				AddD();
				operation = true;
			}

			if (Input.GetKeyUp(y))
			{
				RemoveD();
				operation = true;
			}

			if (operation)
			{
				var code = CheckEndRotation();
//				Debug.LogWarning(Time.frameCount+" frameCount op code:" + code);
				return code;
			}
			else
			{
				var code = CheckKeying();
				return code;
			}
		}
	}
}