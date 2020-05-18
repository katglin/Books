namespace Bootstrap
{
    using AutoMapper;

    public static class ApplicationMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg => {
                //cfg.CreateMap<Source, Destination>().ForMember...;
            });
        }
    }
}
