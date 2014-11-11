using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class GameObjectHelper
	{
		public GameObjectHelper ()
		{
		}
		
		public static string getPlayerName()
		{
			return "Player";
		}
		
		public static GameObject getPlayer()
		{
			return GameObject.Find(getPlayerName());
		}
		
		public static Camera getCamera()
		{
			return Camera.main;
		}

        public static Warehouse getWarehouse()
        {
            GameObject obj = GameObject.Find("Warehouse");
            if (obj == null)
            {
                Debug.LogError("Can't find gameobject \"Warehouse\"");
                return null;
            }

            return obj.GetComponent<Warehouse>();
        }
	}
}

