namespace E_Commerce.Apis.Extensions
{
    public static class AddSwaggerExtensions
    {
        public static WebApplication UseSwaggerMiddleWares( this WebApplication app)
        {

            app.UseSwagger();
            app.UseSwaggerUI();

            return app; 

        }



    }
}
