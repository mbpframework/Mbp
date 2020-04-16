# Mbp
- Mbp是一个.net core 3的web开发框架(https://www.cnblogs.com/mbpframework/) .而如今,.net core已经在大数据,人工智能,容器云化方面取得相当大的进步.借此,本着学习的态度,整合一个基于.net core的开发框架.此框架借鉴了国外优秀开源项目abp vnext,及国内优秀开源框架Osharp的一些思想和实现.
- 欢迎各路开发爱好者加入这个项目.我们将以**Scrum敏捷框架 + azure devops协同工具**的方式进行协作喔,也是为了后期的容器云化做准备.
### Mbp的未来,123方面
#### 1.成为一个rest api的开发框架
**平台提供能力**
- 模块化,包含.net core核心模块和aspnetcore核心模块
- 基于Jwt的统一身份认证
- 基于角色和自定义策略的统一授权系统
- 集成Swagger ui的poco controller.
- 集成ef core
- 提供了Aop机制
- 后续将陆续集成,Hangfire,IentityServer4,NServiceBus,RabbitMQ,SignalR,Redis,ML,ES,Multitenancy,virtualfilesystem等等
- 最后将会实施容器化,使用linux docker container来运行我们的.net core程序.使用K8S来管理我们的micro services.
#### 2.成为一个可视化建模的平台
**平台提供能力**
- 1.包含一个主框架,提供基础主数据的管理功能,比如租户,人员,角色,权限,存储等管理

### Mbp接下来会进行微服务架构设计的改进.具体解决方案将采用优秀开源项目Ocelot集成IentityServer4来做.
- Mbp的前端将暂停开发,学习目前的前端可以参考项目:https://github.com/PanJiaChen/vue-element-admin.
- Mbp将计划在月内加上API网关.
