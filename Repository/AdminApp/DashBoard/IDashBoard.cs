
namespace Repository
{
    public interface IDashBoard
    {
        Task<Response> Save(DasboardModel model);
    }
}
