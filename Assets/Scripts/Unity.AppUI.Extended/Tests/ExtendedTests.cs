using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.AppUI.Extended.DependencyInjection;
using UnityEngine;
using UnityEngine.TestTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using UnityApp = Unity.AppUI.MVVM;
using MSApp = Microsoft.Extensions.DependencyInjection;

public class ExtendedTests
{
    [Test]
    public void WhenResolveSingleton_AndCreateAndRegisterExtendedAppHost_ThenServiceIsNotNull()
    {
        // Arrange.
        var appBuilder = ExtendedAppBuilder.InstantiateWith<TestApp, UnityApp.UIToolkitHost>();
        appBuilder.Services.AddSingleton<ITestInterface, TestClass1>();

        // Act.
        var provider = appBuilder.Services.BuildServiceProvider();
        var service = provider.GetRequiredService<ITestInterface>();

        // Assert.
        Assert.That(service, Is.Not.Null.And.AssignableTo<ITestInterface>().And.InstanceOf<TestClass1>());
    }

    [Test]
    public void WhenResolveScopeService_AndCreateAndRegisterExtendedAppHost_ThenServiceIsNotNull()
    {
        // Arrange.
        var appBuilder = ExtendedAppBuilder.InstantiateWith<TestApp, UnityApp.UIToolkitHost>();
        appBuilder.Services.AddSingleton<ITestInterface, TestClass1>();
        appBuilder.Services.AddScoped<TestScopedClass>();

        // Act.
        var provider = appBuilder.Services.BuildServiceProvider();
        var service = provider.GetRequiredService<TestScopedClass>();

        // Assert.
        Assert.That(service, Is.Not.Null.And.InstanceOf<TestScopedClass>());
    }


    [Test]
    public void WhenResolveCollection_AndTwoServicesRegistered_ThenServicesResolvedAsCollection()
    {
        // Arrange.
        var appBuilder = ExtendedAppBuilder.InstantiateWith<TestApp, UnityApp.UIToolkitHost>();
        appBuilder.Services.AddSingleton<ITestInterface, TestClass1>();
        appBuilder.Services.AddSingleton<ITestInterface, TestClass2>();
        appBuilder.Services.AddScoped<TestScopedClass>();

        // Act.
        var provider = appBuilder.Services.BuildServiceProvider();
        var service = provider.GetRequiredService<TestScopedClass>();

        // Assert.

        Assert.That(service, Is.Not.Null);
        Assert.That(service, Is.AssignableTo<TestScopedClass>());
    }


    [Test]
    public void WhenResolveGeneric_AndGenericIsRegistered_ThenServiceIsNotNull()
    {
        // Arrange.
        var appBuilder = ExtendedAppBuilder.InstantiateWith<TestApp, UnityApp.UIToolkitHost>();
        appBuilder.Services.AddScoped(typeof(ITestGenericInterface<>), typeof(TestGenericClass<>));

        // Act.
        var provider = appBuilder.Services.BuildServiceProvider();
        var service = provider.GetService(typeof(ITestGenericInterface<TestClass1>));

        // Assert.
        Assert.That(service, Is.Not.Null);
        Assert.That(service, Is.InstanceOf<TestGenericClass<TestClass1>>());
        Assert.That(service, Is.AssignableTo(typeof(ITestGenericInterface<TestClass1>)));
    }


    #region TestClasses

    private class TestApp : UnityApp.App
    {
    }

    private interface ITestInterface
    {
    }


    private class TestClass1 : ITestInterface
    {
    }

    private class TestClass2 : ITestInterface
    {
    }


    private class TestGenericClass<T> : ITestGenericInterface<T>
    {
    }

    private interface ITestGenericInterface<T>
    {
    }

    private class TestScopedClass
    {
        public TestScopedClass(IEnumerable<ITestInterface> testInterfaces) =>
            UnityEngine.Debug.Log($"count = {testInterfaces.Count()}");
    }

    #endregion
}