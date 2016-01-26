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

	// Primary trait rolled to determine success or failure of the skill
	private LGstatData.TraitType _sourceTrait;
	private LGstatData.TraitType _targetTrait;

	// Constructor
	public LGskill(uint skillID, string newName, int effortCost, LGskillData.EffectType effect, LGstatData.TraitType sourceT,
		LGstatData.TraitType targetT, LGskillData.TargetType target, LGskillData.TeamType team) {
		// For now generate skillID in order, in future skillID should 
		// be powered by config files
		if (LGskillData.Skills.ContainsKey (skillID)) {
			Debug.LogError ("SkillID already used, use unique skillIDs");
			return;
		}
		_skillID = skillID;
		_name = newName;
		_effect = effect;
		_target = target;
		_team = team;
		_effortCost = effortCost;
		_sourceTrait = sourceT;
		_targetTrait = targetT;
		LGskillData.Skills.Add (_skillID,this);
	}

	public LGskill(LGskillData.SkillStruct newSkill) {
		if (LGskillData.Skills.ContainsKey ((uint)newSkill.skillID)) {
			Debug.LogError ("SkillID already used, use unique skillIDs");
			return;
		}
		_skillID = (uint)newSkill.skillID;
		_name = newSkill.skillName;
		_effect = (LGskillData.EffectType)newSkill.effect;
		_target = (LGskillData.TargetType)newSkill.target;
		_team = (LGskillData.TeamType)newSkill.team;
		_effortCost = newSkill.effortCost;
		_sourceTrait = (LGstatData.TraitType)newSkill.sourceTrait;
		_targetTrait = (LGstatData.TraitType)newSkill.targetTrait;
		LGskillData.Skills.Add (_skillID,this);
	}

	public LGskillData.SkillStruct ToData() {
		LGskillData.SkillStruct data = new LGskillData.SkillStruct 
			((int)_skillID, _name, _effortCost, (int)_effect, (int)_sourceTrait, (int)_targetTrait, (int)_target, (int)_team);
		return data;
	}

	// TODO: Add in Exhaustion Logic/removing characters from fights
	public bool CanUseSkill(LGcharacter toCheck) {
		int energy = toCheck.GetStat (LGstatData.StatusType.ENERGY);
		int energyDiff = 0;
		if (RNGesus.AttemptRoll (energy, _effortCost, out energyDiff, 3, 3)) {
			return true;
		} else {
			// On failed roll, lower energy value by one, collapse if you can't
			if (toCheck.AddStat (LGstatData.StatusType.ENERGY, -1)) {
				// On critical failure, if you roll significantly lower than cost,
				// the move fails and does not get used, but takes up turn/costs energy
				if (energyDiff < -energy*3) {
					return false;
				}
			} else {
				Debug.Log (string.Format("{0} Collapsed", toCheck.Name));
				//TODO: Actually handle character collapse, removal from fight.
				return false;
			}
		}
		return true;
	}

	public bool TriggerSkill(LGcharacter user, LGcharacter target, out bool didCrit) {
		didCrit = false;
		int diff = 0;
		if (RNGesus.AttemptRoll (user.GetTrait (_sourceTrait), target.GetTrait (_targetTrait), out diff)) {
			// HIT
			return true;
		}
		// MISS
		
		return false;
	}

}
