﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeManagement.Infrastructure.BaseRepositories;
using RecipeManagement.Infrastructure.Persistance;

namespace RecipeManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RecipeManagementDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });


            services.AddScoped<IRecipeRepository, RecipeRepository>();
            //services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            return services;
        }
    }
}
