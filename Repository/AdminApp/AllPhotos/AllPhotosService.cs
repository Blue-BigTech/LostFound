namespace Repository
{

    public class AllPhotosService : IAllPhotos
    {
        private Context db;
        public AllPhotosService(Context _db)
        {
            this.db = _db;
        }

        public async Task<Response> FetchAllPhotos(Pagination pages)
        {
            Response response = new Response();
            var userData = await db.LoadStoredProcedure("[dbo].[FetchProfile]")
                                   .WithSqlParams(("PageSize", pages.PageSize),
                                                  ("PageNum", pages.PageNum),
                                                  ("AspNetUserId", pages.AspNetUserId.Decode()))
                                  .ExecuteStoredProcedureAsync<AllPhotosResponseModel>();
            if (!userData.Any())
                throw new ApplicationException(Message.ErrorMessage);

            response.Data = userData.Serialize();
            response.Total = userData[0].Total;
            return response;
        }


        public async Task<Response> DeleteProfilePhotos(string Code)
        {
            Response response = new Response();
            Infrastructure.Profile profile = db.Profiles.FirstOrDefault(x => x.Id == Code.Decode().ToInt());
            if (profile is null)
                throw new ApplicationException(Message.ErrorMessage);

            profile.IsDelete = true;
            response.Detail = Message.DeletePhotoSuccess;
            db.Update(profile);
            await db.SaveChangesAsync();
            return response;
        }
    }
}
