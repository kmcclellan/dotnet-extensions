﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.Enrichment;
using Microsoft.Shared.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for setting up the service enrichers in an <see cref="IServiceCollection" />.
/// </summary>
public static class ApplicationEnricherServiceCollectionExtensions
{
    /// <summary>
    /// Adds an instance of the service enricher to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service enricher to.</param>
    /// <returns>The value of <paramref name="services"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddServiceLogEnricher(this IServiceCollection services)
    {
        _ = Throw.IfNull(services);

        return services
            .AddServiceLogEnricher(_ => { });
    }

    /// <summary>
    /// Adds an instance of the service enricher to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service enricher to.</param>
    /// <param name="configure">The <see cref="ApplicationLogEnricherOptions"/> configuration delegate.</param>
    /// <returns>The value of <paramref name="services"/>.</returns>
    /// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
    public static IServiceCollection AddServiceLogEnricher(this IServiceCollection services, Action<ApplicationLogEnricherOptions> configure)
    {
        _ = Throw.IfNull(services);
        _ = Throw.IfNull(configure);

        return services
            .AddStaticLogEnricher<ApplicationLogEnricher>()
            .AddLogEnricherOptions(configure);
    }

    /// <summary>
    /// Adds an instance of the service enricher to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service enricher to.</param>
    /// <param name="section">The <see cref="IConfigurationSection"/> to use for configuring <see cref="ApplicationLogEnricherOptions"/> in the service enricher.</param>
    /// <returns>The value of <paramref name="services"/>.</returns>
    /// <exception cref="ArgumentNullException">Any of the arguments is <see langword="null"/>.</exception>
    public static IServiceCollection AddServiceLogEnricher(this IServiceCollection services, IConfigurationSection section)
    {
        _ = Throw.IfNull(services);
        _ = Throw.IfNull(section);

        return services
            .AddStaticLogEnricher<ApplicationLogEnricher>()
            .AddLogEnricherOptions(_ => { }, section);
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor, typeof(ApplicationLogEnricherOptions))]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "Addressed by [DynamicDependency]")]
    private static IServiceCollection AddLogEnricherOptions(
        this IServiceCollection services,
        Action<ApplicationLogEnricherOptions> configure,
        IConfigurationSection? section = null)
    {
        _ = services.Configure(configure);

        if (section is not null)
        {
            _ = services.Configure<ApplicationLogEnricherOptions>(section);
        }

        return services;
    }
}
