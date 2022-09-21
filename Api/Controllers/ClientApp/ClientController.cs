
namespace ImageRecongnitionApi.Controllers.ClientApp
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/client")]
    public class ClientController : ControllerBase
    {
        private IAllPhotos _allPhotosRepository;
        public ClientController(IAllPhotos allPhotosRepository)
        {
            _allPhotosRepository = allPhotosRepository;
        }

        [HttpPost("get")]
        public async Task<IActionResult> FetchAllPhotos(Pagination pages)
        {
            return (await _allPhotosRepository.FetchAllPhotos(pages)).Format(this);
        }
    }
}
