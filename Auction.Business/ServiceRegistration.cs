﻿using Auction.Business.DTOs.AuthDTOs;
using Auction.Business.ExternalServices.Implements;
using Auction.Business.ExternalServices.Interfaces;
using Auction.Business.Profiles;
using Auction.Business.Services.Implements;
using Auction.Business.Services.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterDTOValidator>());
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        return services;
    }
}
