
namespace Repository
{
    public interface IAllPhotos
    {
        Task<Response> FetchAllPhotos(Pagination pages);
        Task<Response> DeleteProfilePhotos(string Code);
    }
}
