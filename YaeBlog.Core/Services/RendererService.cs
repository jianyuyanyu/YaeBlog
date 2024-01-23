﻿using System.Collections.Concurrent;
using System.Diagnostics;
using Markdig;
using Microsoft.Extensions.Logging;
using YaeBlog.Core.Abstractions;
using YaeBlog.Core.Exceptions;
using YaeBlog.Core.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace YaeBlog.Core.Services;

public class RendererService(ILogger<RendererService> logger,
    EssayScanService essayScanService,
    MarkdownPipeline markdownPipeline,
    IDeserializer yamlDeserializer,
    EssayContentService essayContentService)
{
    private readonly Stopwatch _stopwatch = new();

    private readonly List<IPreRenderProcessor> _preRenderProcessors = [];

    private readonly List<IPostRenderProcessor> _postRenderProcessors = [];

    public async Task RenderAsync()
    {
        _stopwatch.Start();
        logger.LogInformation("Render essays start.");

        List<BlogContent> contents = await essayScanService.ScanAsync();
        IEnumerable<BlogContent> preProcessedContents =
            PreProcess(contents);

        List<BlogEssay> essays = [];
        await Task.Run(() =>
        {
            foreach (BlogContent content in preProcessedContents)
            {
                MarkdownMetadata? metadata = TryParseMetadata(content);
                BlogEssay essay = new()
                {
                    Title = metadata?.Title ?? content.FileName,
                    FileName = content.FileName,
                    PublishTime = metadata?.Date ?? DateTime.Now,
                    HtmlContent = content.FileContent
                };

                if (metadata?.Tags is not null)
                {
                    essay.Tags.AddRange(metadata.Tags);
                }
                essays.Add(essay);
            }
        });

        ConcurrentBag<BlogEssay> postProcessEssays = [];
        Parallel.ForEach(essays, essay =>
        {

            BlogEssay newEssay = new()
            {
                Title = essay.Title,
                FileName = essay.FileName,
                PublishTime = essay.PublishTime,
                HtmlContent = Markdown.ToHtml(essay.HtmlContent, markdownPipeline)
            };
            newEssay.Tags.AddRange(essay.Tags);

            postProcessEssays.Add(newEssay);
            logger.LogDebug("Render markdown file {}.", newEssay);
        });

        PostProcess(postProcessEssays);

        _stopwatch.Stop();
        logger.LogInformation("Render finished, consuming {} s.",
            _stopwatch.Elapsed.ToString("s\\.fff"));
    }

    public void AddPreRenderProcessor(IPreRenderProcessor processor)
    {
        bool exist = _preRenderProcessors.Any(p => p.Name == processor.Name);

        if (exist)
        {
            throw new InvalidOperationException("There exists one pre-render processor " +
                                                $"with the same name: {processor.Name}.");
        }

        _preRenderProcessors.Add(processor);
    }

    public void AddPostRenderProcessor(IPostRenderProcessor processor)
    {
        bool exist = _postRenderProcessors.Any(p => p.Name == processor.Name);

        if (exist)
        {
            throw new InvalidCastException("There exists one post-render processor " +
                                           $"with the same name: {processor.Name}.");
        }

        _postRenderProcessors.Add(processor);
    }

    private IEnumerable<BlogContent> PreProcess(IEnumerable<BlogContent> contents)
    {
        ConcurrentBag<BlogContent> processedContents = [];

        Parallel.ForEach(contents, content =>
        {
            foreach (IPreRenderProcessor processor in _preRenderProcessors)
            {
                content = processor.Process(content);
            }

            processedContents.Add(content);
        });

        return processedContents;
    }

    private void PostProcess(IEnumerable<BlogEssay> essays)
    {
        Parallel.ForEach(essays, essay =>
        {
            foreach (IPostRenderProcessor processor in _postRenderProcessors)
            {
                essay = processor.Process(essay);
            }

            if (!essayContentService.TryAdd(essay.FileName, essay))
            {
                throw new BlogFileException(
                    $"There are two essays with the same name: '{essay.FileName}'.");
            }

            logger.LogDebug("Post-Process essay: {}.", essay);
        });
    }

    private MarkdownMetadata? TryParseMetadata(BlogContent content)
    {
        string fileContent = content.FileContent.Trim();

        if (!fileContent.StartsWith("---"))
        {
            return null;
        }

        // 移除起始的---
        fileContent = fileContent[3..];

        int lastPos = fileContent.IndexOf("---", StringComparison.Ordinal);
        if (lastPos is -1 or 0)
        {
            return null;
        }

        string yamlContent = fileContent[..lastPos];
        // 返回去掉元数据之后的文本
        lastPos += 3;
        content.FileContent = fileContent[lastPos..];

        try
        {
            MarkdownMetadata metadata =
                yamlDeserializer.Deserialize<MarkdownMetadata>(yamlContent);
            logger.LogDebug("Title: {}, Publish Date: {}.",
                metadata.Title, metadata.Date);

            return metadata;
        }
        catch (YamlException e)
        {
            logger.LogWarning("Failed to parse '{}' metadata: {}", yamlContent, e);
            return null;
        }
    }
}
