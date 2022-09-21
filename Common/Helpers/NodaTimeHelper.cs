using AutoMapper;
using NodaTime;

namespace Common.Abstract.Helpers
{
	public static class NodaTimeHelper
	{
		public static readonly DateTimeZone MachineTimeZone = DateTimeZoneProviders.Bcl.GetSystemDefault();
		public static readonly DateTimeZone ApplicationTimeZone = DateTimeZoneProviders.Tzdb["Europe/Stockholm"];//get user current time zone dynamically

		public static Instant Now => SystemClock.Instance.GetCurrentInstant();
		public static ZonedDateTime NowAppZone => Now.InZone(ApplicationTimeZone);
		public static LocalDateTime NowLocal => NowAppZone.LocalDateTime;
		public static OffsetDateTime NowOffset => Now.WithOffset(NowAppZone.Offset);
		public static LocalDate NowLocalDate => NowLocal.Date;

		public static ZonedDateTime? InstantToZonedDateTime(Instant? instant)
		{
			return instant?.InZone(ApplicationTimeZone);
		}

		public static LocalDate Today => new LocalDate(NowAppZone.Year, NowAppZone.Month, NowAppZone.Day);
		public static CalendarSystem CalanderAppZone => NowAppZone.Calendar;
	}


	public class NodaConverterHelper :
		ITypeConverter<LocalDate, DateTime>,
		ITypeConverter<LocalDate?, DateTime?>,
		ITypeConverter<DateTime, LocalDate>,
		ITypeConverter<DateTime?, LocalDate?>
	{
		public DateTime Convert(LocalDate source, DateTime destination, ResolutionContext context)
		{
			return source.AtMidnight().ToDateTimeUnspecified();
		}

		public DateTime? Convert(LocalDate? source, DateTime? destination, ResolutionContext context)
		{
			return source?.AtMidnight().ToDateTimeUnspecified();
		}

		public LocalDate Convert(DateTime source, LocalDate destination, ResolutionContext context)
		{
			return LocalDateTime.FromDateTime(source).Date;
		}

		public LocalDate? Convert(DateTime? source, LocalDate? destination, ResolutionContext context)
		{
			if (source == null)
				return null;

			return LocalDateTime.FromDateTime((DateTime)source).Date;
		}
	}
}
