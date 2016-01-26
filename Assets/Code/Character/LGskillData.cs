using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Parent Class for all Skills, holds values/functions
// that are consistent across all skills.
// TODO: Possibly break this down into subclasses
// TODO: JSON built Skill system
public static class LGskillData {

	// Struct for initializing skills
	public struct SkillStruct {
		public int effortCost;
		public int skillID;
		public string skillName;
		public int effect;
		public int target;
		public int team;
		public int sourceTrait;
		public int targetTrait;
		// Constructor
		public SkillStruct(int skillID, string n, int effortCost, int effect, int sourceT,
			int targetT, int target, int team) {
			this.effortCost = effortCost;
			this.skillID = skillID;
			this.skillName = n;
			this.effect = effect;
			this.target = target;
			this.team = team;
			this.sourceTrait = sourceT;
			this.targetTrait = targetT;
		}
	}
	#region Type
	// TODO: This class will hold all logic for Effects/Targeting for Skills
	public enum EffectType {
		// STATUS MOD EFFECTS
		HEAL, // Raise Health
		WOUND, //Lower Health
		ENERGIZE, // Raise Energy
		EXHAUST, // Lower Energy
		INFUSE, // Raise Toxicity
		OXIDIZE, // Lower Toxicity
		DEMORALIZE, // Raise Morale
		CHEER, // Lower Morale
		// TEMP BUFF EFFECTS
		SADISM, // Raise Prowess
		PACIFISM, // Lower Prowess
		VIGOR, // Raise Endurance
		WEAKNESS, // Lower Endurance
		RESISTANCE, // Raise Tolerance
		CRAVING, // Lower Tolerance
		BRAVERY, // Raise Charisma
		COWARDICE, // Lower Charisma
		COUNT
	}
	public enum TargetType {
		SINGLE,
		SINGLE_RANDOM,
		SINGLE_WEAK,
		SINGLE_STRONG,
		DUO_RANDOM,
		ALL,
		COUNT
	}
	public enum TeamType {
		FRIEND,
		FOE,
		ALL,
		COUNT
	}
	#endregion
	// TODO: JSON Object for loading this in
	public static Dictionary<uint,LGskill> Skills;

	// For potential in game editor/Testing
	public static void SaveSkillsToJSON()
	{
		List<JSONObject> allSkills = new List<JSONObject> ();
		foreach (uint ID in Skills.Keys) {
			LGskill toSave = Skills [ID];
			SkillStruct theD = toSave.ToData ();
			Debug.Log (string.Format ("Saving Skill : {0} ID : {1}", theD.skillName, theD.skillID));
			JSONObject newJSON = new JSONObject(JSONObject.Type.OBJECT);
			newJSON.AddField ("ID", theD.skillID);
			newJSON.AddField ("Name", theD.skillName);
			newJSON.AddField ("EffortCost", theD.effortCost);
			newJSON.AddField ("Effect", theD.effect);
			newJSON.AddField ("Target", theD.target);
			newJSON.AddField ("Team", theD.team);
			newJSON.AddField ("TTrait", theD.targetTrait);
			newJSON.AddField ("STrait", theD.sourceTrait);
			allSkills.Add (newJSON);
		}
		JSONObject saveArray = new JSONObject(allSkills.ToArray());
		string toWrite = saveArray.ToString(true);
		Debug.Log (toWrite);
		using (StreamWriter sw = new StreamWriter ("Skills.txt")) {
			sw.WriteLine (toWrite);
		}
	}

	public static void LoadSkillsFromJSON()
	{
		Skills = new Dictionary<uint, LGskill> ();
		string encodedJSONString = "";
		string line = "";
		using (StreamReader sr = new StreamReader ("Skills.txt")) {
			while ((line = sr.ReadLine ()) != null) {
				encodedJSONString += line;
			}
		}
		JSONObject toLoad = new JSONObject (encodedJSONString);
		if (toLoad.IsArray) {
			foreach (JSONObject skillJSON in toLoad.list) {
				
				int sID = 0;
				if (skillJSON.GetField (ref sID, "ID")) {
					Debug.Log (string.Format ("ID : {0}", sID));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no ID Field");
				}

				string n = "";
				if (skillJSON.GetField (ref n, "Name")) {
					Debug.Log (string.Format ("Name : {0}", n));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no name Field");
				}

				int eCost = 0;
				if (skillJSON.GetField (ref eCost, "EffortCost")) {
					Debug.Log (string.Format ("EffortCost : {0}", eCost));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no EffortCost Field");
				}

				int effect = 0;
				if (skillJSON.GetField (ref effect, "Effect")) {
					Debug.Log (string.Format ("Effect : {0}", (EffectType)effect));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no Effect Field");
				}

				int target = 0;
				if (skillJSON.GetField (ref target, "Target")) {
					Debug.Log (string.Format ("Target : {0}", (TargetType)target));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no Target Field");
				}

				int team = 0;
				if (skillJSON.GetField (ref team, "Team")) {
					Debug.Log (string.Format ("Target : {0}", (TeamType)team));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no Team Field");
				}

				int sourceT = 0;
				if (skillJSON.GetField (ref sourceT, "STrait")) {
					Debug.Log (string.Format ("STrait : {0}", (LGstatData.TraitType)sourceT));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no SourceTrait Field");
				}

				int targetT = 0;
				if (skillJSON.GetField (ref targetT, "TTrait")) {
					Debug.Log (string.Format ("TTrait : {0}", (LGstatData.TraitType)targetT));
				} else {
					Debug.LogError ("INVALID SKILL JSON, no TargetTrait Field");
				}

				new LGskill ((uint)sID, n, eCost, (EffectType)effect, (LGstatData.TraitType)sourceT, 
					(LGstatData.TraitType)targetT, (TargetType)target, (TeamType)team);
			}
		} else {
			Debug.LogError ("INVALID JSON OBJECT, Not a List");
		}

		foreach (LGskill s in Skills.Values) {
			Debug.Log(s.ToData ().skillName + " loaded");
		}
	}
}
