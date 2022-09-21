//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;

//namespace Repository
//{
//    public static class ProgramService
//    {
//        public static void ConfigureApp(WebApplication app)
//        {
//            //
//            app.UseCors("CorsPolicy");
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }
//            app.UseHttpsRedirection();
//            app.UseRouting();
//            app.UseAuthorization();
//            app.UseAuthentication();
//            app.UseStaticFiles();
//        }
//    }
//}
