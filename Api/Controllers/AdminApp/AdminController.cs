
namespace ImageRecongnitionApi.Controllers.AdminApp
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/admin")]
    public class AdminController : ControllerBase
    {
        private IDashBoard _dashboardRepository;
        private IAllPhotos _allPhotosRepository;
        public AdminController(IDashBoard dashboardRepository, IAllPhotos allPhotosRepository)
        {
            _dashboardRepository = dashboardRepository;
            _allPhotosRepository = allPhotosRepository;
        }
     
        [HttpPost("save")]
        public async Task<IActionResult> Save(DasboardModel model)
        {
            return (await _dashboardRepository.Save(model)).Format(this);
        }

        [HttpPost("get")]
        public async Task<IActionResult> FetchAllPhotos(Pagination pages)
        {
            return (await _allPhotosRepository.FetchAllPhotos(pages)).Format(this);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteProfilePhotos(string Code)
        {
            return (await _allPhotosRepository.DeleteProfilePhotos(Code)).Format(this);
        }
    }
}
