using AutoMapper;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.EntityFrameworkCore.PermissionModel;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.Demo.Dto;
using EMS.Application.Contracts.LogService.Dto;
using EMS.Domain.DomainEntities.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mbp.EntityFrameworkCore.Domain;
using EMS.Application.Contracts.Base.Dto;
using EMS.Domain.DomainEntities.Base;
using EMS.Domain.DomainEntities.Train.Plan;
using EMS.Application.Contracts.Train.Dto;
using EMS.Domain.DomainEntities.Train;
using EMS.Application.Contracts.Operation.Dto;
using EMS.Domain.DomainEntities.Operation;
using EMS.Application.Contracts.FileServer.Dto;

namespace EMS.Application
{
    /// <summary>
    /// object to object Map
    /// </summary>
    public static class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            // TO DO 在这里注册所有的类型映射cfg.CreateMap<TSource, TDestination>()
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDto>();
                cfg.CreateMap<Post, PostDto>();

                cfg.CreateMap<BlogDto, Blog>();
                cfg.CreateMap<PostDto, Post>();

                // 用户映射
                cfg.CreateMap<UserInputDto, MbpUser>();
                cfg.CreateMap<MbpUser, UserOutputDto>();

                // 角色映射
                cfg.CreateMap<RoleInputDto, MbpRole>();
                cfg.CreateMap<MbpRole, RoleOutputDto>();

                // 菜单映射
                cfg.CreateMap<MenuInputDto, MbpMenu>();
                cfg.CreateMap<MbpMenu, MenuOutputDto>();

                // 日志映射
                cfg.CreateMap<LogInputDto, MbpOperationLog>();
                cfg.CreateMap<MbpOperationLog, LogOutInputDto>();

                // 路由映射
                cfg.CreateMap<MbpMenu, RouteOutputDto>();

                // 部门映射
                cfg.CreateMap<DeptInputDto, MbpDept>();
                cfg.CreateMap<MbpDept, DeptOutputDto>();

                // 用户部门映射
                cfg.CreateMap<UserDeptInputDto, MbpUserDept>();
                cfg.CreateMap<MbpUserDept, UserDeptOutputDto>();

                // 岗位映射
                cfg.CreateMap<PositionInputDto, MbpPosition>();
                cfg.CreateMap<MbpPosition, PositionOutputDto>();

                // 用户岗位映射
                cfg.CreateMap<UserPositionInputDto, MbpUserPosition>();
                cfg.CreateMap<MbpUserPosition, UserPositionOutputDto>();

                // 训练科目映射
                cfg.CreateMap<TrainSubjectInputDto, EmsTrainSubject>();
                cfg.CreateMap<EmsTrainSubject, TrainSubjectOutputDto>();

                // 训练周计划映射
                cfg.CreateMap<TrainPlanWeekInputDto, EmsTrainPlanWeek>();
                cfg.CreateMap<EmsTrainPlanWeek, TrainPlanWeekOutputDto>();
                cfg.CreateMap<TrainPlanWeekDetailInputDto, EmsTrainPlanWeekDetail>();
                cfg.CreateMap<EmsTrainPlanWeekDetail, TrainPlanWeekDetailOutputDto>();

                // 训练成绩映射
                cfg.CreateMap<TrainScoreInputDto, EmsTrainScore>();
                cfg.CreateMap<EmsTrainScore, TrainScoreOutputDto>();

                // 文件信息映射
                cfg.CreateMap<NoticeInputDto, EmsTrainNotice>();
                cfg.CreateMap<EmsTrainNotice, NoticeOutputDto>();

                // 附件映射
                cfg.CreateMap<AttachmentInputDto, EmsAttachment>();
                cfg.CreateMap<EmsAttachment, AttachmentOutputDto>();
            });

            return config.CreateMapper();
        }
    }
}
