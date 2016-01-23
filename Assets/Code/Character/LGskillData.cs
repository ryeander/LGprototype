using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Parent Class for all Skills, holds values/functions
// that are consistent across all skills.
// TODO: Possibly break this down into subclasses
// TODO: JSON built Skill system
public static class LGskillData {
	// TODO: This class will hold all logic for Effects/Targeting for Skills
	public enum EffectType {
		STATUS_MOD,
		TEMP_BUFF,
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
		ENEMY,
		ALL,
		COUNT
	}
	public static Dictionary<uint,LGskill> Skills;

}
