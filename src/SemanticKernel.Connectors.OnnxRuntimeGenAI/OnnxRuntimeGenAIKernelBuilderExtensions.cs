﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using philabs.SemanticKernel.Connectors.OnnxRuntimeGenAI;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Extension methods for adding OnnxRuntimeGenAI Text Generation service to the kernel builder.
/// </summary>
public static class OnnxRuntimeGenAIKernelBuilderExtensions
{
    /// <summary>
    /// Add OnnxRuntimeGenAI Chat Completion services to the kernel builder.
    /// </summary>
    /// <param name="builder">The kernel builder.</param>
    /// <param name="modelPath">The generative AI ONNX model path.</param>
    /// <param name="serviceId">The optional service ID.</param>
    /// <param name="decorate">Optional decorator</param>
    /// <returns>The updated kernel builder.</returns>
    public static IKernelBuilder AddOnnxRuntimeGenAIChatCompletion(
        this IKernelBuilder builder,
        string modelPath,
        string? serviceId = null,
        Func<IChatCompletionService, IChatCompletionService>? decorate = null)
    {
        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) =>
        {
            IChatCompletionService onnxRuntimeGenAiChatCompletionService = new OnnxRuntimeGenAIChatCompletionService(
                modelPath: modelPath,
                loggerFactory: serviceProvider.GetService<ILoggerFactory>());
            if (decorate is not null)
            {
                onnxRuntimeGenAiChatCompletionService = decorate(onnxRuntimeGenAiChatCompletionService);
            }
            return onnxRuntimeGenAiChatCompletionService;
        });

        return builder;
    }
}
