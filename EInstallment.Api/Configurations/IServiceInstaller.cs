﻿namespace EInstallment.Api.Configurations;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}