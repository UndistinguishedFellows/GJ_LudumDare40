using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;

namespace BBUnity.Actions
{

	[Action("Basic/SetGameOver")]
	[Help("Sets a value to a boolean variable")]
	public class ActionGameOver : BasePrimitiveAction
	{
		[InParam("var")]
		[Help("output variable")]
		public GameManager var;

		[InParam("value")]
		[Help("Value")]
		public bool value;

		public override void OnStart()
		{
			if(value) var.GameOver();
			//var.gameOver = value;
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.COMPLETED;
		}
	}
}