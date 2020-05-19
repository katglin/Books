namespace Bootstrap
{
    using AutoMapper;
    using DTO;
    using ViewModels;

    public static class ApplicationMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AuthorDTO, Author>().ReverseMap();
            });
        }
    }
}
