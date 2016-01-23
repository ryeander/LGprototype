using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LGskill {
	// Effort value for determining energy costs
	private int _effortCost;
	private uint _skillID;
	// Name of the skill
	private string _name;
	// Effect of the Actual Skill, whether it heals/damages a status
	// or applies a temporary buff
	private LGskillData.EffectType _effect;
	// What the skill targets, and which team it impacts
	private LGskillData.TargetType _target;
	private LGskillData.TeamType _team;
	// Effectiveness of the skill, use determined by effect type
	private float _potency;

	// Constructor
	public LGskill(uint skillID, string name, int effortCost, LGskillData.EffectType effect, LGskillData.TargetType target, LGskillData.TeamType team) {
		// For now generate skillID in order, in future skillID should 
		// be powered by config files
		if (LGskillData.Skills.ContainsKey (skillID)) {
			Debug.LogError ("SkillID already used, use unique skillIDs");
			return;
		}
		_name = name;
		_effect = effect;
		_target = target;
		_team = team;
		_effortCost = effortCost;
		LGskillData.Skills.Add (_skillID,this);
	}

	// TODO: Do exhaustion checks here.
	public bool CanUseSkill(LGcharacter toCheck) {
		int energy = toCheck.GetStat (LGstatData.StatusType.ENERGY);
		int energyDiff = 0;
		if (RNGesus.AttemptRoll (energy, _effortCost, out energyDiff, 3, 1)) {
			return true;
		} else {
			toCheck.AddStat (LGstatData.StatusType.ENERGY,-1);
			if (energyDiff < -energy) {
				return false;
			}
			return true;
		}
		return true;
	}

	public bool TriggerSkill(LGcharacter user, LGcharacter target) {
		return true;
	}

}
