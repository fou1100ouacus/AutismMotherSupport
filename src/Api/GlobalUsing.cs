global using Api.Errors;
global using Api.Middlewares;
global using System.Text.Json;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using System;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Builder;
global using System.Linq;
global using Api.Extensions;
global using Microsoft.EntityFrameworkCore;
global using Infrastructure.Data.DbContext;
global using Infrastructure.SeedDataClass;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Mvc;
global using Application.DTOs.AuthDto;
global using Application.UseCases;
global using Application.Interfaces.Repositories;
global using Application.Interfaces.Services;
global using Domain.Models.IdentityUser;
global using Infrastructure.Repositories.AuthRepository;
global using Infrastructure.Services;
global using Microsoft.AspNetCore.Authorization;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi;
global using System.Text;

global using Microsoft.AspNetCore.Identity;
global using Application.Interfaces.UnitOfWork;
global using Infrastructure.UnitOfWork;

global using Application.DTOs.RolesDto;

global using Infrastructure.Repositories.RoleRepository;








