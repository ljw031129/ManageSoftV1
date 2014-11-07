using AutoMapper;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.Mappings
{

    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<UserFormModel, ApplicationUser>();
            Mapper.CreateMap<UserProfileFormModel, UserProfile>();           

            //测试
            Mapper.CreateMap<EquipmentViewModel,Equipment>();

            Mapper.CreateMap<OrgEnterpriseViewModel, OrgEnterprise>();
            Mapper.CreateMap<TerminalSimCardViewModel, TerminalSimCard>();
            Mapper.CreateMap<TerminalEquipmentViewModel, TerminalEquipment>();
            //Mapper.CreateMap<XViewModel, X()
            //    .ForMember(x => x.PropertyXYZ, opt => opt.MapFrom(source => source.Property1));     
        }
    }
}