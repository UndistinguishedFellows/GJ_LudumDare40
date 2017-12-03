using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

	[Action("Basic/ReturnToPatrol")]
	[Help("Returns to patrol state")]
	public class ConditionReturnToNormalState : GOAction
	{
		private Guard guardScript = null;
			
		public override void OnStart()
		{
			if (guardScript == null)
			{
				guardScript = gameObject.GetComponent<Guard>();
			}
		}

		public override TaskStatus OnUpdate()
		{
			guardScript.search = false;
			return TaskStatus.COMPLETED;
		}
	}
}