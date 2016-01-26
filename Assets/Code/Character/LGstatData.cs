using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Static class for initializing and referencing stat types
public static class LGstatData {

	public const int kMinTrait = 1;
	public const int kMaxTrait = 5;
	public const double kMeanTrait = 2.0;
	public const double kTraitDev = 1.0;
	public const int kMaxGenPoints = 6;
	// All Traits are on a pre-buff scale of 1-5
	public enum TraitType {
		ENDURANCE=0, // Your ability to resist damage and negative effects
		PROWRESS=1,  // Your ability in combat, chance to inflict damage
		TOLERANCE=2, // Resistance to negative effects of Spellgas
		CHARISMA=3,  // Your ability to make friends and influence others
		COUNT=4
	}

	public const int kMinStatus = 0;
	public const int kMaxStatus = 5;
	// All Status are on a scale of 0-5. Each does
	// something different if at 0 at start of turn.
	public enum StatusType {
		HEALTH=0, // Your health, determines ability to take damage. AT 0 : Dying, Check for Death
		ENERGY=1, // Your energy, determines ability to use skills. AT 0 : Collapse, remain stunned for rest of the fight
		TOXICITY=2, // Spellgas intoxication, allows you to augment skills. Start at 0. AT 5 : Overdose.
		MORALE=3, // Morale, impacts relationships, alt win/lose condition. AT 0 : Routed, Flee battle TODO: Random table of bad things at 0 morale
		COUNT=4
	}

	public static LGcharacter CharacterGen () 
	{
		LGcharacter Baby = LGcharacter.CreateInstance<LGcharacter>();
		List<int> toGen = new List<int>();
		List<int> lastPass = new List<int> ();
		for (int i = 0; i < (int)TraitType.COUNT; i++) {
			Baby.SetTrait ((TraitType)i, kMinTrait);
			toGen.Add (i);
		}
		toGen.Shuffle ();
		int toSpend = kMaxGenPoints;
		while (toGen.Count > 0) {
			TraitType type = (TraitType)toGen [0];
			lastPass.Add (toGen [0]);
			toGen.RemoveAt (0);
			double gaussRoll = RNGesus.PraiseGauss (kMeanTrait, kTraitDev);
			int newVal = (int)gaussRoll;
			if (newVal > kMaxTrait-1) {
				newVal = kMaxTrait-1;
			}
			if (newVal < kMinTrait) {
				newVal = kMinTrait;
			}
			if (newVal > toSpend) {
				newVal = toSpend;
			}
			if (Baby.AddTrait (type, newVal)) {
				toSpend -= newVal;
			}
		}
		// Do one last pass to spend remaining points randomly.
		// Removing stats that are too full. 
		// TODO: Instead, spend these on specialized traits based on the number of unspent points
		/*
		while (toSpend > 0 && lastPass.Count > 0) {
			int roll = RNGesus.Praise (0, lastPass.Count-1);
			if (Baby.AddTrait ((TraitType)lastPass[roll], 1)) {
				toSpend--;
			} else {
				lastPass.RemoveAt (roll);
			}
		}
		*/
		// Set stats to their default values (max for most, min for Toxicity)
		for (int i = 0; i < (int)StatusType.COUNT; i++) {
			Baby.SetStat ((StatusType)i, kMaxStatus);
		}
		Baby.SetStat (StatusType.TOXICITY, kMinStatus);

		// Roll Random Skills
		Dictionary<uint, LGskill> genSkill = LGskillData.Skills;
		Baby._AttackSkill = genSkill [(uint)RNGesus.Praise (0, genSkill.Count)];
		Baby._ClassSkill = genSkill [(uint)RNGesus.Praise (0, genSkill.Count)];
		Baby._UtilitySkill = genSkill [(uint)RNGesus.Praise (0, genSkill.Count)];
		Baby._AugmentSkill = genSkill [(uint)RNGesus.Praise (0, genSkill.Count)];


		return Baby;
	}
}
