namespace UnityEngine.UI
{
	public class LoopScrollSendIndexSource : LoopScrollDataSource
	{
		public static readonly LoopScrollSendIndexSource Instance = new LoopScrollSendIndexSource();

		private LoopScrollSendIndexSource()
		{
		}

		public override void ProvideData(Transform transform, int idx)
		{
			transform.SendMessage("ScrollCellIndex", idx);
		}
	}
}
