using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemsCrate : MonoBehaviour {

	public cls_Item[] AllPossibleItems;

	void OnTriggerEnter (Collider coll) {
		KartReferences temp = coll.GetComponentInParent<KartReferences>();
		if (temp != null) {
			if (temp.KartItem.HoldingItem == null) {
				/*Boost_Item1 Ttemp = temp.gameObject.AddComponent<Boost_Item1> ();
				Ttemp.BoostPercent = BoostPercent;
				Ttemp.InitAcceleration = InitAcceleration;
				Ttemp.LongTime = LongTime;
				temp.KartItem.HoldingItem = Ttemp;
				Destroy (gameObject);*/
				GiveRandomItem (temp);
			}
		}
	}

	void GiveRandomItem(KartReferences temp){
		int ItemID = UnityEngine.Random.Range (0, AllPossibleItems.Length);
		Type ttype = AllPossibleItems [ItemID].GetType ();
		/*print (ttype.AssemblyQualifiedName);
		cls_Item Ttemp = temp.gameObject.AddComponent<>(ttype.AssemblyQualifiedName);
		Ttemp.BoostPercent = AllPossibleItems [ItemID].BoostPercent;
		Ttemp.InitAcceleration = AllPossibleItems [ItemID].InitAcceleration;
		Ttemp.LongTime = AllPossibleItems [ItemID].LongTime;
		temp.KartItem.HoldingItem = Ttemp;*/
		cls_Item Ttemp = null;
		switch (ttype.ToString()) {
			case "Boost_Item1":
				Ttemp = temp.gameObject.AddComponent<Boost_Item1>();
				break;
			case "TNT_Item":
				Ttemp = temp.gameObject.AddComponent<TNT_Item>();
				break;
		}
		Ttemp.KartRef = temp;
		Ttemp.BoostPercent = AllPossibleItems [ItemID].BoostPercent;
		Ttemp.InitAcceleration = AllPossibleItems [ItemID].InitAcceleration;
		Ttemp.LongTime = AllPossibleItems [ItemID].LongTime;
		temp.KartItem.HoldingItem = Ttemp;
		Destroy (gameObject);
	}
}
