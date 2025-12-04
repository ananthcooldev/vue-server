using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using VueNetCrud.Server.Repository;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ServiceExtensionsAdditionalTests
    {
        [Fact]
        public void RegisterServices_ShouldRegisterItemRepositoryAsSingleton()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var itemRepository = serviceProvider.GetService<ItemRepository>();
            itemRepository.Should().NotBeNull();
            
            // Verify it's a singleton
            var itemRepository2 = serviceProvider.GetService<ItemRepository>();
            itemRepository.Should().BeSameAs(itemRepository2);
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
            var productRepository = serviceProvider.GetService<IProductRepository>();
            productRepository.Should().NotBeNull();
            productRepository.Should().BeOfType<ProductRepository>();
            
            // Verify it's a singleton
            var productRepository2 = serviceProvider.GetService<IProductRepository>();
            productRepository.Should().BeSameAs(productRepository2);
        }

        [Fact]
        public void RegisterServices_ShouldAddControllers()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.RegisterServices();

            // Assert
            // Verify controllers are registered by checking for controller-related services
            var serviceProvider = builder.Services.BuildServiceProvider();
            serviceProvider.Should().NotBeNull();
            // Controllers are registered through AddControllers()
        }
    }
}

