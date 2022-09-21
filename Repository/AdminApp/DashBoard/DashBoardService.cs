global using Common.Dto;
global using Common.Abstract.Entities;
global using Common.Abstract.Configurations;
global using Common.Abstract.Helpers;
global using Infrastructure;
global using Microsoft.Extensions.Options;
global using Common.Abstract.Extension;
global using Microsoft.AspNetCore.Http;

namespace Repository
{
    public class DashBoardService : IDashBoard
    {
        private Context _db { get; }
        private Jwt _jwt { get; }

        private readonly IMapper _mapper;

        public DashBoardService(IOptions<Jwt> jwt, Context db, IMapper mapper)
        {
            _jwt = jwt.Value;
            _db = db;
            _mapper = mapper;
        }

        public async Task<Response> Save(DasboardModel model)
        {
            Response response = new();
            string path = "";
            if (model.Image != null)
                path = await Extension.FileUpload(model.AspNetUserId.Decode(), model.Image, model.FileName);

            Infrastructure.Profile profileDto = _mapper.Map<Infrastructure.Profile>(model);
            profileDto.ImageURL = path;
            profileDto.CreatedDate = NodaTimeHelper.Now;
            await _db.Profiles.AddAsync(profileDto);
            await _db.SaveChangesAsync();
            response.Message = Message.SuccessMessage;
            return response;

        }
    }
}
