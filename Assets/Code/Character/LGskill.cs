using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LGskill {
	// Status values required in order to be able to use this skill
	public int[] StatusReq = new int[(int)LGstatData.StatusType.COUNT];
	private uint _skillID;
	// Effect of the Actual Skill, whether it heals/damages a status
	// or applies a temporary buff
	private LGskillData.EffectType effect;
	// What the skill targets, and which team it impacts
	private LGskillData.TargetType target;
	private LGskillData.TeamType team;
	// Effectiveness of the skill, use determined by effect type
	private float potency;

	// Constructor
	public LGskill() {
		
	}
}
