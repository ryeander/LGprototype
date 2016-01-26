using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestBuddyChar : MonoBehaviour {
	[SerializeField]
	Text text;

	[SerializeField]
	Text avText;
	float[] totals = new float[(int)LGstatData.TraitType.COUNT];
	int count = 0;
	LGcharacter c;
	// Use this for initialization
	void Start () {
		// Test Mass Character Generation
		MassTest (1000);

		// Test Saving
		/*
		LGskillData.Skills = new Dictionary<uint, LGskill>();
		new LGskill(0,"Dummy1",2,LGskillData.EffectType.WOUND,LGstatData.TraitType.PROWRESS,LGstatData.TraitType.ENDURANCE,LGskillData.TargetType.SINGLE,LGskillData.TeamType.FOE);
		new LGskill(1,"Dummy2",1,LGskillData.EffectType.CHEER,LGstatData.TraitType.CHARISMA,LGstatData.TraitType.CHARISMA,LGskillData.TargetType.ALL,LGskillData.TeamType.FRIEND);
		new LGskill(2,"Dummy3",2,LGskillData.EffectType.DEMORALIZE,LGstatData.TraitType.CHARISMA,LGstatData.TraitType.CHARISMA,LGskillData.TargetType.DUO_RANDOM,LGskillData.TeamType.FOE);		
		new LGskill(3,"TEST",2,LGskillData.EffectType.BRAVERY,LGstatData.TraitType.CHARISMA,LGstatData.TraitType.CHARISMA,LGskillData.TargetType.DUO_RANDOM,LGskillData.TeamType.FOE);		
		LGskillData.SaveSkillsToJSON();
		*/

		// Test Loading Skills
		LGskillData.LoadSkillsFromJSON();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Test ();
		}
	}

	void Test() {
		count++;
		c = LGstatData.CharacterGen ();
		text.text = LGstatData.CharacterGen ().ToString();
		string output = string.Format("AVERAGES OUT OF {0} GEN ",count);
		for (int i = 0; i < (int)LGstatData.TraitType.COUNT; i++) {
			totals [i] += c.GetTrait( (LGstatData.TraitType)i);
			output+=(string.Format ("- {0} : {1} ", (LGstatData.TraitType)i, totals[i]/count));
		}
		avText.text = output;
	}

	void MassTest(int toTest)
	{
		int target = count + toTest;
		while (count < target) {
			Test ();
		}
	}
}
