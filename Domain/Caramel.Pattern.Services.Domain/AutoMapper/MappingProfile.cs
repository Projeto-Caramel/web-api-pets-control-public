using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PetInfoRequest, PetInfo>()
                .ForMember(dest => dest.RegisterDate, opt => opt.Ignore())
                .ForMember(dest => dest.AdoptionDate, opt => opt.Ignore());

            CreateMap<PetRequest, Pet>()
                .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.PartnerId))
                .ForMember(dest => dest.Info, opt => opt.MapFrom(src => src.Info))
                .ForMember(dest => dest.Healthy, opt => opt.MapFrom(src => src.Healthy))
                .ForMember(dest => dest.Caracteristics, opt => opt.MapFrom(src => src.Caracteristics));

            CreateMap<PetInfoEditRequest, PetInfo>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.Base64Image))
                .ForMember(dest => dest.AdoptionDate, opt => opt.MapFrom(src => src.AdoptionDate))
                .ForMember(dest => dest.RegisterDate, opt => opt.Ignore());

            CreateMap<PetEditRequest, Pet>()
               .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.PartnerId))
               .ForMember(dest => dest.Info, opt => opt.Ignore())
               .ForMember(dest => dest.Healthy, opt => opt.MapFrom(src => src.Healthy))
               .ForMember(dest => dest.Caracteristics, opt => opt.MapFrom(src => src.Caracteristics));

            CreateMap<PetGalleryImageRequest, PetGalleryImage>()
               .ForMember(dest => dest.PetId, opt => opt.MapFrom(src => src.PetId))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Base64Image));
        }
    }
}
