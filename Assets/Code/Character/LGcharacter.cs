using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Combat interactions, displaying the character
public class LGcharacter : ScriptableObject {
	// TODO: unique ID for saving character data
	private uint _charID;
	public uint ID {
		get {
			return _charID;
		}
	}

	private string _name;
	public string Name {
		get {
			return _name;
		}
	}    

	// Traits for the character, refers to permanent stat values that can be modified by buffs
	// Set and Add functions return true if the value is valid, and false otherwise.
	private int[] _TraitValues = new int[(int)LGstatData.TraitType.COUNT];
	public int GetTrait(LGstatData.TraitType type) {
		int index = (int)type;
		// Modify by Buffs here, return post buff value
		return _TraitValues[index];
	}
	public bool SetTrait(LGstatData.TraitType type, int val) {
		int index = (int)type;
		if ((val < 0)||(val > LGstatData.kMaxTrait)) {
			return false;
		}
		_TraitValues[index] = val;
		return true;
	}
	public bool AddTrait(LGstatData.TraitType type, int val) {
		int index = (int)type;
		_TraitValues[index] += val;
		if (_TraitValues [index] < 0) {
			_TraitValues [index] = 0;
			return false;
		} else if (_TraitValues [index] > LGstatData.kMaxTrait) {
			_TraitValues [index] = LGstatData.kMaxTrait;
			return false;
		}
		return true;
	}

	// Status values for the character, refers to current state that can be modified by skills
	private int[] _StatValues = new int[(int)LGstatData.TraitType.COUNT];
	public int GetStat(LGstatData.StatusType type) {
		int index = (int)type;
		return _StatValues[index];
	}
	public bool SetStat(LGstatData.StatusType type, int val) {
		int index = (int)type;
		if ((val < 0)||(val > LGstatData.kMaxStatus)) {
			return false;
		}
		_StatValues [index] = val;
		return true;
	}
	public bool AddStat(LGstatData.StatusType type, int val) {
		int index = (int)type;
		_StatValues [index] += val;
		if (_StatValues [index] < 0) {
			_StatValues [index] = 0;
			return false;
		} else if (_StatValues [index] > LGstatData.kMaxStatus) {
			_StatValues [index] = LGstatData.kMaxStatus;
			return false;
		}
		return true;
	}

	// List of Skill IDs, used to look up skills from data
	public LGskill _AttackSkill;
	public LGskill _UtilitySkill;
	public LGskill _ClassSkill;
	public LGskill _AugmentSkill;

	// Initialization Function for Character TODO: Saved Characters and other bullshit
	public LGcharacter() {
	}

	public override string ToString() {
		string output = string.Format("NAME : {0}\n",_name);
		for (int i = 0; i < (int)LGstatData.TraitType.COUNT; i++) {
			output+=(string.Format ("{0} : {1}\n", (LGstatData.TraitType)i, GetTrait((LGstatData.TraitType)i)));
		}
		for (int i = 0; i < (int)LGstatData.StatusType.COUNT; i++) {
			output+=(string.Format ("{0} : {1}\n", (LGstatData.StatusType)i, GetStat((LGstatData.StatusType)i)));
		}
		output += string.Format ("Attack Skill : {0}\n", _AttackSkill.ToString());
		output += string.Format ("Utility Skill : {0}\n", _UtilitySkill.ToString());
		output += string.Format ("Class Skill : {0}\n", _ClassSkill.ToString());
		output += string.Format ("Augment Skill : {0}\n", _AugmentSkill.ToString());

		return output;
	}

}
