using AutoMapper;
using SocialGoal.Domain.Commands;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.SocialGoal.Web.API.Mappings
{

    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {            

            //测试
            Mapper.CreateMap<EquipmentViewModel,Equipment>();

            Mapper.CreateMap<OrgEnterpriseViewModel, OrgEnterprise>();           
            Mapper.CreateMap<TerminalEquipmentViewModel, TerminalEquipment>();
            Mapper.CreateMap<TerminalSimCardViewModel, CreateOrUpdateTerminalSimCardCommand>();
            //Mapper.CreateMap<XViewModel, X()
            //    .ForMember(x => x.PropertyXYZ, opt => opt.MapFrom(source => source.Property1));     
        }
    }
}