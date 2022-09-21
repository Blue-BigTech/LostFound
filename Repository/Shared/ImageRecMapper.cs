
namespace Repository.Shared
{
    public abstract class AppProfile : AutoMapper.Profile
    {

        public void CreateMapBothDirections<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
    public class ImageRecMapper : AppProfile
    {
        public ImageRecMapper()
        {
            CreateMap<UserModel, AppUser>();
            CreateMap<DasboardModel, Infrastructure.Profile>()             
             .ForMember(z => z.AspNetUserId, opt => opt.MapFrom(s => s.AspNetUserId.Decode()));

            //.ForMember(z => z.CreatedDate, opt => opt.MapFrom(s => NodaTimeHelper.Now))
        }
    }
}
