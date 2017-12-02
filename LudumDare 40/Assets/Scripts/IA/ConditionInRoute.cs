using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;

namespace Assets.Scripts.IA
{
	[Condition("Scripts/InRoute")]
	[Help("Checks if the guard is in route")]

	public class ConditionInRoute : GOCondition
	{
		public override bool Check()
		{
			Guard guard = gameObject.GetComponent<Guard>();

			return !guard.inRoute;
		}

	}
}
