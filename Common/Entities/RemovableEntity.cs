using NodaTime;

namespace Common.Abstract.Entities
{
	public abstract class RemovableEntity : IRemovableEntity
	{
		public Instant? Removed { get; set; }
		public string RemovedBy { get; set; }
	}

	public interface IRemovableEntity
	{
		public Instant? Removed { get; set; }
		public string RemovedBy { get; set; }
	}
}
