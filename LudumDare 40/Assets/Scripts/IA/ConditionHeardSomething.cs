using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
	[Condition("Perrception/HeardSomething")]
	[Help("Checks if has heard something")]
	public class ConditionHeardSomething : GOCondition
	{
		[OutParam("NoiseTarget")]
		[Help("Noise target last position")]
		public Vector3 noiseTarget;

		private Guard guardScript = null;

		public override bool Check()
		{
			if (guardScript == null)
			{
				guardScript = gameObject.GetComponent<Guard>();
			}
			if (guardScript.search)
			{
				noiseTarget = guardScript.suspiciousSpot;
				return true;
			}
			return false;
		}
	}
}
