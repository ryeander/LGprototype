using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		MassTest (1000);
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
