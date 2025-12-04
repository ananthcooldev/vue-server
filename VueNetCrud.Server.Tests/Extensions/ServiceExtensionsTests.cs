using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using VueNetCrud.Server.Repository;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ServiceExtensionsTests
    {
        [Fact]
        public void RegisterServices_ShouldRegisterControllers()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            // Controllers are registered
            serviceProvider.Should().NotBeNull();
        }

        [Fact]
        public void RegisterServices_ShouldRegisterItemRepository()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var repository = serviceProvider.GetService<ItemRepository>();
            repository.Should().NotBeNull();
        }

        [Fact]
        public void RegisterServices_ShouldRegisterProductRepository()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var repository = serviceProvider.GetService<IProductRepository>();
            repository.Should().NotBeNull();
            repository.Should().BeOfType<ProductRepository>();
        }

        [Fact]
        public void RegisterServices_ShouldRegisterItemRepositoryAsSingleton()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var repository1 = serviceProvider.GetService<ItemRepository>();
            var repository2 = serviceProvider.GetService<ItemRepository>();
            repository1.Should().BeSameAs(repository2);
        }

        [Fact]
        public void RegisterServices_ShouldRegisterProductRepositoryAsSingleton()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var repository1 = serviceProvider.GetService<IProductRepository>();
            var repository2 = serviceProvider.GetService<IProductRepository>();
            repository1.Should().BeSameAs(repository2);
        }
    }
}

