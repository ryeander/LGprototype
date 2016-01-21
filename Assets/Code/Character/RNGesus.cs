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

	public static double PraiseGauss(double mean, double stdDev) {
		double u1 = ThreadSafeRandom.ThisThreadsRandom.NextDouble ();
		double u2 = ThreadSafeRandom.ThisThreadsRandom.NextDouble ();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		return (mean + stdDev * randStdNormal);
	}
}


