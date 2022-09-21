using NodaTime;

namespace Common.Abstract.Entities
{
	public interface ICreatedUpdatedEntity : IUpdatedEntity
	{
		public Instant CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}

	public abstract class CreatedUpdatedEntity : UpdatedEntity, ICreatedUpdatedEntity
	{
		public Instant CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}

	public interface IUpdatedEntity
	{
		public Instant? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}

	public abstract class UpdatedEntity : IUpdatedEntity
	{
		public Instant? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}
