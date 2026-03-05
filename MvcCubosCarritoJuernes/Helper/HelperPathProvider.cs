namespace MvcCubosCarritoJuernes.Helper
{
    
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private IHttpContextAccessor httpContextAccesor;
        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.hostEnvironment = hostEnvironment;
            this.httpContextAccesor = httpContextAccesor;
        }
        public string MapPath(string fileName)
        {
            string rootPath = this.hostEnvironment.WebRootPath;
            return Path.Combine(rootPath, "images", "cubos", fileName);
        }
    }
}
