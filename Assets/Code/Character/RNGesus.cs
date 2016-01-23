using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class ThreadSafeRandom
{
	[ThreadStatic] private static Random Local;

	public static Random ThisThreadsRandom
	{
		get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
	}
}

public static class RNGesus
{

	public static void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static int Praise(int min, int max) {
		return ThreadSafeRandom.ThisThreadsRandom.Next (min, max);
	}

	/// <summary>
	/// Rolls a number of dice equal to the attacking stat value against the defending stat value.
	/// Returns true if offsense rolls higher and can output a positive number representing the difference
	/// Returns false if tie or defense rolls higher, will output a negative number representing the difference
	/// Use DieSides at 1 for specific targets and exhaustion rolls
	/// TODO : When crit? Perfect roll, small range, or if you roll better than defRoll's best possible roll
	/// </summary>
	/// <returns><c>true</c>, if offense rolls higher, <c>false</c> otherwise.</returns>
	/// <param name="offStat">Off stat.</param>
	/// <param name="defStat">Def stat.</param>
	/// <param name="diff">Diff.</param>
	/// <param name="dieSides">Die sides.</param>
	public static bool AttemptRoll(int offStat, int defStat, out int diff, int dieSidesOff = 3, int dieSidesDef = 3) {
		int offRoll = 0;
		int defRoll = 0;

		if (dieSidesOff == 1) {
			dieSidesOff = offStat;
		} else {
			for (int i = 0; i < offStat; i++) {
				offRoll += Praise (1, dieSidesOff);
			}
		}
		if (dieSidesDef == 1) {
			defRoll = defStat;
		} else {
			for (int i = 0; i < defStat; i++) {
				defRoll += Praise (1, dieSidesDef);
			}
		}
		diff = offRoll - defRoll;
		if (offRoll > defRoll) {
			return true;
		}
		return false;
	}

	public static double PraiseGauss(double mean, double stdDev) {
		double u1 = ThreadSafeRandom.ThisThreadsRandom.NextDouble ();
		double u2 = ThreadSafeRandom.ThisThreadsRandom.NextDouble ();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		return (mean + stdDev * randStdNormal);
	}
}


